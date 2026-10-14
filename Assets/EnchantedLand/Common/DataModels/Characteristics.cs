using System;

namespace EnchantedLand.Common.DataModels
{
    [Serializable]
    public class Characteristics
    {
        public int Strength = 0;
        public int Intelligence = 0;
        public int Agility = 0;
        public int Charisma = 0;
        public int Endurance = 0;
        
    }
    
    [Serializable]
    public class Skills
    {
        public int Stealth = 0;
        public int Accuracy = 0;
        public int Destruction = 0;
        public int Armor = 9;
        public int Initiative;
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