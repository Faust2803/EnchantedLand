using System;
using System.Collections.Generic;
using Common.Scripts.GoogleSheets;
using Common.Scripts.Network;
using Common.Scripts.Tutorial;
using Managers;
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


        [Inject]
        private void Construct(UiManager uiManager, TutorialManager tutorialManager)
        {
            _uiManager = uiManager; 
            _tutorialManager = tutorialManager;
        }

        void Start()
        {
            if (_tutorialManager.CurrentTutorialIndex <= TutorialStep.ByeInShop )
            {
                _uiManager.OpenPanel(PanelType.LobbyMainPanel);
            }
            else if (_tutorialManager.CurrentTutorialIndex <=TutorialStep.EquipItems)
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
