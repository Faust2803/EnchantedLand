using System;

namespace EnchantedLand.Common.DataModels
{
    
    [Serializable]
    public class Characteristics
    {
        public int Strength;
        public int Intelligence;
        public int Agility;
        public int Charisma;
        public int Endurance;
    }
    
    public enum SkillsID
    {
        Stealth = 0,
        Accuracy = 1,
        Destruction = 2,
        Armor = 3,
        Initiative = 4
    }

    public enum CharacteristicsID
    {
        Strength = 0,
        Intelligence = 1,
        Agility = 2,
        Charisma = 3,
        Endurance = 4,
    }
    
    public enum DamageType
    {
        Physical = 0,
        Fire = 1,
        Ice = 2,
        Acid = 3,
        Electricity = 4,
        Paralysis = 5,
        Death = 6,
        Оther = 7,
    }
    
    
    
}