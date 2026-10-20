using System;
using System.Collections.Generic;
using System.Linq;
using EnchantedLand.Common.Armor;
using EnchantedLand.Common.DataModels;
using EnchantedLand.Common.Race;
using EnchantedLand.Common.Scripts;


namespace Common.Scripts.Player
{
    [Serializable]
    public class PlayerData
    {
        public const float WEIGHT_COEFFICIENT = 2.5F;
        public const int START_PLAYER_MONEY = 200;
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
        
        
        public int AvailableWeight => LiftingCapacity - Weight;
        
        public void SaveCharacteristics(Race raсe, Characteristics characteristics)
        {
            RaсeId = raсe.ID;
            BaseCharacteristics = characteristics; 
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
                    Count = item.Count
                };
                OnEquipedChanged?.Invoke(index, item, true); 
            }
            return true;
        }

        private int GetFreeEquipmentSlot(EquipmentSlotID slot)
        {
            var i = (int)slot;
            if (_equiped[i] == null) return i;
            if (i + 1 < _equiped.Count && _equiped[i + 1] == null) return i + 1;
            return -1;
        }

        public bool DeEquip(int index, bool isRemoved = false)
        {
            if (index < 0 || index >= _equiped.Count) return false;
            var item = _equiped[index];
            if (item == null) return false;
            _equiped[index] = null;
            EquipedSave[index] = null;
            
            if (isRemoved)
            {
                Weight -= item.Weight;
            }
           
            OnEquipedChanged?.Invoke(index, item, false);
            return true;
        }
    }
    
    [Serializable]
    public class SaveItemData
    {
        public ItemType Type = ItemType.None;
        public int Id = -1;
        public int Count = 0;
    }
    
    
}