using System;
using System.Collections.Generic;
using Common.Scripts.GoogleSheets;
using Cysharp.Threading.Tasks;
using EnchantedLand.Common.Damage;
using EnchantedLand.Common.Scripts;
using SO.Scripts;
using Tanks.Common.Scripts.Player;
using UnityEngine;
using Zenject;

public abstract class EquipmentManager<T, G>
    where T : IIdentifiable
    where G : new()
{
    private GoogleSheetsService _googleSheetsService;
    private PlayerManager _playerManager;
    protected AvatarImageService _avatarImageService;
    protected DamageManager DamageManager;
    protected ItemType Type { get; set; }

    private List<T> _items = new();
    
    public IReadOnlyList<T> Items => _items;
    public int Count => _items.Count;
    public Action<ItemType> SetSavedDataAction;

    protected abstract string SheetUrl { get;}

    [Inject]
    private void Construct(
        GoogleSheetsService googleSheetsService,
        AvatarImageService avatarImageService,
        DamageManager damageManager,
        PlayerManager playerManager)
    {
        _googleSheetsService = googleSheetsService;
        _avatarImageService = avatarImageService;
        DamageManager = damageManager;
        _playerManager = playerManager;

        if (DamageManager.Count > 0)
        {
            Load().Forget();
        }
        else
        {
            DamageManager.LoadCompleat += OnDamageLoadComplete;
        }
    }

    private void OnDamageLoadComplete(bool result)
    {
        DamageManager.LoadCompleat -= OnDamageLoadComplete;

        if (result)
            Load().Forget();
    }

    protected virtual async UniTask Load()
    {
        List<G> data = await _googleSheetsService.LoadSheet<G>(SheetUrl);

        if (data == null || data.Count == 0)
        {
            Debug.LogError($"{typeof(T).Name} data is empty.");
            return;
        }
        _items.Clear();
        foreach (G dto in data)
        {
            _items.Add(Create(dto));
        }
        Debug.Log($"{typeof(T).Name} loaded: {_items.Count}");
        SetSavedData();
    }

    private void SetSavedData()
    {
        for (var i = 0; i < _playerManager.Data.StorageSave.Count; i++)
        {
            if (_playerManager.Data.StorageSave[i].Type == Type)
            {
                var item = GetById(_playerManager.Data.StorageSave[i].Id);
                _playerManager.Data.AddToStorage(item, false);
            }
        }
        
        for (var i = 0; i < _playerManager.Data.EquipedSave.Count; i++)
        {
            if (_playerManager.Data.EquipedSave[i].Type == Type)
            {
                var item = GetById(_playerManager.Data.EquipedSave[i].Id);
                _playerManager.Data.Equip(item, false, false);
            }
        }
        
        SetSavedDataAction?.Invoke(Type);
    }
    
    public T GetById(int id)
    {
        foreach (T item in _items)
        {
            if (item.ID == id) return item;
        }

        Debug.LogError($"{typeof(T).Name} with ID {id} not found.");
        return default;
    }

    public T GetByIndex(int index)
    {
        if (index >= 0 && index < _items.Count) return _items[index];

        Debug.LogError($"{typeof(T).Name} with index {index} not found.");
        return default;
    }
    
    

    protected abstract T Create(G dto);
}