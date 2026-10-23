using System;
using System.Collections.Generic;
using System.Linq;
using EnchantedLand.Common.Armor;
using EnchantedLand.Common.DataModels;
using EnchantedLand.Common.Race;
using EnchantedLand.Common.Scripts;
using UnityEngine;


namespace Common.Scripts.Player
{
    [Serializable]
    public class PlayerData
    {
        public const float WEIGHT_COEFFICIENT = 2.5F;
        public const int START_PLAYER_MONEY = 600;
        public const float CHARISMA_PERCENTAGE = 0.05f;
        public const int ITEM_COUNT_IN_STORAGE = 25;
        public const int ITEM_COUNT_IN_EQUIPED = 11;
        public const float HP_COEFFICIENT = 0.5F;
        
        private int Weight;
        
        public int Currency;
        public int RaсeId;
        public int LiftingCapacity;
        public int MaxHP;
        public int HP;
        public Characteristics BaseCharacteristics;
        
        public List<SaveItemData> StorageSave = Enumerable.Repeat<SaveItemData>(null, ITEM_COUNT_IN_STORAGE).ToList();
        public List<SaveItemData> EquipedSave = Enumerable.Repeat<SaveItemData>(null, ITEM_COUNT_IN_EQUIPED).ToList();
        
        private List<IIdentifiable> _storage = Enumerable.Repeat<IIdentifiable>(null, ITEM_COUNT_IN_STORAGE).ToList();
        private List<IIdentifiable> _equiped = Enumerable.Repeat<IIdentifiable>(null, ITEM_COUNT_IN_EQUIPED).ToList();
        public IReadOnlyList<IIdentifiable> Storage => _storage;
        public IReadOnlyList<IIdentifiable> Equiped => _equiped;
        
        public Action<int, IIdentifiable, bool> OnStorageChanged;
        public Action<int, IIdentifiable, bool> OnEquipedChanged;
        
        public Dictionary<CharacteristicsID, int> СurrentCharacteristics { get; set; } = new Dictionary<CharacteristicsID, int>();
        public Dictionary<SkillsID, int> Skills { get; set; } = new Dictionary<SkillsID, int>();
        public Dictionary<SkillsID, int> AddSkills { get; set; } = new Dictionary<SkillsID, int>();
        public Dictionary<DamageType, float> Resistics { get; set; } = new Dictionary<DamageType, float>();
        public float AddResistics { get; set; }
        
        public int AvailableWeight => LiftingCapacity - Weight;

        public void IntiData()
        {
            СurrentCharacteristics[CharacteristicsID.Endurance] = BaseCharacteristics.Endurance;
            СurrentCharacteristics[CharacteristicsID.Intelligence] = BaseCharacteristics.Intelligence;
            СurrentCharacteristics[CharacteristicsID.Strength] = BaseCharacteristics.Strength;
            СurrentCharacteristics[CharacteristicsID.Agility] = BaseCharacteristics.Agility;
            СurrentCharacteristics[CharacteristicsID.Charisma] = BaseCharacteristics.Charisma;
            
            Skills[SkillsID.Accuracy] = 0;
            Skills[SkillsID.Armor] =  0;
            Skills[SkillsID.Initiative] = 0;
            Skills[SkillsID.Destruction] = 0;
            Skills[SkillsID.Stealth] = 0;
            
            AddSkills[SkillsID.Accuracy] = 0;
            AddSkills[SkillsID.Armor] =  0;
            AddSkills[SkillsID.Initiative] = 0;
            AddSkills[SkillsID.Destruction] = 0;
            AddSkills[SkillsID.Stealth] = 0;
            
            Resistics[DamageType.Acid] = 0;
            Resistics[DamageType.Death] = 0;
            Resistics[DamageType.Electricity] = 0;
            Resistics[DamageType.Fire] = 0;
            Resistics[DamageType.Ice] = 0;
            Resistics[DamageType.Physical] = 0;
            Resistics[DamageType.Оther] = 0;
            Debug.Log($"<color=#00FF00>Player Data created</color>");
        }
        
