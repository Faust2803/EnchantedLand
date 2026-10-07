using System;
using EnchantedLand.Common.Armor;
using EnchantedLand.Common.Damage;
using EnchantedLand.Common.DataModels;
using EnchantedLand.Common.Scripts;
using UnityEngine;

namespace EnchantedLand.Common.Weapon
{
    [Serializable]
    public class Weapon : IIdentifiable
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
        
        public int MaxStrength;
        public int CurrentStrength;
        public bool TwoHands = false;
        public bool Projectile = false;
        public int Distance;
    
        public Characteristics NeededCharacteristics;
        public Characteristics InfluenceCharacteristics;
        public Skills Skills;
        public DamageDTO [] Damage;
    }
    
    [Serializable]
    public class WeaponDTO
    {
        public int ID;
        public int Image_ID;
        public string Title;
        public string Description;
        public int Price;
        public int Max_Strength;
        public int Current_Strength;
        public int Slot_ID;
        public int Weight;
        public int In_Shop;
        public int Projectile;
        public int Two_Hands;
        public int Distance;
        
        
        public int Need_Strength = 0;
        public int Need_Intelligence = 0;
        public int Need_Agility = 0;
        public int Need_Charisma = 0;
        public int Need_Endurance = 0;
        
        public int Influence_Strength;
        public int IInfluence_Intelligence;
        public int Influence_Agility;
        public int Influence_Charisma;
        public int Influence_Endurance;
        
        public int Damage_ID_0;
        public int Damage_ID_1;
        public int Damage_ID_2;
        
        public int Stealth = 0;
        public int Accuracy = 0;
        public int Desttuction = 0;
        public int Armor = 0;
        public int Initiative = 0;

        public int Count;
    }
}