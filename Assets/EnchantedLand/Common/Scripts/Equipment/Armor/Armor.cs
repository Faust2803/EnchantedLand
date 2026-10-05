using System;
using EnchantedLand.Common.DataModels;
using UnityEngine;

namespace EnchantedLand.Common.Armor
{
    [Serializable]
    public class Armor
    {
        public int ID;
        public Sprite Image;
        public string Title;
        public string Description;
        public int Price;
        public Characteristics NeededCharacteristics;
        public Characteristics InfluenceCharacteristics;
        public Skills Skills;
        public Resistance Resistance;
        public int MaxStrength;
        public int CurrentStrength;
        public int Weight;
        public EquipmentSlotID SlotId;
        public bool InShop = true;
    }
    
    [Serializable]
    public class ArmorDTO
    {
        public int ID;
        public int  Image_ID;
        public string Title;
        public string Description;
        public int Price;
        public int Max_Strength;
        public int Current_Strength;
        public int Slot_ID;
        public int Weight;
        public int In_Shop;
        
        public int Need_Strength;
        public int Need_Intelligence;
        public int Need_Agility;
        public int Need_Charisma;
        public int Need_Endurance;
        
        public int Influence_Strength;
        public int IInfluence_Intelligence;
        public int Influence_Agility;
        public int Influence_Charisma;
        public int Influence_Endurance;
        
        public float Resistics_Physical;
        public float Resistics_Fire;
        public float Resistics_Ice;
        public float Resistics_Acid;
        public float Resistics_Electricity;
        public float Resistics_Paralysis;
        public float Resistics_Death;
        
        public int Stealth;
        public int Accuracy;
        public int Desttuction;
        public int Armor;
        public int Initiative;
    }
    
    public enum EquipmentSlotID
    {
        Armor = 0,
        Helm = 1,
        Weapon = 9,
        Shield = 5,
        Boots = 3,
        Gloves = 4,
        Bolt = 2,
        Amulet = 7,
        Ring = 8,
        Cape = 6
    }
}