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
        // Assign from player prefs
        _currentVolume = PlayerPrefs.GetFloat(_volumeParameter, 1.0f);
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
        // Set audio mixer master volume
        _audioMixer.SetFloat(_volumeParameter, Mathf.Log10(volume) * 20);
    }
}