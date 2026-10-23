
using System;
using System.Collections.Generic;
using Common.Scripts.Player;
using Managers;
using UI.Windows;
using UnityEngine;
using Zenject;

namespace EnchantedLand.Common.Scripts
{
    public class Storage : MonoBehaviour
    {
        [SerializeField]  private List<StorageItem> _storageItem;
        [SerializeField]  private bool _isShop;
        
        private PlayerManager _playerManager;
        private UiManager _uiManager;
        public static Action<bool> ShowEqupErrorWindow;
        
        [Inject]
        private void Construct(PlayerManager playerManager, UiManager uiManager)
        {
            _playerManager = playerManager;
            _uiManager = uiManager;
            
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

        private void RemoveItem(int i, IIdentifiable item)
        {
            if (_isShop)
            {
                _playerManager.RemoveFromStorage(i);
            }
            else
            {
                if (_playerManager.Equip(item, true))
                {
                    _playerManager.RemoveFromStorage(i, true);
                }
                else
                {
                    var data = new MessageWindowData
                    {
                        Title = "Не экипировано",
                        Description = $"Ты не можешь экипировать или использовать {item.Title} . \n" +
                                      "Возможно слот уже занят другим предметом или не хватает характеристик персонажа.",
                        CancelAction = null ,
                        OKAction = () => ShowEqupErrorWindow?.Invoke(false),
                        CancelButtonText = "Cancel",
                        HasCancelButton = false,
                        OkButtonText = "Продолжить игру",
                        HasOKButton = true,
                    };
                    _uiManager.ForceOpenWindow(WindowType.MessageWindow,data);
                    ShowEqupErrorWindow?.Invoke(true);
                }
            }
        }
        
        
        
        
        
        
    }
}