using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ValueShower : MonoBehaviour
{

    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text textfield;


    private void Reset()
    {
        slider = GetComponent<Slider>();
        textfield = GetComponent<TMP_Text>();
    }

    public void HandleSliderValueChange(float newValue)
    {
        textfield.SetText(newValue.ToString());
    }
}
