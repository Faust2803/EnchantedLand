
using System.Collections.Generic;
using Common.Scripts.Player;
using UnityEngine;
using Zenject;

namespace EnchantedLand.Common.Scripts
{
    public class Storage : MonoBehaviour
    {
        [SerializeField]  private List<StorageItem> _storageItem;
        [SerializeField]  private bool _isShop;
        
        private PlayerManager _playerManager;
        
        [Inject]
        private void Construct(PlayerManager playerManager)
        {
            _playerManager = playerManager;
            for (var i = 0; i < _playerManager.Data.Storage.Count; i++)
            {
                if (_playerManager.Data.Storage[i] != null)
                {
                    AddStorageItem(_playerManager.Data.Storage[i]);
                }
            }
            for (var i = 0; i < _storageItem.Count; i++)
            {
                if (!_storageItem[i].ItemData)
                {
                    _storageItem[i].RemoveItem();
                }        
            }
        }
        
        private void AddStorageItem(IIdentifiable item)
        {
            for (var i = 0; i < _storageItem.Count; i++)
            {
                if (!_storageItem[i].ItemData)
                {
                    _storageItem[i].AddItem(item);
                    return;
                }        
            }
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
        private void OnEnable()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            for (var i = 0; i < _storageItem.Count; i++)
            {
                _storageItem[i].OnDoubleClick += RemoveItem;
            }
            _playerManager.Data.OnStorageChanged += OnStorageChanged;
        }

        private void Unsubscribe()
        {
            for (var i = 0; i < _storageItem.Count; i++)
            {
                _storageItem[i].OnDoubleClick -= RemoveItem;
            }
            _playerManager.Data.OnStorageChanged -= OnStorageChanged;
        }
        
        private void OnStorageChanged(int i, IIdentifiable item, bool isAdded)
        {
            if (isAdded)
            {
                _storageItem[i].AddItem(item);
            }
            else
            {
                _storageItem[i].RemoveItem();
                
            }
        }

        private void RemoveItem(int i)
        {
            if (_isShop)
            {
                _playerManager.RemoveFromStorage(i);
            }
            else
            {
                if (_playerManager.Equip(_playerManager.Data.Storage[i], true))
                {
                    _playerManager.RemoveFromStorage(i, true);
                }
            }
        }
        
        
    }
}