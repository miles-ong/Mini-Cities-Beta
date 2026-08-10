using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace MiniCities.UI
{
    [RequireComponent(typeof(PanelRenderer))]
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }

        [SerializeField] private float fadeDuration;

        private PanelRenderer _panelRenderer;
        
        private VisualElement _overlay;

        private int _uiVersion = -1;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            _panelRenderer = GetComponent<PanelRenderer>();
            _panelRenderer.RegisterUIReloadCallback(OnUIReload);
        }
        
        private void OnDestroy() => _panelRenderer.UnregisterUIReloadCallback(OnUIReload);

        private void OnUIReload(PanelRenderer panelRenderer, VisualElement root, int version)
        {
            if (_uiVersion == version) return;

            _uiVersion = version;
            Debug.Log($"Version Reloaded: Current Version: {_uiVersion}");
            _overlay = root.Q<VisualElement>("FadeOverlay");
        }

        public void LoadScene(string sceneName) => StartCoroutine(TransitionRoutine(sceneName));

        private IEnumerator TransitionRoutine(string sceneName)
        {
            _overlay.pickingMode = PickingMode.Position;
            _overlay.style.opacity = 1f;

            yield return new WaitForSeconds(fadeDuration);

            var load = SceneManager.LoadSceneAsync(sceneName);

            if (load == null) throw new ArgumentNullException(nameof(sceneName), "Scene not found.");
            
            load.allowSceneActivation = false;

            while (load.progress < 0.9f) yield return null;
            
            load.allowSceneActivation = true;

            while (!load.isDone) yield return null;

            yield return null;
            
            _overlay.pickingMode = PickingMode.Position;
            _overlay.style.opacity = 0f;

            yield return new WaitForSeconds(fadeDuration);
            
            _overlay.pickingMode = PickingMode.Ignore;
        }
    }
}