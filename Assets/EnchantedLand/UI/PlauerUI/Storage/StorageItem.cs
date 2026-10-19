using System;
using Common.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace EnchantedLand.Common.Scripts
{
    public class StorageItem : MonoBehaviour
    { 
        [FormerlySerializedAs("ID")] [SerializeField] private int _ID;
        [SerializeField] private TextMeshProUGUI _counter;
        [SerializeField] private Image _image;
        [SerializeField] private DoubleClickButton _button;
        
        private IIdentifiable _itemData;
        public bool ItemData => _itemData != null;
        public Action<int> OnDoubleClick;
        public int ID => _ID;

        public virtual void AddItem(IIdentifiable itemData)
        {
            _itemData = itemData;
            _counter.text = itemData.Count.ToString();
            _image.sprite = itemData.StorageImage;
            _button.OnDoubleClick += OnButtonClicked;
            SetActive(true);
            _counter.gameObject.SetActive(itemData.Count > 1);
        }

        public virtual void RemoveItem()
        {
            _itemData = null;
            _button.OnDoubleClick -= OnButtonClicked;
            SetActive(false);
        }

        private void SetActive(bool value)
        {
            _counter.gameObject.SetActive(value);
            _button.gameObject.SetActive(value);
        }

        public virtual void OnButtonClicked()
        {
            OnDoubleClick?.Invoke(_ID);
        }
        
    }
}