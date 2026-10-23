using System.Collections.Generic;
using System.Text;
using Common.Scripts.Player;
using EnchantedLand.Common.DataModels;
using EnchantedLand.Common.Race;
using EnchantedLand.Common.Scripts;
using Managers;
using UI.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EnchantedLand.UI.PlauerUI
{
    public class ActiveEquipmentUI : MonoBehaviour
    {
        [SerializeField] private List<PlauerUI> _plauerUIElement;
        [SerializeField] private GameObject _UIElement;
        [SerializeField] private Button _infoButton;
        private PlayerManager _playerManager;
        private UiManager _uiManager;
        private RaceManager _raceManager;
        
        [Inject]
        private void Construct(PlayerManager playerManager, UiManager uiManager, RaceManager raceManager)
        {
            _playerManager = playerManager;
            _uiManager = uiManager;
            _raceManager = raceManager;
            for (var i = 0; i < _playerManager.Data.Equiped.Count; i++)
            {
                if (_playerManager.Data.Equiped[i] != null)
                {
                    OnEquipedChanged(i, _playerManager.Data.Equiped[i], true);
                }
            }
            for (var i = 0; i < _plauerUIElement.Count; i++)
            {
                if (!_plauerUIElement[i].ItemData)
                {
                    _plauerUIElement[i].RemoveItem();
                }        
            }
        }
        
        private void OnEnable()
        {
            Subscribe();
        }
        
        private void OnDisable()
        {
            Unsubscribe();
        }
        
        private void Subscribe()
        {
            for (var i = 0; i < _plauerUIElement.Count; i++)
            {
                _plauerUIElement[i].OnDoubleClick += RemoveItem;
            }
            _playerManager.Data.OnEquipedChanged += OnEquipedChanged;

            Storage.ShowEqupErrorWindow += WindowOpen;
            _infoButton.onClick.AddListener(OnShowInfoButton);
        }

        private void Unsubscribe()
        {
            for (var i = 0; i < _plauerUIElement.Count; i++)
            {
                _plauerUIElement[i].OnDoubleClick -= RemoveItem;
            }
            _playerManager.Data.OnEquipedChanged -= OnEquipedChanged;
            Storage.ShowEqupErrorWindow -= WindowOpen;
            _infoButton.onClick.RemoveListener(OnShowInfoButton);
        }
        
        private void WindowOpen(bool obj)
        {
            _UIElement.SetActive(!obj);
        }

        private void OnShowInfoButton()
        {
            var builder = new StringBuilder();
            var characteristics = CharacteristicsServise.Characteristics(_playerManager.Data.СurrentCharacteristics);
            var skills = CharacteristicsServise.Skills(_playerManager.Data.Skills);
            var res = CharacteristicsServise.Resistance(_playerManager.Data.Resistics);
            var race = _raceManager.GetById(_playerManager.Data.RaсeId);
            builder.Append("ОБЩИЕ ДАННЫЕ\n");
            builder.Append($"Расса: {race.Title} \n");
            builder.Append($"Очков здоровья: {_playerManager.Data.HP}/{_playerManager.Data.MaxHP} \n");
            builder.Append($"Грузоподъемность : {_playerManager.Data.LiftingCapacity}кг \n");
            builder.Append($"Деньги : {_playerManager.Data.Currency}монет \n");
            builder.Append("ХАРАКТЕРИСТИКИ\n");
            builder.Append($"{characteristics.ToString()}\n");
            builder.Append("УМЕНИЯ\n");
            builder.Append($"{skills.ToString()}\n");
            builder.Append("СОПРОТИВЛЕНИЯ УРОНУ\n");
            builder.Append($"{res.ToString()}\n");
            
            var data = new InfoWindowData
            {
                Title = "Информация о персонаже",
                Description = builder.ToString(),
                Icon = race.Image,
                OKAction = () =>  WindowOpen(false),
            };
            _uiManager.ForceOpenWindow(WindowType.InfoWindow,data);
            WindowOpen(true);
        }

        private void OnEquipedChanged(int index, IIdentifiable item, bool added)
        {
            for (var i = 0; i < _plauerUIElement.Count; i++)
            {
                if (_plauerUIElement[i].SlotID == item.SlotId)
                {
                    if (added)
                    {
                        if (!_plauerUIElement[i].ItemData)
                        {
                            _plauerUIElement[i].AddItem(item);
                            return;
                        }
                    }
                    else
                    {
                        if (index == i)
                        {
                            _plauerUIElement[i].RemoveItem();
                            return;
                        }
                       
                    }
                }
            }
        }
        
        private void RemoveItem(int i, IIdentifiable item)
        {
            if (_playerManager.AddToStorage(item, true, true))
            {
                _playerManager.DeEquip(i, item);
            }
        }
    }
}