using UnityEngine;
using Vuforia;

namespace Experimental
{
    public class RenderBackgroundToggler : MonoBehaviour
    {
        VuforiaConfiguration.VideoBackgroundConfiguration config = null;
        [SerializeField] private bool _toggleOnStart = false;

        private void Start()
        {
            config = VuforiaConfiguration.Instance.VideoBackground;
            ToggleRenderBackground(_toggleOnStart);
        }

        public void ToggleRenderBackground(bool toggle)
        {
            config.VideoBackgroundEnabled = toggle;
        }
    }
}