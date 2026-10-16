using System;
using EnchantedLand.Common.Armor;
using UnityEngine;

namespace EnchantedLand.Common.Scripts.Potions
{
    public class Potions : IIdentifiable
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


        public PotionsType SubType;
        public int Impact;
    }
    
    [Serializable]
    public class PotionsDTO
    {
        public int ID;
        public int Image_ID;
        public string Title;
        public string Description;
        public int Price;
        public int In_Shop;
        public int Type;
        public int Count;
        public int Impact;
    }

    public enum PotionsType
    {
        HP,
        Stealth,
        Initiative
    }
}