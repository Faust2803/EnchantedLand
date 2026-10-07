using System;
using System.Collections.Generic;
using System.Linq;
using EnchantedLand.Common.Armor;
using EnchantedLand.Common.DataModels;
using EnchantedLand.Common.Race;
using EnchantedLand.Common.Scripts;
using EnchantedLand.Common.Weapon;
using SO.Scripts;
using UnityEngine;
using UnityEngine.Serialization;


namespace Tanks.Common.Scripts.Player
{
    [Serializable]
    public class PlayerData
    {
        private const float WEIGHT_COEFFICIENT = 2.5F;
        public const int START_PLAYER_MONEY = 200;
        private const float CHARISMA_PERCENTAGE = 0.05f;
        public const int ITEM_COUNT_IN_STORAGE = 25;
        
        
        public int Currency;
        public int RaсeId;
        public int LiftingCapacity;
        public int Weight;
        public Characteristics BaseCharacteristics;
        public Characteristics CurrentCharacteristics;
        public List<SaveItemData> StorageSave = Enumerable.Repeat<SaveItemData>(null, ITEM_COUNT_IN_STORAGE).ToList();
        public List<SaveItemData> EquipedSave = new();
        
        private List<IIdentifiable> Storage = Enumerable.Repeat<IIdentifiable>(null, ITEM_COUNT_IN_STORAGE).ToList();
        private List<IIdentifiable> Equiped = new List<IIdentifiable>();
        
        public Action<int, IIdentifiable, bool> OnStorageChanged;
        public Action<int, IIdentifiable, bool> OnEquipedChanged;
        
        public int AvailableWeight => LiftingCapacity - Weight;
        

        public void SaveCharacteristics(Race raсe, Characteristics characteristics)
        {
            RaсeId = raсe.Id;
            BaseCharacteristics = characteristics; 
            CurrentCharacteristics = new Characteristics
            {
                Agility = characteristics.Agility + raсe.Characteristics.Agility,
                Strength = characteristics.Strength + raсe.Characteristics.Strength,
                Endurance = characteristics.Endurance + raсe.Characteristics.Endurance,
                Charisma = characteristics.Charisma + raсe.Characteristics.Charisma,
                Intelligence = characteristics.Intelligence + raсe.Characteristics.Intelligence
            };
            UpdateLiftingCapacity();
        }

        private void UpdateLiftingCapacity()
        {
            LiftingCapacity = Mathf.RoundToInt( (CurrentCharacteristics.Endurance + CurrentCharacteristics.Strength) * WEIGHT_COEFFICIENT);
        }

        public int CalculatePrice(int price)
        {
            var multiplier = 1f - (CurrentCharacteristics.Charisma - 9) * CHARISMA_PERCENTAGE; 
            return Mathf.CeilToInt(price * multiplier);
        }

        public bool AddToStorage(IIdentifiable item, bool isSaved = true)
        {
            if (item == null) return false;
            if (item.Weight > AvailableWeight) return false;
            var index = Storage.FindIndex(x => x == null); 
            if (index < 0) return false;
            Storage[index] = item;
            Weight += item.Weight;

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


        public bool RemoveFromStorage(int index)
        {
            if (index < 0 || index >= Storage.Count) return false;
            var item = Storage[index];
            if (item == null) return false;
            Storage[index] = null;
            StorageSave[index] = null;
            Weight -= item.Weight;
            OnStorageChanged?.Invoke(index, item, false);
            return true;
        }

        public bool Equip(IIdentifiable item, bool isSaved = false)
        {
            return true;
        }

        public bool DeEquip(IIdentifiable item)
        {
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