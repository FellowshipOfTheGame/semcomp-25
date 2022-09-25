using UnityEngine;
using TMPro;

#if !UNITY_WEBGL && !UNITY_EDITOR
using UnityEngine.EventSystems;

public class PasteFromClipboard : MonoBehaviour, IPointerClickHandler
{
#else
public class PasteFromClipboard : MonoBehaviour
{
#endif
    private TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
    }

#if !UNITY_WEBGL && !UNITY_EDITOR
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
        inputField.SetTextWithoutNotify(GUIUtility.systemCopyBuffer);
        inputField.ForceLabelUpdate();
    }

#if !UNITY_WEBGL && !UNITY_EDITOR
    public void OnPointerClick(PointerEventData eventData)
    {
        Paste();
    }
#endif
}
