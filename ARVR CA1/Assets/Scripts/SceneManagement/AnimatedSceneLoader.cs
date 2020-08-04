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
        private static bool _currentlyLoading = true;

        public void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private IEnumerator Start()
        {
            // Wait until animator transition stops playing
            // Normalized time is 1 when clip reaches its end
            yield return new WaitWhile(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1);
            _currentlyLoading = false;
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
            _animator.SetBool(BooleanTargetOnLoad, true);
            yield return new WaitForSeconds(delay);
            SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Single);
        }
    }
};