using System;
using EnchantedLand.Common.DataModels;
using SO.Scripts;


namespace Tanks.Common.Scripts.Player
{
    [Serializable]
    public class PlayerData
    {
      
        public int Currency;
        public int RaseId;
        public Characteristics BaseCharacteristics;
        public Characteristics CurrentCharacteristics;
    }
    
    
}