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
    
    [Serializable]
    public class Resistance
    {
        public float Physical = 0;
        public float Fire = 0;
        public float Ice = 0;
        public float Acid = 0;
        public float Electricity = 0;
        public float Poison = 0;
        public float Paralysis = 0;
        public float Deatch = 0;
    }
    
}