using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    [RequireComponent(typeof(Animator))]
    public class AnimatedSceneLoader : MonoBehaviour
    {
        [SerializeField] private float _delay = 0.5f;

        [Header("Optional")]
        [Tooltip("The default scene to load when LoadScene() is called")]
        [SerializeField] private string _defaultSceneToLoad = string.Empty;
        public string SceneToLoad { get => _defaultSceneToLoad; set => _defaultSceneToLoad = value; }

        // The boolean parameter on the animator controller to set to true
        private const string BooleanTargetOnLoad = "end";
        private Animator _animator = null;
        private bool _currentlyLoading = false;


        public void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void LoadScene()
        {
            LoadScene(_defaultSceneToLoad, _delay);
        }

        public void LoadScene(string sceneName)
        {
            LoadScene(sceneName, _delay);
        }

        private void LoadScene(string sceneName, float delay)
        {
            // Return if a scene is already currently being loaded
            if (_currentlyLoading)
            {
                return;
            }

            _currentlyLoading = true;
            StartCoroutine(LoadSceneWithTransition(sceneName, delay));
        }

        // Load scene while playing transition animation
        private IEnumerator LoadSceneWithTransition(string targetScene, float delay)
        {
            _animator.SetTrigger(BooleanTargetOnLoad);
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(targetScene);
        }
    }
};