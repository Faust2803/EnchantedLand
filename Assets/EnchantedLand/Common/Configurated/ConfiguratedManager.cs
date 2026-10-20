using System;
using System.Collections.Generic;
using System.Linq;
using Common.Scripts.GoogleSheets;
using Cysharp.Threading.Tasks;
using Game.Scripts;
using UnityEngine;
using Zenject;

public abstract class ConfiguratedManager<T, G>
    where T : Configurated
    where G : new()
{
    private GoogleSheetsService _googleSheetsService;


    protected Dictionary<int, T> _items = new();
    public IReadOnlyCollection<T> Items => _items.Values;
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
        InitData(data);
    }

    protected virtual void InitData(List<G> data)
    {
        _items.Clear();
        foreach (G dto in data)
        {
            T item = Create(dto);
            _items.Add(item.ID, item);
        }
        Debug.Log($"{typeof(T).Name} loaded: {_items.Count}");
        LoadCompleat?.Invoke(true);
    }
    
    
    
    
    
    protected abstract T Create(G dto);
    
    public T GetById(int id)
    {
        T item;
        if(_items.TryGetValue(id, out item))return item;
        
        Debug.LogError($"{typeof(T).Name} with ID {id} not found.");
        return default;
    }
    
    public T GetAt(int index)
    {
        return _items.Values.ElementAt(index);
    }
}
   