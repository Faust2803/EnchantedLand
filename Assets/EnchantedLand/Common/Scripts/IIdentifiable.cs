using EnchantedLand.Common.Armor;
using UnityEngine;

namespace EnchantedLand.Common.Scripts
{
    public interface IIdentifiable
    {
        public int ID { get; set; }
        public Sprite Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public EquipmentSlotID SlotId { get; set; }
        public bool InShop { get; set; }
        public int Weight { get; set; }
    }

    
}