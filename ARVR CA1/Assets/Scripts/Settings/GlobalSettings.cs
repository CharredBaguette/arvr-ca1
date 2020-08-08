using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName="Global Settings", menuName="GlobalSettings")]
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
        PlayerPrefs.SetFloat(_volumeParameter, _currentVolume);
    }

    public void SetMasterVolume(float volume)
    {
        _currentVolume = volume;
        
        // Set audio mixer master volume
        _audioMixer.SetFloat(_volumeParameter, Mathf.Log10(volume) * 20);
    }
}