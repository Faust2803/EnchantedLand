using System;
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
        private const string  SheetUrl = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRgOkzycG2abPB1w6LScMWBTGLVAx62vSXtimAjeuA2t2Qp5KyRUScbnuoGO8m9H3hl0J2YRJEn3BTR/pub?gid=0&single=true&output=csv";

        private GoogleSheetsService _googleSheetsService;
        private List<TutorialModel> _tutorials = new();
        public int TutorialCount => _tutorials.Count;
        private TutorialStep _currentTutorialIndex;

        public TutorialStep CurrentTutorialIndex => _currentTutorialIndex;
        public TutorialModel CurrentTutorial => GetTutorial(_currentTutorialIndex);
        public Action OnLoadTutorialData;


        private TutorialManager(GoogleSheetsService googleSheetsService)
        {
            _googleSheetsService = googleSheetsService;
            OnLoadTutorialData += LoadCurrentTutorial;
            LoadTutorialData().Forget();
        }
        

        private async UniTask LoadTutorialData()
        {
            _tutorials = await _googleSheetsService
                .LoadSheet<TutorialModel>(SheetUrl);

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
            OnLoadTutorialData?.Invoke();
            OnLoadTutorialData -= LoadCurrentTutorial;
        }

        // --------------------------------------------------
        // Current tutorial
        // --------------------------------------------------

        private TutorialModel GetTutorial(TutorialStep step)
        {
            var index = (int)step;
            if (index < 0 || index >= _tutorials.Count)
            {
                Debug.LogError($"Tutorial index out of range: {index}");
                return null;
            }
            return _tutorials[index];
        }

        // --------------------------------------------------
        // Save / Load
        // --------------------------------------------------

        public void SaveCurrentTutorial()
        {
            PlayerPrefs.SetInt(CURRENT_TUTORIAL_KEY, (int)_currentTutorialIndex);
            PlayerPrefs.Save();
            Debug.Log($"Current tutorial saved: {_currentTutorialIndex}");
        }

        private void LoadCurrentTutorial()
        {

            if (PlayerPrefs.HasKey(CURRENT_TUTORIAL_KEY))
            {
                _currentTutorialIndex = (TutorialStep)PlayerPrefs.GetInt(CURRENT_TUTORIAL_KEY) + 1;
            }
            else
            {
                _currentTutorialIndex = 0;
            }
            Debug.Log($"Current tutorial loaded: {_currentTutorialIndex}");
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

            if ((int)_currentTutorialIndex >= _tutorials.Count)
            {
                _currentTutorialIndex = (TutorialStep)(_tutorials.Count - 1);
            }
        }

        // --------------------------------------------------
        // Navigation
        // --------------------------------------------------

        public void NextTutorial()
        {
            if (_tutorials.Count == 0) return;
            SaveCurrentTutorial();
            if ((int)_currentTutorialIndex >= _tutorials.Count) return;
            _currentTutorialIndex++;
        }
    }

    public enum TutorialStep
    {
        SelectRace = 0,
        ByeInShop = 1,
        EquipItems = 2
    }
}