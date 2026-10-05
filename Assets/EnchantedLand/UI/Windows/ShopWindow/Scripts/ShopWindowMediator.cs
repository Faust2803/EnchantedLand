using System;
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

        private int _counter;
        private ItemType _activeType;
        private IIdentifiable _activeItem; 

        [Inject]
        private void Construct(PlayerManager playerManager, ArmorManager armorManager, WeaponManager weaponManager)
        {
            _playerManager = playerManager;
            _armorManager = armorManager;
            _weaponManager = weaponManager;
        }
        
        private void OnCurrencvCange()
        {
            //Target.CurrencyText.text =_playerManager.Data.Currency.ToString();
        }
        
        
		
        protected override void ShowStart()
        {
            base.ShowStart();
            
            for (var i = 0; i < Target.ShopTabToggle.Count; i++)
            {
                Target.ShopTabToggle[i].onValueChanged.AddListener(OnToggleClicked);
            }
            Target.ShopTabToggle[0].isOn = true;
            Target.LeftButton.onClick.AddListener(PrewItem);
            Target.RightButton.onClick.AddListener(NextItem);
            OnToggleClicked(true);
            
            OnCurrencvCange();
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
                    _activeItem = _armorManager.GetByIndex(_counter);
                    break;
                case ItemType.Weapons:
                    if (_weaponManager.Count == _counter) _counter = 0;
                    _activeItem = _weaponManager.GetByIndex(_counter);
                    break;
                case ItemType.Elecsire:
                    
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
                Target.ItemImage = _activeItem.Image;
                Target.PriceText = _activeItem.Price.ToString();
                Target.WeightItemText = _activeItem.Weight.ToString();
            }
            else
            {
                _counter++;
                GetShopItem();
            }
        }

        private enum ItemType
        {
            Armor,
            Weapons,
            Elecsire,
            Other
        }
        
        
        protected override void CloseStart()
        { 
            for (var i = 0; i < Target.ShopTabToggle.Count; i++)
            {
                Target.ShopTabToggle[i].onValueChanged.RemoveListener(OnToggleClicked);
            }
            base.CloseStart();
        }
    }
}