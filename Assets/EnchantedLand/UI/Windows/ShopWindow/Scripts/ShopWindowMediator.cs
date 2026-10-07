using System;
using Common.Scripts.Tutorial;
using EnchantedLand.Common.Armor;
using EnchantedLand.Common.Scripts;
using EnchantedLand.Common.Weapon;
using Tanks.Common.Scripts.Player;
using UnityEngine;
using Zenject;

namespace UI.Windows
{
    public class ShopWindowMediator :BaseWindowMediator <ShopWindowView, UIData>
    {
        private PlayerManager _playerManager;
        private ArmorManager _armorManager;
        private WeaponManager _weaponManager;
        private TutorialManager _tutorialManager;

        private int _counter;
        private ItemType _activeType;
        private IIdentifiable _activeItem;
        private int _currentPrise;

        [Inject]
        private void Construct(PlayerManager playerManager,
            ArmorManager armorManager,
            WeaponManager weaponManager,
            TutorialManager tutorialManager
            )
        {
            _playerManager = playerManager;
            _armorManager = armorManager;
            _weaponManager = weaponManager;
            _tutorialManager = tutorialManager;
        }
		
        protected override void ShowStart()
        {
            base.ShowStart();
            Target.ByeButton.onClick.AddListener(OnByeButtonClicked);
            for (var i = 0; i < Target.ShopTabToggle.Count; i++)
            {
                Target.ShopTabToggle[i].onValueChanged.AddListener(OnToggleClicked);
            }
            _playerManager.Data.OnStorageChanged += OnStorageChanged;
            
            Target.ShopTabToggle[0].isOn = true;
            Target.LeftButton.onClick.AddListener(PrewItem);
            Target.RightButton.onClick.AddListener(NextItem);
            
            Target.WeightText = _playerManager.Data.AvailableWeight;
            Target.MoneyText = _playerManager.Data.Currency;
            
            OnToggleClicked(true);
        }

        private void OnByeButtonClicked()
        {
            var currentPrice = _playerManager.Data.CalculatePrice(_activeItem.Price);
            
            if (_playerManager.Data.Currency < currentPrice)
            {
                var data = new MessageWindowData
                {
                    Title = "Нехватает денег",
                    Description = $"Ты не можеш купить этот предмет. Тебе  нехватает {_activeItem.Price - _playerManager.Data.Currency} монет",
                    HasCancelButton = false,
                    OkButtonText = "Продолжить игру",
                    HasOKButton = true,
                };
                _uiManager.ForceOpenWindow(WindowType.MessageWindow, data );
                return;
            }
            
            if (!_playerManager.Data.AddToStorage(_activeItem))
            {
                var data = new MessageWindowData
                {
                    Title = "Невозможно купить",
                    Description = $"Ты не можеш положить этот предмет в хранилище!\n Хранилище полностью заполнено или нехватает грузоподемности!",
                    HasCancelButton = false,
                    OkButtonText = "Продолжить игру",
                    HasOKButton = true,
                };
                _uiManager.ForceOpenWindow(WindowType.MessageWindow, data );
            }
            else
            {
                _playerManager.Data.Currency -= currentPrice;
                Target.MoneyText = _playerManager.Data.Currency;
                _playerManager.Save();
            } 
        }

        private void OnStorageChanged(int index , IIdentifiable item, bool isDeleted)
        {
            Target.WeightText = _playerManager.Data.AvailableWeight;
        }

        private void NextItem()
        {
            _counter++;
            GetShopItem();
        }

        private void PrewItem()
        {
            _counter--;
            GetShopItem();
        }

        private void OnToggleClicked(bool isOn)
        {
            if (isOn)
            {
                for (var i = 0; i < Target.ShopTabToggle.Count; i++)
                {
                    if (Target.ShopTabToggle[i].isOn)
                    {
                        _counter = 0;
                        _activeType = (ItemType)i;
                        GetShopItem();
                    }
                }
            }
        }

        private void GetShopItem()
        {
            switch (_activeType)
            {
                case ItemType.Armor:
                    if (_armorManager.Count == _counter) _counter = 0;
                    if (_counter == -1) _counter = _armorManager.Count -1 ;
                    _activeItem = _armorManager.GetByIndex(_counter);
                    break;
                case ItemType.Weapon:
                    if (_weaponManager.Count == _counter) _counter = 0;
                    if ( _counter == -1) _counter = _weaponManager.Count - 1;
                    _activeItem = _weaponManager.GetByIndex(_counter);
                    break;
                case ItemType.Amulete:
                    break;
                case ItemType.Other:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            ShowItem();
        }

        private void ShowItem()
        {
            if (_activeItem == null) return;
            if (_activeItem.InShop)
            {
                Target.TitleText = _activeItem.Title;
                Target.DescriptionText = _activeItem.Description;
                Target.ItemImage = _activeItem.PreviewImage;
                
                _currentPrise = _playerManager.Data.CalculatePrice(_activeItem.Price);
                Target.PriceText = _currentPrise.ToString();
                    
                Target.WeightItemText = _activeItem.Weight.ToString();
            }
            else
            {
                _counter++;
                GetShopItem();
            }
        }
        
        protected override void CloseStart()
        { 
            for (var i = 0; i < Target.ShopTabToggle.Count; i++)
            {
                Target.ShopTabToggle[i].onValueChanged.RemoveListener(OnToggleClicked);
            }
            _playerManager.Data.OnStorageChanged -= OnStorageChanged;
            base.CloseStart();
        }
    }
}