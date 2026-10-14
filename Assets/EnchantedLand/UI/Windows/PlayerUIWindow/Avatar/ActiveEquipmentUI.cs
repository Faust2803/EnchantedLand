using System.Collections.Generic;
using EnchantedLand.Common.Armor;
using EnchantedLand.Common.Scripts;
using Tanks.Common.Scripts.Player;
using UnityEngine;
using Zenject;

namespace EnchantedLand.UI.PlauerUI
{
    public class ActiveEquipmentUI : MonoBehaviour
    {
        [SerializeField] private List<PlauerUI> _plauerUIElement;
        
        private PlayerManager _playerManager;
        
        [Inject]
        private void Construct(PlayerManager playerManager)
        {
            _playerManager = playerManager;
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
        }

        private void Unsubscribe()
        {
            for (var i = 0; i < _plauerUIElement.Count; i++)
            {
                _plauerUIElement[i].OnDoubleClick -= RemoveItem;
            }
            _playerManager.Data.OnEquipedChanged -= OnEquipedChanged;
        }

        private void OnEquipedChanged(int index, IIdentifiable item, bool added)
        {
            for (var i = 0; i < _plauerUIElement.Count; i++)
            {
                if ((int)_plauerUIElement[i].SlotID + _plauerUIElement[i].ID  == index)
                {
                    if (added)
                    {
                        _plauerUIElement[i].AddItem(item);
                        return;
                    }
                    else
                    {
                        _plauerUIElement[i].RemoveItem();
                        return;
                    }
                }
            }
        }
        
        private void RemoveItem(int i)
        {
            if (_playerManager.Data.AddToStorage(_playerManager.Data.Equiped[i], true, true))
            {
                _playerManager.Data.DeEquip(i);
            }
        }
    }
}