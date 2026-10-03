using System;
using System.Collections.Generic;
using EnchantedLand.Common.DataModels;
using UnityEngine;

namespace SO.Scripts
{
    [CreateAssetMenu(fileName = "WeaponsConfig", menuName = "Configs/Game/SO Weapons Config", order = 1)]
    public class WeaponsConfig : ScriptableObject
    {
        [SerializeField] private  List<Weapon> _weaponsList = null;

        public Weapon GetWeapons(int type)
        {
            for (var i = 0; i < _weaponsList.Count; i++)
            {
                if (_weaponsList[i].Id == type)
                {
                    return _weaponsList[i];
                }
            }
            Debug.LogWarning("<color=#FF6600>WeaponsConfig: Weapon not found!!!</color>");
            return null;
        }
    }
    
    
    [Serializable]
    public class Weapon
    {
        public int Id;
        public Sprite Image;
        public string Title;
        public string Description;
        public int Price;
        public Characteristics NeededCharacteristics;
        public Skills InfluenceSkills;
        public int MaxStrength;
        public int CurrentStrength;
        public EquipmentSlotID SlotId;
        public bool InShop = true;
        
        public int MinDamage;
        public int MaxDamage;
    }
}