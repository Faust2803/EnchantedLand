using System.Collections.Generic;
using Common.Scripts.Player;
using Common.Scripts.Tutorial;
using EnchantedLand.Common.DataModels;
using EnchantedLand.Common.Race;
using UI.Windows;

using Zenject;

namespace UI.Panels
{
    public class LobbyMainPanelMediator :BasePanelMediator <LobbyMainPanelView, LobbyMainPanelData>
    {
        private PlayerManager _playerManager;
        private const int FREE_AND_SKILLED = 0;
        private int _free_and_skilled;
        public const int BASE_PARAMS_RESULT = 9;
        
        private const string BASE_DESCRIPTION = "Создай своего героя и отправься навстречу опасным приключениям. Выбери расу,  и распредели характеристики, чтобы определить сильные и слабые стороны своего персонажа.";
        private const string BASE_RASE_DESCRIPTION = "\n\nРАСА";
        private const string BASE_CHARACTERISTICS_DESCRIPTION = "\n\nХАРАКТЕРИСТИКИ";
        
        private TutorialManager _tutorialManager;
        
        [Inject]
        private void Construct(PlayerManager playerManager, TutorialManager tutorialManager)
        {
            _playerManager = playerManager;
            _free_and_skilled = FREE_AND_SKILLED;
           _tutorialManager = tutorialManager;
        }
		
        protected override void ShowStart()
        {
            base.ShowStart();
            Target.StartButton.onClick.AddListener(OnStartButtonClicked);

            for (var i = 0; i < Target.ParamsPanel.Count; i++)
            {
                Target.ParamsPanel[i].OnChange += ChangeParametr;
            }

            Target.RaseSelector.OnRaseSelected += RaseSelected;
            Target.FreeAndSkilledText.text = FREE_AND_SKILLED.ToString();
            RaseSelected(Target.RaseSelector.CurrentRace);
            Target.ScrollRect.verticalNormalizedPosition = 1f;
            
            
            
            
        }

        protected override void OpenFinish()
        {
            base.OpenFinish();
            if (_tutorialManager.CurrentTutorialIndex <= TutorialStep.SelectRace )
            {
                _uiManager.ForceOpenWindow(WindowType.TutorialWindow);
            }
        }

        private void RaseSelected(Race obj)
        {
            var _characteristics_description =
                $"\nРаспредели очки между основными характеристиками персонажа\n " +
                $"Для данной рассы имеются следующие бонусы:\n" +
                $"Сила {obj.Characteristics[CharacteristicsID.Strength]}, " +
                $"Ловкость {obj.Characteristics[CharacteristicsID.Agility]}, " +
                $"Выносливость {obj.Characteristics[CharacteristicsID.Endurance]}, " +
                $"Интеллект {obj.Characteristics[CharacteristicsID.Intelligence]}, " +
                $"Харизма {obj.Characteristics[CharacteristicsID.Charisma]}";
            
            var  _raseDescription = $"\n{obj.Description}";
            
            Target.Description.text = BASE_DESCRIPTION + BASE_RASE_DESCRIPTION  + _raseDescription 
                                      + BASE_CHARACTERISTICS_DESCRIPTION + _characteristics_description
                                      + "\n\nПОДТВЕРЖДЕНИЕ\nПроверь выбранные параметры и заверши создание персонажа, чтобы начать своё путешествие.";
        }

        private void ChangeParametr(CharacteristicsID id, bool isAdded, int counter)
        {
            for (var i = 0; i < Target.ParamsPanel.Count; i++)
            {
                if (id == Target.ParamsPanel[i].CharacteristicId)
                {
                    if (isAdded)
                    {
                        if (_free_and_skilled > 0)
                        {
                            _free_and_skilled--;
                            Target.ParamsPanel[i].SetParams(counter);
                            Target.FreeAndSkilledText.text = _free_and_skilled.ToString();
                        }
                    }
                    else
                    {
                        _free_and_skilled++;
                        Target.ParamsPanel[i].SetParams(counter);
                        Target.FreeAndSkilledText.text = _free_and_skilled.ToString();
                    }
                }
            }

            for (var i = 0; i < Target.ParamsPanel.Count; i++)
            {
                Target.ParamsPanel[i].ButtonsIsActive(_free_and_skilled);
            }
        }

        private void OnStartButtonClicked()
        {
            /*var result = await _sceneLoader.LoadScene(SceneName.Game.ToString(),
                new Progress<float>(value =>
                {
                    Debug.Log($"Loading: {value:P0}");
                })
            );

            if (result)
            {
                CloseSelf();
            }*/
            var characteristics = new Dictionary<CharacteristicsID, int>();
            for (var i = 0; i < Target.ParamsPanel.Count; i++)
            {
                Target.ParamsPanel[i].OnChange -= ChangeParametr;
                switch (Target.ParamsPanel[i].CharacteristicId)
                {
                    case CharacteristicsID.Strength:
                        characteristics[CharacteristicsID.Strength] = Target.ParamsPanel[i].Result;
                        break;
                    case CharacteristicsID.Intelligence:
                        characteristics[CharacteristicsID.Intelligence] = Target.ParamsPanel[i].Result;
                        break;
                    case CharacteristicsID.Agility:
                        characteristics[CharacteristicsID.Agility] = Target.ParamsPanel[i].Result;
                        break;
                    case CharacteristicsID.Charisma:
                        characteristics[CharacteristicsID.Charisma] = Target.ParamsPanel[i].Result;
                        break;
                    case CharacteristicsID.Endurance:
                        characteristics[CharacteristicsID.Endurance] = Target.ParamsPanel[i].Result;
                        break;
                }
            }
            _playerManager.SaveCharacteristicsByRace(Target.RaseSelector.CurrentRace, characteristics);
            
            _uiManager.OpenWindow(WindowType.ShopWindow);
            _uiManager.ClosePanel(PanelType.LobbyMainPanel);
        }
        protected override void CloseStart()
        {
            Target.StartButton.onClick.RemoveListener(OnStartButtonClicked);
            
            Target.RaseSelector.OnRaseSelected -= RaseSelected;
            base.CloseStart();
        }
    }
    
}