using System;
using System.Collections.Generic;
using EnchantedLand.Common.DataModels;
using UnityEngine;

namespace SO.Scripts
{
    [CreateAssetMenu(fileName = "ArmorsConfig", menuName = "Configs/Game/SO Armors Config", order = 1)]
    public class ArmorsConfig : ScriptableObject
    {
        [SerializeField] private  List<Armor> _armorsList = null;

        public Armor GetWeapons(int type)
        {
            for (var i = 0; i < _armorsList.Count; i++)
            {
                if (_armorsList[i].Id == type)
                {
                    return _armorsList[i];
                }
            }
            Debug.LogWarning("<color=#FF6600>ArmorsConfig: Armor not found!!!</color>");
            return null;
        }
    }
    
    
    [Serializable]
    public class Armor
    {
        public int Id;
        public Sprite Image;
        public string Title;
        public string Description;
        public int Price;
        public Characteristics NeededCharacteristics;
        public Characteristics InfluenceCharacteristics;
        public Skills Skills;
        public Resistance Resistance;
        public int MaxStrength;
        public int CurrentStrength;
        public int Weight;
        public EquipmentSlotID SlotId;
        public bool InShop = true;

    }

    public enum EquipmentSlotID
    {
        Armor,
        Helm,
        Weapon,
        Shield,
        Boots,
        Gloves,
        Bolt,
        Amulet,
        Ring,
        Cape
    }
}