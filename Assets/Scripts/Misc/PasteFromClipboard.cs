using System;
using UnityEngine;
using TMPro;

public class PasteFromClipboard : MonoBehaviour
{
    private TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Paste();
        }
    }

    public void Paste()
    {
        // inputField.interactable = false;
// #if UNITY_ANDROID
//         TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, true);
// #endif
        
#if UNITY_WEBGL
        WebGLCopyAndPasteAPI.GetClipboard(inputField.text);
#else
        inputField.text = GUIUtility.systemCopyBuffer;
#endif
    }
}
