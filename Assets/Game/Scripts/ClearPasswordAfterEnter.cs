using UnityEngine;
using TMPro;

public class ClearPasswordAfterEnter : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    public void ClearInput(string _)
    {
        inputField.text = "";
    }
}
