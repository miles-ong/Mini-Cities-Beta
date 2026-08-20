using System.Collections.Generic;
using MiniCities.Definitions.Blueprint;
using MiniCities.Definitions.Map;
using MiniCities.Systems.Game;
using UnityEngine;
using UnityEngine.UIElements;

namespace MiniCities.UI.Controllers
{
    [RequireComponent(typeof(PanelRenderer))]
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private BlueprintFileRegistry blueprintFileRegistry;
        [SerializeField] private MapPoolDefinition mapPoolDefinition; // remove
        [SerializeField] private BlueprintPoolDefinition blueprintPoolDefinition; // remove
        [SerializeField] private GameSessionContext gameSessionContext;
        [SerializeField] private string gameSceneName = "Game";
        
        private PanelRenderer _panelRenderer;

        private VisualElement _mainMenu;
        private VisualElement _playMenu;
        private VisualElement _optionsMenu;

        private Label _blueprintFileSelectorLabel;
        private IntegerField _seedField;

        private List<BlueprintFileDefinition> _blueprintFiles;
        private int _selectedBlueprintFileIndex;

        private void Awake()
        {
            _panelRenderer = GetComponent<PanelRenderer>();
            _panelRenderer.RegisterUIReloadCallback(OnUIReload);
            _blueprintFiles = blueprintFileRegistry.blueprintFiles;
        }
        
        private void OnDestroy() => _panelRenderer.UnregisterUIReloadCallback(OnUIReload);

        private void OnUIReload(PanelRenderer panelRenderer, VisualElement root, int version)
        {
            _mainMenu = root.Q<VisualElement>("MainMenu");
            _playMenu = root.Q<VisualElement>("PlayMenu");
            _optionsMenu = root.Q<VisualElement>("OptionsMenu");

            root.Q<Button>("PlayButton").clicked += OpenPlayMenu;
            root.Q<Button>("OptionsButton").clicked += OpenOptionsMenu;
            root.Q<Button>("ExitButton").clicked += Application.Quit;

            RegisterPlayMenuContents(root);
            RegisterOptionsMenuContents(root);
        }

        #region Play Menu
        
        private void RegisterPlayMenuContents(VisualElement root)
        {
            root.Q<Button>("BlueprintFileSelectorLeftButton").clicked += () => CycleBlueprintFileSelector(-1);
            root.Q<Button>("BlueprintFileSelectorRightButton").clicked += () => CycleBlueprintFileSelector(1);
            root.Q<Button>("StartGameButton").clicked += StartGame;
            root.Q<Button>("PlayCloseButton").clicked += ClosePlayMenu;

            _blueprintFileSelectorLabel = root.Q<Label>("BlueprintFileSelectorLabel");
            _seedField = root.Q<IntegerField>("SeedIntegerField");

            _selectedBlueprintFileIndex = 0;
            _seedField.value = Random.Range(int.MinValue, int.MaxValue);
                
            RefreshBlueprintFileSelectorLabel();
        }
        
        private void OpenPlayMenu()
        {
            _mainMenu.AddToClassList("mainMenuOpen");
            _playMenu.AddToClassList("sideMenuOpen");
            _optionsMenu.RemoveFromClassList("sideMenuOpen");
        }

        private void ClosePlayMenu()
        {
            _mainMenu.RemoveFromClassList("mainMenuOpen");
            _playMenu.RemoveFromClassList("sideMenuOpen");
        }

        private void CycleBlueprintFileSelector(int direction)
        {
            _selectedBlueprintFileIndex = (_selectedBlueprintFileIndex + direction + _blueprintFiles.Count) % _blueprintFiles.Count;
            RefreshBlueprintFileSelectorLabel();
        }

        private void StartGame()
        {
            var file = _blueprintFiles[_selectedBlueprintFileIndex];
            var seed = _seedField.value;
            
            gameSessionContext.LoadDefinitions(seed, mapPoolDefinition, blueprintPoolDefinition, file);
            SceneLoaderController.Instance.LoadScene(gameSceneName);
        }

        private void RefreshBlueprintFileSelectorLabel() =>
            _blueprintFileSelectorLabel.text = _blueprintFiles[_selectedBlueprintFileIndex].displayName;
        
        #endregion
        
        #region Options Menu

        private void RegisterOptionsMenuContents(VisualElement root)
        {
            root.Q<Button>("OptionsCloseButton").clicked += CloseOptionsMenu;
        }
        
        private void OpenOptionsMenu()
        {
            _mainMenu.AddToClassList("mainMenuOpen");
            _playMenu.RemoveFromClassList("sideMenuOpen");
            _optionsMenu.AddToClassList("sideMenuOpen");
        }

        private void CloseOptionsMenu()
        {
            _mainMenu.RemoveFromClassList("mainMenuOpen");
            _optionsMenu.RemoveFromClassList("sideMenuOpen");
        }
        
        #endregion
    }
}