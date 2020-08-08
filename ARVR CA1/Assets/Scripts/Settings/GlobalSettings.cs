using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Global Settings", menuName = "GlobalSettings")]
public class GlobalSettings : ScriptableObject
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private string _volumeParameter = "Master";
    private float _currentVolume = 1.0f;

    private void OnEnable()
    {
        // Initialize master volume from playerprefs.
        // Sets to default value of 1.0f if playerprefs not found.
        SetMasterVolume(PlayerPrefs.GetFloat(_volumeParameter, 1.0f));
    }

    private void OnDisable()
    {
        // Save volume to playerprefs
        PlayerPrefs.SetFloat(_volumeParameter, _currentVolume);
    }

    /// <summary>
    /// Set master audio mixer's volume parameter
    /// </summary>
    /// <param name="volume"></param>
    public void SetMasterVolume(float volume)
    {
        // Clamp the volume to 0 and 1.0f
        volume = Mathf.Clamp01(volume);
        _currentVolume = volume;

        // Mute audio if volume is 0
        // Note: volume is interpolated between -80 to 0 to match audio mixer range
        if (volume == 0)
        {
            _audioMixer.SetFloat(_volumeParameter, -80.0f);
        }
        else
        {
            // Set audio mixer master volume with logarithmic mapping
            _audioMixer.SetFloat(_volumeParameter, Mathf.Log10(volume) * 20);
        }
    }

    /// <summary>
    /// Quit application and resolve any callbacks
    /// </summary>
    public void QuitApplication()
    {
        Application.Quit();
    }
}