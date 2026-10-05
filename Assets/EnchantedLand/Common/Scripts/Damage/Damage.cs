using System;

namespace EnchantedLand.Common.Damage
{
    [Serializable]
    public class DamageDTO
    {
        public int ID;
        public int Min;
        public int Max;
        public int Radius;
        public int Type;
        public int Opposition;
        public string For_Wepon;
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