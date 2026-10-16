using System;

namespace Tanks.Game.Scripts.Entetyes.Loot
{
    public class Loot : Configurated
    {
        public int ID { get; set; }
        
    }
    
    [Serializable]
    public class LootDTO
    {
        public int ID;
        
    }
}