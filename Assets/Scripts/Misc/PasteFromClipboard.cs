using UnityEngine;
using TMPro;

public class PasteFromClipboard : MonoBehaviour
{
    private TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
    }

#if !UNITY_WEBGL
    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Paste();
        }
    }
#endif

    public void Paste()
    {
        inputField.text = GUIUtility.systemCopyBuffer;
    }
}
