using System.Collections.Generic;
using Common.Scripts.GoogleSheets;
using Cysharp.Threading.Tasks;
using Managers;
using UnityEngine;

namespace Common.Scripts.Tutorial
{
    public class TutorialManager
    {
        private const string CURRENT_TUTORIAL_KEY = "CurrentTutorial";
        
        private GoogleSheetsService _googleSheetsService;
        private List<TutorialModel> _tutorials = new();
        public int TutorialCount => _tutorials.Count;
        private int _currentTutorialIndex;

        public int CurrentTutorialIndex => _currentTutorialIndex;
        public TutorialModel CurrentTutorial => GetTutorial(_currentTutorialIndex);


        private TutorialManager(GoogleSheetsService googleSheetsService)
        {
            _googleSheetsService = googleSheetsService;
             LoadCurrentTutorial();
            LoadTutorialData().Forget();
        }

        private async UniTask LoadTutorialData()
        {
            _tutorials = await _googleSheetsService
                .LoadSheet<TutorialModel>(GoogleSheetsURL.Tutorial);

            if (_tutorials == null || _tutorials.Count == 0)
            {
                Debug.LogError("Tutorial data is empty.");
                return;
            }

            // Проверяем сохраненный индекс после загрузки уроков
            ValidateCurrentTutorial();

            Debug.Log(
                $"Tutorials loaded: {_tutorials.Count}. " +
                $"Current tutorial: {_currentTutorialIndex}");
        }

        // --------------------------------------------------
        // Current tutorial
        // --------------------------------------------------

        private TutorialModel GetTutorial(int index)
        {
            if (index < 0 || index >= _tutorials.Count)
            {
                Debug.LogError($"Tutorial index out of range: {index}");
                return null;
            }

            return _tutorials[index];
        }

        public bool TryGetTutorial(
            int index,
            out TutorialModel tutorial)
        {
            if (index >= 0 && index < _tutorials.Count)
            {
                tutorial = _tutorials[index];
                return true;
            }

            tutorial = null;
            return false;
        }

        // --------------------------------------------------
        // Save / Load
        // --------------------------------------------------

        public void SaveCurrentTutorial()
        {
            PlayerPrefs.SetInt(CURRENT_TUTORIAL_KEY, _currentTutorialIndex);
            PlayerPrefs.Save();
            Debug.Log($"Current tutorial saved: {_currentTutorialIndex}");
        }

        private int LoadCurrentTutorial()
        {
            var index  = 0;
            if (PlayerPrefs.HasKey(CURRENT_TUTORIAL_KEY))
            {
                index = PlayerPrefs.GetInt(CURRENT_TUTORIAL_KEY);
                _currentTutorialIndex = index+1;
            }
            else
            {
                _currentTutorialIndex = index;
            }
            Debug.Log($"Current tutorial loaded: {_currentTutorialIndex}");
            return index;
        }

        // --------------------------------------------------
        // Validation
        // --------------------------------------------------

        private void ValidateCurrentTutorial()
        {
            if (_tutorials == null || _tutorials.Count == 0)
            {
                _currentTutorialIndex = 0;
                return;
            }

            if (_currentTutorialIndex < 0)
            {
                _currentTutorialIndex = 0;
            }

            if (_currentTutorialIndex >= _tutorials.Count)
            {
                _currentTutorialIndex = _tutorials.Count - 1;
            }
        }

        // --------------------------------------------------
        // Navigation
        // --------------------------------------------------

        public void NextTutorial()
        {
            if (_tutorials.Count == 0) return;
            if (_currentTutorialIndex >= _tutorials.Count - 1) return;
            SaveCurrentTutorial();
            _currentTutorialIndex++;
        }
    }
}