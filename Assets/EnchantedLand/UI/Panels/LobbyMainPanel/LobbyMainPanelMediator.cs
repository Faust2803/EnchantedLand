using EnchantedLand.Common.Race;
using Tanks.Common.Scripts.Player;
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
        
        [Inject]
        private void Construct(PlayerManager playerManager)
        {
            _playerManager = playerManager;
            _free_and_skilled = FREE_AND_SKILLED;
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
        
        private void RaseSelected(Race obj)
        {
            var _characteristics_description =
                $"\nРаспредели очки между основными характеристиками персонажа\n " +
                $"Для данной рассы имеются следующие бонусы:\n" +
                $"Сила {obj.Characteristics.Strength}, " +
                $"Ловкость {obj.Characteristics.Agility}, " +
                $"Выносливость {obj.Characteristics.Endurance}, " +
                $"Интеллект {obj.Characteristics.Intelligence}, " +
                $"Харизма {obj.Characteristics.Charisma}";
            
            var  _raseDescription = $"\n{obj.Description}";
            
            Target.Description.text = BASE_DESCRIPTION + BASE_RASE_DESCRIPTION  + _raseDescription 
                                      + BASE_CHARACTERISTICS_DESCRIPTION + _characteristics_description
                                      + "\n\nПОДТВЕРЖДЕНИЕ\nПроверь выбранные параметры и заверши создание персонажа, чтобы начать своё путешествие.";
        }

        private void ChangeParametr(int id, bool isAdded, int counter)
        {
            for (var i = 0; i < Target.ParamsPanel.Count; i++)
            {
                if (id == i)
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
            _uiManager.OpenWindow(WindowType.ShopWindow);
        }

        protected override void CloseStart()
        {
            Target.StartButton.onClick.RemoveListener(OnStartButtonClicked);
            for (var i = 0; i < Target.ParamsPanel.Count; i++)
            {
                Target.ParamsPanel[i].OnChange -= ChangeParametr;
            }
            Target.RaseSelector.OnRaseSelected -= RaseSelected;
            base.CloseStart();
        }

        
        
    }
    
}