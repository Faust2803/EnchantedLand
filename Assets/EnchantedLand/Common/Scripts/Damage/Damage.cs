using System;
using EnchantedLand.Common.DataModels;
using UnityEngine.Serialization;

namespace EnchantedLand.Common.Damage
{
    [Serializable]
    public class DamageDTO
    {
        public int ID;
        public string Title;
        public int Min;
        public int Max;
        public int Radius;
        public int Type;
        public int Opposition;
        public string For_Wepon;
    }
    
    [Serializable]
    public class Damage
    {
        public int ID;
        public string Title;
        public int Min;
        public int Max;
        public int Radius;
        public DamageType Type;
        public int Opposition;
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