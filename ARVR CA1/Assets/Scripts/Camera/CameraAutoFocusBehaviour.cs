using UnityEngine;
using Vuforia;

/// <summary>
/// Class with camera focus functions for Vuforia.
/// Script partly referenced from : https://developer.vuforia.com/forum/unity-extension-technical-discussion/blurry-camera-image
/// </summary>
namespace Camera
{
    public class CameraAutoFocusBehaviour : MonoBehaviour
    {
        private CameraDevice _cameraDevice;

        private void Start()
        {
            var vuforia = VuforiaARController.Instance;
            vuforia.RegisterVuforiaStartedCallback(OnVuforiaStarted);
            vuforia.RegisterOnPauseCallback(OnPaused);
        }

        private void OnVuforiaStarted()
        {
            CameraDevice.Instance.SetFocusMode(
                CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        }

        private void OnPaused(bool paused)
        {
            if (!paused) // resumed
            {
                // Set again autofocus mode when app is resumed
                CameraDevice.Instance.SetFocusMode(
                    CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
            }
        }
    }
}