using System;
using EnchantedLand.Common.DataModels;
using Tanks.Game.Scripts;
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

    public class Damage : Configurated
    {
        public int ID { get; set; }
        public string Title;
        public int Min;
        public int Max;
        public int Radius;
        public DamageType Type;
        public int Opposition;
    }
}