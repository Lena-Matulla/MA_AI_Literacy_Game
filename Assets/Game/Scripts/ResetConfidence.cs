using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResetConfidence : MonoBehaviour
{
    [SerializeField] TMP_Text m_Text;
    [SerializeField] Slider slider;

    
    private void OnEnable()
    {
        //Debug.Log("ResetConfidence OnEnable");
        slider.SetValueWithoutNotify(slider.minValue);
        m_Text.text = slider.value.ToString("1");
        //Debug.Log($"After reset: {slider.value}");
    }
}
