using System;
using System.Collections.Generic;
using EnchantedLand.Common.DataModels;
using Game.Scripts;
using UnityEngine;

namespace EnchantedLand.Common.Race
{
    public class Race : Configurated
    {
        public int ID { get; set; }
        public Sprite Image;
        public string Title;
        public string Description;
        public Dictionary<CharacteristicsID, int> Characteristics;
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