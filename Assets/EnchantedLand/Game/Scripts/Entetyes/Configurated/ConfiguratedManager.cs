using System;
using System.Collections.Generic;
using Common.Scripts.GoogleSheets;
using Cysharp.Threading.Tasks;
using Tanks.Game.Scripts;
using UnityEngine;
using Zenject;

public abstract class ConfiguratedManager<T, G>
    where T : Configurated
    where G : new()
{
    private GoogleSheetsService _googleSheetsService;


    private List<T> _items = new();
    
    public IReadOnlyList<T> Items => _items;
    public int Count => _items.Count;
    public Action<bool> LoadCompleat;

    protected abstract string SheetUrl { get;}

    [Inject]
    protected virtual void Construct(GoogleSheetsService googleSheetsService)
    {
        _googleSheetsService = googleSheetsService;
    }

    protected async UniTask Load()
    {
        var data = await _googleSheetsService.LoadSheet<G>(SheetUrl);

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
        LoadCompleat?.Invoke(true);
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
    
    protected abstract T Create(G dto);
}
   