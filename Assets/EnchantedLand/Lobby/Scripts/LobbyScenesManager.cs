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
        private bool _isFirstLoad = false;


        [Inject]
        private void Construct(UiManager uiManager, TutorialManager tutorialManager)
        {
            _uiManager = uiManager; 
            _tutorialManager = tutorialManager;
        }

        void Start()
        {
            var tutorial = _tutorialManager.CurrentTutorial;
            if (tutorial.StepId == 0 )
            {
                _isFirstLoad = true;
                _uiManager.OpenWindow(WindowType.TutorialWindow,
                    new TutorialWindowData()
                    {
                        OkAction = OpenStart
                    });
            }
            else
            {
                OpenChosenStart();
            }
        }
        
        private void OpenChosenStart()
        {
            var data = new MessageWindowData
            {
                Title = "Начать игру",
                Description = "Ты можешь начать  новую игру или запустить игру с того места где остановился прошлый раз",
                CancelAction = OpenStart,
                CancelButtonText = "Новая игра",
                HasCancelButton = true,
                OKAction = OpenContinue,
                OkButtonText = "Продолжить игру",
                HasOKButton = true,
            };

            _uiManager.OpenWindow(WindowType.MessageWindow,data);
        }
        
        private void OpenContinue()
        {
            _uiManager.OpenWindow(WindowType.ConnectionWindow);
        }
        
        private void OpenStart()
        {
           _uiManager.OpenPanel(
               PanelType.LobbyMainPanel,
               new LobbyMainPanelData
               {

               });
        }
    }
}
