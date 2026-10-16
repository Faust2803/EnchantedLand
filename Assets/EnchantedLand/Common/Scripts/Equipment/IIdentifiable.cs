using EnchantedLand.Common.Armor;
using UnityEngine;

namespace EnchantedLand.Common.Scripts
{
    public interface IIdentifiable
    {
        public int ID { get; set; }
        public ItemType Type { get; set; }
        public Sprite PreviewImage { get; set; }
        public Sprite StorageImage { get; set; }
        public Sprite EquipedImage { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public EquipmentSlotID SlotId { get; set; }
        public bool InShop { get; set; }
        public int Weight { get; set; }
        public int Count { get; set; }
        
        public int MaxStrength{ get; set; }
        public int CurrentStrength{ get; set; }

    }

    public enum ItemType
    {
        None = -1,
        Armor,
        Weapon,
        Potions,
        Other,
    }
}