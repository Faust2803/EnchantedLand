using Common.Scripts.Player;
using Common.Scripts.Tutorial;
using EnchantedLand.Common.Armor;
using EnchantedLand.Common.Race;
using EnchantedLand.Common.Weapon;
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
        private PlayerManager _playerManager;
        private RaceManager _raceManager;
        private ArmorManager _armorManager;
        private WeaponManager _weaponManager;

        [Inject]
        private void Construct(UiManager uiManager,
            TutorialManager tutorialManager,
            PlayerManager playerManager,
            RaceManager raceManager,
            ArmorManager armorManager,
            WeaponManager weaponManager)
        {
            _uiManager = uiManager; 
            _tutorialManager = tutorialManager;
            _playerManager = playerManager;
            _raceManager = raceManager;
            _armorManager = armorManager;
            _weaponManager = weaponManager;
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
                    OnRaceLoaded(true);
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
                _playerManager.SetCharacteristics(_raceManager.GetById(_playerManager.Data.RaсeId).Characteristics, true);

                if (_weaponManager.Count > 0 )
                {
                    _weaponManager.SetSavedData();
                }
                else
                {
                    _weaponManager.LoadDataAction += WeaponLoaded;

                }
                
                if (_armorManager.Count > 0 )
                {
                    _armorManager.SetSavedData();
                }
                else
                {
                    _armorManager.LoadDataAction += ArmorLoaded;
                }
            }
            else
            {
                Debug.LogError("Race Loaded Failed");
            }
        }

        private void ArmorLoaded()
        {
            _armorManager.LoadDataAction -= ArmorLoaded;
            _armorManager.SetSavedData();
        }

        private void WeaponLoaded()
        {
            _weaponManager.LoadDataAction -= WeaponLoaded;
            _weaponManager.SetSavedData();
        }


        private void OpenChosenStart(WindowType type)
        {
            var data = new MessageWindowData
            {
                Title = "Начать игру",
                Description = "Ты можешь начать  новую игру или запустить игру с того места где остановился прошлый раз",
                CancelAction = CreateNewGame ,
                OKAction = () => _uiManager.OpenWindow(type),
                CancelButtonText = "Новая игра",
                HasCancelButton = true,
                OkButtonText = "Продолжить игру",
                HasOKButton = true,
            };
            _uiManager.OpenWindow(WindowType.MessageWindow,data);
        }

        private void CreateNewGame()
        {
            _playerManager.CreateDefaultData();
            _uiManager.OpenPanel(PanelType.LobbyMainPanel);
        }
    }
}
