using System;
using System.Collections.Generic;
using Common.Scripts.GoogleSheets;
using Common.Scripts.Network;
using Common.Scripts.Tutorial;
using EnchantedLand.Common.Race;
using Managers;
using Tanks.Common.Scripts.Player;
using UI.Panels;
using UI.Windows;
using UnityEngine;
using Zenject;

namespace Tanks.Lobby.Scripts
{
    public class LobbyScenesManager : MonoBehaviour
    {
        private UiManager _uiManager;
        private TutorialManager _tutorialManager;
        private PlayerManager _playerManager;
        private RaceManager _raceManager;

        [Inject]
        private void Construct(UiManager uiManager, TutorialManager tutorialManager, PlayerManager playerManager, RaceManager raceManager)
        {
            _uiManager = uiManager; 
            _tutorialManager = tutorialManager;
            _playerManager = playerManager;
            _raceManager = raceManager;
        }

        void Start()
        {
            if (_tutorialManager.CurrentTutorialIndex <= TutorialStep.ByeInShop )
            {
                _uiManager.OpenPanel(PanelType.LobbyMainPanel);
            }
            else
            {
                if (_raceManager.Count > 0 )
                {
                    _playerManager.SetCharacteristics(_raceManager.GetById(_playerManager.Data.RaсeId));
                }
                else
                {
                    _raceManager.LoadCompleat += OnRaceLoaded;
                }
                
                if (_tutorialManager.CurrentTutorialIndex <=TutorialStep.EquipItems)
                {
                    OpenChosenStart(WindowType.ShopWindow);
                }
                else if (_tutorialManager.CurrentTutorialIndex <= TutorialStep.Connection)
                {
                    OpenChosenStart(WindowType.PlayerUIWindow);
                }
                else
                {
                    OpenChosenStart(WindowType.ConnectionWindow);
                }
            }
            
        }

        private void OnRaceLoaded(bool success)
        {
            if (success)
            {
                _playerManager.SetCharacteristics(_raceManager.GetById(_playerManager.Data.RaсeId));
            }
            else
            {
                Debug.LogError("Race Loaded Failed");
            }
        }

        private void OpenChosenStart(WindowType type)
        {
            var data = new MessageWindowData
            {
                Title = "Начать игру",
                Description = "Ты можешь начать  новую игру или запустить игру с того места где остановился прошлый раз",
                CancelAction = () => _uiManager.OpenPanel(PanelType.LobbyMainPanel),
                OKAction = () => _uiManager.OpenWindow(type),
                CancelButtonText = "Новая игра",
                HasCancelButton = true,
                OkButtonText = "Продолжить игру",
                HasOKButton = true,
            };
            _uiManager.OpenWindow(WindowType.MessageWindow,data);
        }
    }
}
