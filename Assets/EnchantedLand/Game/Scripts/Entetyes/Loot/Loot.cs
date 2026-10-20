using System;
using EnchantedLand.Common.Scripts;

namespace Game.Scripts.Entetyes.Loot
{
    public class Loot : Configurated
    {
        public int ID { get; set; }
        public ItemType Type;
        public int ElementID;
        public int Count;
        public int Prosentage;
    }
    
    [Serializable]
    public class LootDTO
    {
        public int ID;
        public int Type;
        public int Element_ID;
        public int Count;
        public int Prosentage;
    }
}