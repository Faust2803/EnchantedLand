using Common.Scripts.Utils;
using EnchantedLand.Common.Armor;
using EnchantedLand.Common.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EnchantedLand.UI.PlauerUI
{
    public class PlauerUI : StorageItem
    {
        [SerializeField] private EquipmentSlotID _slotID;
        [SerializeField] private TextMeshProUGUI _strength;
        public EquipmentSlotID SlotID => _slotID;
        
        public override void AddItem(IIdentifiable itemData)
        {
           base.AddItem(itemData);
           _strength.text = $"{itemData.MaxStrength}/{itemData.CurrentStrength}";
           _strength.gameObject.SetActive(true);
        }
        
        public override void RemoveItem()
        {
            base.RemoveItem();
            _strength.gameObject.SetActive(false);
        }
        
        public override void OnButtonClicked()
        {
            OnDoubleClick?.Invoke((int)_slotID);
        }
    }
}