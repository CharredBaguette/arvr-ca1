using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Monobehaviour to interface with settings panel.
/// </summary>
public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private string _masterVolumeParameter = "Master";

    public void Start()
    {
        // Initialize master volume slider value from playerprefs
        if (_masterVolumeSlider)
        {
            _masterVolumeSlider.value = PlayerPrefs.GetFloat(_masterVolumeParameter);
        }
    }
}