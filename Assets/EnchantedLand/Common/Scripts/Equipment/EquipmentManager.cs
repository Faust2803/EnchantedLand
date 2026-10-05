using System.Collections.Generic;
using Common.Scripts.GoogleSheets;
using Cysharp.Threading.Tasks;
using EnchantedLand.Common.Damage;
using EnchantedLand.Common.Scripts;
using SO.Scripts;
using UnityEngine;
using Zenject;

public abstract class EquipmentManager<T, G>
    where T : IIdentifiable
    where G : new()
{
    protected GoogleSheetsService _googleSheetsService;
    protected AvatarImageService _avatarImageService;
    protected DamageService _damageService;

    protected List<T> _items = new();

    public IReadOnlyList<T> Items => _items;
    public int Count => _items.Count;

    protected abstract string SheetUrl { get;}

    [Inject]
    private void Construct(
        GoogleSheetsService googleSheetsService,
        AvatarImageService avatarImageService,
        DamageService damageService)
    {
        _googleSheetsService = googleSheetsService;
        _avatarImageService = avatarImageService;
        _damageService = damageService;

        if (_damageService.Count > 0)
        {
            Load().Forget();
        }
        else
        {
            _damageService.LoadCompleat += OnDamageLoadComplete;
        }
    }

    private void OnDamageLoadComplete(bool result)
    {
        _damageService.LoadCompleat -= OnDamageLoadComplete;

        if (result)
            Load().Forget();
    }

    protected async UniTask Load()
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