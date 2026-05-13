using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TrialManager : MonoBehaviour
{
    [Header("Refs")]
    public ImgProvider imageProvider;
    public DataLogger dataLogger;
    public MouseClickPosition interactionLayer;

    [Header("UI Buttons")]
    public Button realButton;
    public Button fakeButton;
    public Button confirmeButton;
    public Button confirmeMarked;

    [Header("Checkbox")]
    [SerializeField] public Toggle toggle;

    [Header("Confidence refs")]
    public GameObject confidencePanel;
    public Slider confidenceSlider;
    public Button confienceButton;

    [Header("MarkedFields")]
    public GameObject Markerpart;
    public TMP_InputField whyInputField;

    private string whytext;
    
    private int _trialIndex = 0;
    private float _trialStartTime = 0f;
    private bool _hasAnsweredThisTrial = false;
    private bool choseFake = false;
    private bool marked = false;

    private Vector2 latestlocal;
    private Vector2 latestnormal;

    private int _realClickedInTrial = 0;
    private int _fakeClickedInTrial = 0;

    //to be able to make the zip file later on, storing the used images there
    public ExportManager exportManager;


    //Saving vars for logging
    private bool _ChoseFake = false;
    private Vector2 _Local = new Vector2(-1000, -1000);
    private Vector2 _Norm  = new Vector2(-1, -1);
    private float _confidence;
    private bool _toggleChecked = false;

    //SELECTION STATES
    //Visual manipulation based on selection
    [Header("Selection Visuals")]
    public Color normalColor = Color.white;
    public Color selectedColor = new Color(0.8f, 0.9f, 1f, 1f);
    public float selectedScale = 1.08f;

    private enum Selection { None, Real, Fake}
    private Selection _selection = Selection.None;

    //Trial states
    //What state of answering it is currently
    private enum TrialState { Answering, Marking,Confidence}
    private TrialState _state = TrialState.Answering;

    //saving trustvalue points per round (computer screen opened, after closing send to overall points and start new)
    [Header("Points when correct")]
    private int ScoreThisRound = 0;
    [SerializeField]
    private int pointUpdate = 10;
    [SerializeField] private TextMeshProUGUI ScoreThisRoundTextField;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        realButton.onClick.AddListener(() => Select(Selection.Real));
        fakeButton.onClick.AddListener(() => Select(Selection.Fake));

        confirmeButton.onClick.AddListener(OnConfirmClicked);
        confienceButton.onClick.AddListener(OnConfidenceOkClicked);
        confirmeMarked.onClick.AddListener(OnConfirmMarkedClicked);
        toggle.onValueChanged.AddListener(delegate { OnToggleChanged(toggle); });


        confidencePanel.SetActive(false);
        ResetSelectionUI();
        StartNewTrial();
    }


    //SELECTION STATES FOR THE BUTTONS
    //Change based on selection, also changes the interaction possibility
    private void Select(Selection selection)
    {
        _selection = selection;
        UpdateSelectionUI();
        //activate interaction if Fake selected
        if (selection == Selection.Fake)
        {
            /*if (!interactionLayer.gameObject.active)
            {
                interactionLayer.gameObject.SetActive(true);
            }*/
            _fakeClickedInTrial++;
        }
        else
        {
           // interactionLayer.gameObject.SetActive(false);
            _realClickedInTrial++;
        }
    }

    //reset
    private void ResetSelectionUI()
    {
        _selection = Selection.None;
        UpdateSelectionUI();
    }

    //updates selection visually
    private void UpdateSelectionUI()
    {

        if(_state == TrialState.Answering)
        {
            //activate ok button if real is slected OR fake + ineteracted
            confirmeButton.interactable = (_selection == Selection.Real || (_selection == Selection.Fake)); //&& interactionLayer.interacted));
        }
        else
        {
            confirmeButton.interactable = false;
        }


        //Visual
        SetButtonVisual(realButton, _selection == Selection.Real);
        SetButtonVisual(fakeButton, _selection == Selection.Fake);

    }

    //Visual for the buttons
    private void SetButtonVisual(Button button, bool selected)
    {
        button.image.color = selected ? selectedColor : normalColor;
        button.transform.localScale = selected ? Vector3.one * selectedScale : Vector3.one;
    }

    private void OnConfirmClicked()
    {
        if (_state != TrialState.Answering) return;

        realButton.interactable = false;
        fakeButton.interactable = false;
        confirmeButton.interactable = false;

        if (_selection == Selection.Real)
        {
            _ChoseFake = false;
            _Local = new Vector2(-1000, -1000);
            _Norm = new Vector2(-1, -1);
            EnterConfienceState();
        }
        else if (_selection == Selection.Fake)
        {
            _ChoseFake = true;
            _state = TrialState.Marking;
            interactionLayer.gameObject.SetActive(true);
            Markerpart.gameObject.SetActive(true);
            whyInputField.interactable = true;
            
        }
        else return;

        //EnterConfienceState();
        
    }


    private void OnToggleChanged(Toggle toggle)
    {
        if (toggle.isOn)
        {
            _toggleChecked = true;
            interactionLayer.gameObject.SetActive(false);
            _Local = new Vector2(-1000, -1000);
            _Norm = new Vector2(-1, -1);
        }
        else
        {
            _toggleChecked = false;
            interactionLayer.gameObject.SetActive(true);
        }
    }

    private void OnConfirmMarkedClicked()
    {
        // marked = true;
        if(interactionLayer.interacted || toggle.isOn)
        {
            if (!toggle.isOn)
            {
                _Local = latestlocal;
                _Norm = latestnormal;
            }

            whytext = whyInputField.text;
            whyInputField.text = "";
            EnterConfienceState();
        }
        
    }

    private void EnterConfienceState()
    {
        _state = TrialState.Confidence;

        //Lock answering UI
        realButton.interactable = false;
        fakeButton.interactable = false;
        confirmeButton.interactable = false;
        interactionLayer.gameObject.SetActive(false);
        toggle.interactable = false;
        confirmeMarked.interactable = false;
        whyInputField.interactable=false;

        //show confidence panel
        confidencePanel.SetActive(true);

    }

    //once ok is clicked, safe the confidence value and proceed to logging
    private void OnConfidenceOkClicked()
    {
        if (_state != TrialState.Confidence) return;
        _confidence = confidenceSlider.value;
        
        FinalizeAndLog();
    }

    //Log with all safe values
    private void FinalizeAndLog()
    {
        bool groundTruthIsFake = imageProvider.currentIsFake;
        string imageName = imageProvider.currentImgName;
        string category = imageProvider.currentCategory;

        int accuracy = (_ChoseFake == groundTruthIsFake) ? 1 : 0;
        int reactionTimeMs = Mathf.RoundToInt((Time.time - _trialStartTime) * 1000f);
        Debug.Log(_confidence);
        //Log
        dataLogger.LogTrial(
            trialIndex: _trialIndex,
            imageName: imageName,
            groundTruthIsFake: groundTruthIsFake,
            userChoseFake: _ChoseFake,
            accuracy: accuracy,
            reactionTimeMs: reactionTimeMs,
            lastLocal: _Local,
            lastNormal: _Norm,
            toggleChecked: _toggleChecked,
            confidence: _confidence,
            realclicked: _realClickedInTrial,
            fakeclicked: _fakeClickedInTrial,
            whyText: whytext,
            currentCategory: category
        );

        //exportManager.RegisterUsedImage(imageProvider.CurrentImgEntry);

        //update the score on the computerscreen
        if(accuracy == 1)
        {
            ScoreThisRound += pointUpdate;
            ScoreThisRoundTextField.text = ScoreThisRound.ToString();
        }


        ExitConfidenceState();
    }

    private void ExitConfidenceState()
    {
        confidencePanel.SetActive (false);
        _state = TrialState.Answering;

        //re enable UI
        realButton.interactable = true;
        fakeButton.interactable= true;
        toggle.interactable = true;
        confirmeMarked.interactable = true;



        StartNewTrial();
        ResetSelectionUI();

    }

    //reset to new trial with new image 
    private void StartNewTrial()
    {
        _trialIndex++;
        _hasAnsweredThisTrial = false;

        imageProvider.LoadRandomImage();
        _trialStartTime = Time.time;
        marked = false;
        latestlocal = new Vector2(-1000, -1000);
        latestnormal = new Vector2(-1, -1);
        interactionLayer.gameObject.SetActive(false);
        _ChoseFake = false;
        _Local = new Vector2(-1000, -1000);
        _Norm = new Vector2(-1, -1);
        _toggleChecked = false;
        _confidence = 0f;
        _realClickedInTrial = 0;
        _fakeClickedInTrial = 0;
        Markerpart.gameObject.SetActive(false);
        toggle.isOn = false;
    }


    //function that updates the click locations when interactable image is clicked

    public void updatemarked(Vector2 ll, Vector2 ln)
    {
        marked = true;
        latestlocal = ll;
        latestnormal = ln;
        UpdateSelectionUI();
    }


    //when home button is clicked, the roundscore is updated on the overall score and reset
    public void exportPointsOnClosed()
    {
        ProgressManager.Instance.AddPoints(ScoreThisRound);
        ScoreThisRound = 0;
        ScoreThisRoundTextField.text = ScoreThisRound.ToString();
    }











    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /*
    private void OnAnswer(bool userChoseFake, Vector2 lastLocal, Vector2 lastNorm)
    {
        if (_hasAnsweredThisTrial) return;
        _hasAnsweredThisTrial = true;


        bool groundTruthIsFake = imageProvider.currentIsFake;
        string imageName = imageProvider.currentImageName;

        int accuracy = (userChoseFake == groundTruthIsFake) ? 1 : 0;
        int reactionTimeMs = Mathf.RoundToInt((Time.time - _trialStartTime) * 1000f);

        //Log
        dataLogger.LogTrial(
            trialIndex: _trialIndex,
            imageName: imageName,
            groundTruthIsFake: groundTruthIsFake,
            userChoseFake: userChoseFake,
            accuracy: accuracy,
            reactionTimeMs: reactionTimeMs,
            lastLocal: lastLocal,
            lastNormal: lastNorm,
            toggleChecked: _toggleChecked,
            confidence: _confidence,
            realclicked: _realClickedInTrial,
            fakeclicked: _fakeClickedInTrial,
            whyText: whytext
        );

        StartNewTrial();
        ResetSelectionUI();
    }


    private void Fake()
    {
        //BUTTON MARKIEREN UND ANDEREN WEG
        //activate interaction if not done already
        if (!interactionLayer.gameObject.active)
        {
            interactionLayer.gameObject.SetActive(true);
        }

        /*if (interactionLayer.interacted)
        {
            OnAnswer(userChoseFake: true, interactionLayer.lastLocal, interactionLayer.lastNormal);
            //interactionLayer.gameObject.SetActive(false);
        }*/
    //}

}
