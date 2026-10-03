using System;
using EnchantedLand.Common.DataModels;

using UnityEngine;

namespace EnchantedLand.Common.Race
{
    [Serializable]
    public class Race
    {
        public int Id;
        public Sprite Image;
        public string Title;
        public string Description;
        public Characteristics Characteristics;
    }
    
    [Serializable]
    public class RaceDTO
    {
        public int Id;
        public int ImageID;
        public string Title;
        public string Description;
        
        public int Strength = 0;
        public int Intelligence = 0;
        public int Agility = 0;
        public int Charisma = 0;
        public int Endurance = 0;
    }
}