        public void SaveRaceAndBaseCharacteristics(Race raсe , Dictionary<CharacteristicsID, int> characteristics)
        {
            RaсeId = raсe.ID;
            
            BaseCharacteristics.Endurance = characteristics[CharacteristicsID.Endurance]; 
            BaseCharacteristics.Intelligence = characteristics[CharacteristicsID.Intelligence]; 
            BaseCharacteristics.Strength = characteristics[CharacteristicsID.Strength]; 
            BaseCharacteristics.Agility = characteristics[CharacteristicsID.Agility]; 
            BaseCharacteristics.Charisma = characteristics[CharacteristicsID.Charisma];
            IntiData();
        }

        

        public bool AddToStorage(IIdentifiable item, bool isSaved = true, bool isEquiped = false)
        {
            if (item == null) return false;
            var index = _storage.FindIndex(x => x == null); 
            if (index < 0) return false;

            if (!isEquiped)
            {
                if (item.Weight > AvailableWeight) return false;
                Weight += item.Weight;
            }
            
            _storage[index] = item;
           
            if (isSaved)
            {
                StorageSave[index] = new SaveItemData
                {
                    Type = item.Type,
                    Id = item.ID,
                    Count = item.Count
                };
                OnStorageChanged?.Invoke(index, item, true); 
            }
            return true;
        }

        public bool RemoveFromStorage(int index, bool isEquiped = false)
        {
            if (index < 0 || index >= _storage.Count) return false;
            var item = _storage[index];
            if (item == null) return false;
            _storage[index] = null;
            StorageSave[index] = null;
            
            if (!isEquiped)
            {
                Weight -= item.Weight;
            }
           
            OnStorageChanged?.Invoke(index, item, false);
            return true;
        }

        public bool Equip(IIdentifiable item, bool isSaved = false, bool fromStorage = true)
        {
            if (item == null) return false;
            var index = GetFreeEquipmentSlot(item.SlotId);
            if (index == -1) return false;
            
            if (!fromStorage)
            {
                Weight += item.Weight;
            }
            _equiped[index] = item;
           
            if (isSaved)
            {
                EquipedSave[index] = new SaveItemData
                {
                    Type = item.Type,
                    Id = item.ID,
                    Count = item.Count,
                    SlotId = item.SlotId
                };
                OnEquipedChanged?.Invoke(index, item, true); 
            }
            return true;
        }

        private int GetFreeEquipmentSlot(EquipmentSlotID slot)
        {
            var emptyIndex = -1;
            var ringCount = 0;

            for (var i = 0; i < _equiped.Count; i++)
            {
                if (_equiped[i] == null)
                {
                    emptyIndex = i;
                    continue;
                }
                
                if (_equiped[i].SlotId == EquipmentSlotID.None)
                {
                    if (emptyIndex == -1)
                        emptyIndex = i;

                    continue;
                }

                if (_equiped[i].SlotId != slot)
                    continue;

                if (slot != EquipmentSlotID.Ring)
                    return -1;

                ringCount++;

                if (ringCount >= 2)
                    return -1;
            }

            return emptyIndex;
        
        }

        public bool DeEquip(int slotIndex, IIdentifiable item, bool isRemoved = false)
        {
            if (item == null) return false;
            for (var i = 0; i < _equiped.Count(); i++)
            {
                if (_equiped[i] != null && _equiped[i].ID == item.ID)
                {
                    _equiped[i] = null;
                    EquipedSave[i] = new SaveItemData();
                    OnEquipedChanged?.Invoke(slotIndex, item, false);
                    if (isRemoved)
                    {
                        Weight -= item.Weight;
                    }
                    return true;
                }
            }
            return false;
        }
    }
    
    [Serializable]
    public class SaveItemData
    {
        public ItemType Type = ItemType.None;
        public int Id = -1;
        public EquipmentSlotID SlotId;
        public int Count = 0;
    }
    
    
}