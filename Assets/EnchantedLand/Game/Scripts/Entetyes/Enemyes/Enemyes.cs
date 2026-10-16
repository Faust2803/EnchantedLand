using System;
using System.Collections.Generic;
using EnchantedLand.Common.DataModels;
using Tanks.Game.Scripts.Entetyes.Loot;
using UnityEngine;

namespace Tanks.Game.Scripts
{
    public class Enemy : Configurated
    {
        public int ID { get; set; }
        public Sprite Icon;
        public string Title;
        public EnemyType Type;
        public int Level;
        public string Description;
        public int Armor;
        public int HP;
        public int AttakeBonus;
        public Dictionary<DamageType, float> Resistance;
        public int Initiative;
        public List<EnchantedLand.Common.Damage.Damage> Damage;
        public List<int> DamageDistance;
        
        public int EscapeBonus;
        public int PoltrooneryBonus;
        public List<Loot> Loot;
    }

    public enum EnemyType
    {
        Dialogue = 0,
        NotDialogue = 1,
        Dragons = 2,
        CanDialogue = 3,
        Animals = 4,
        Undeds = 5,
        Humans = 6,
        Warloks = 7
        
    }
    
    [Serializable]
    public class EnemyDTO
    {
        public int ID;
        public int Icon_ID;
        public string Title;
        public int Type;
        public int Level;
        public string Description;
        public int Armor;
        public int HP;
        public int Attake_bonus;
        public float Resistics_Physical;
        public float Resistics_Fire;
        public float Resistics_Ice;
        public float Resistics_Acid;
        public float Resistics_Electricity;
        public float Resistics_Paralysis;
        public float Resistics_Death;
        public float Resistics_Spesial;
        public int Initiative;
        public int Attack_Distance_0; 
        public int Attack_ID_0;
        public int Attack_Distance_1;
        public int Attack_ID_1;
        public int Attack_Distance_2;
        public int Attack_ID_2;
        public int Escape_Bonus;
        public int Poltroonery_Bonus;
        public int Loot_ID_0;
        public int Loot_ID_1;
        public int Loot_ID_2;
    }
}