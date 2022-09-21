using UnityEngine;
using UnityEngine.UI;
 
public class ScrollWithSlider: MonoBehaviour
{
    [SerializeField] private ScrollRect rect;
    [SerializeField] private Slider slider;
 
    private void OnEnable()
    {
        slider.onValueChanged.AddListener(UpdateRectValue);
        rect.onValueChanged.AddListener(UpdateSliderValue);
    }
 
    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(UpdateRectValue);
        rect.onValueChanged.RemoveListener(UpdateSliderValue);
    }
 
    private void UpdateRectValue(float value)
    {
        // Here I flip the value in the code instead of trying to rotate the UI element itself since it's easier for me :P
        rect.verticalNormalizedPosition = value;
    }
 
    private void UpdateSliderValue(Vector2 scrollPosition)
    {
        // Again, flippin the value for visual consistency
        slider.SetValueWithoutNotify(scrollPosition.y);
    }
}