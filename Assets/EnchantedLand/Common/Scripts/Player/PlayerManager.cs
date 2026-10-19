using System;
using System.Collections.Generic;
using System.IO;
using EnchantedLand.Common.Armor;
using EnchantedLand.Common.DataModels;
using EnchantedLand.Common.Race;
using EnchantedLand.Common.Scripts;
using EnchantedLand.Common.Scripts.Potions;
using SO.Scripts;
using UnityEngine;

namespace Tanks.Common.Scripts.Player
{
    public class PlayerManager
    {
        private const string FileName = "player_data.json";
       
        private PlayerData _data;
        private string SavePath => Path.Combine(Application.persistentDataPath, FileName);
        public PlayerData Data => _data;
        public int CurrentMapId { get;  set; }
        public int CurrentPlayerCount { get;  set; }


        private Characteristics _currentCharacteristics;

        public Action<int, IIdentifiable, bool> OnStorageChanged;
        public Action<int, IIdentifiable, bool> OnEquipedChanged;

        public PlayerManager()
        {
            Load();
        }

        // =========================================================
        // LOAD
        // =========================================================

        public void Load()
        {
            if (!File.Exists(SavePath))
            {
                CreateDefaultData();
                Save();
                return;
            }

            try
            {
                var json = File.ReadAllText(SavePath);
                _data = JsonUtility.FromJson<PlayerData>(json);
                if (_data == null)
                {
                    CreateDefaultData();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"<color=#FF0000>Failed to load player data: {e}</color>");
                CreateDefaultData();
            }
            Save();
        }

        // =========================================================
        // SAVE
        // =========================================================

        public void Save()
        {
            try
            {
                var json = JsonUtility.ToJson(_data, true);
                File.WriteAllText(SavePath, json);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"<color=#FF0000>Failed to save player data: {e}</color>");
            }
        }

        // =========================================================
        // DEFAULT DATA
        // =========================================================

        private void CreateDefaultData()
        {
            _data = new PlayerData
            {
                BaseCharacteristics = new Characteristics(),
                RaсeId = 0,
                Currency = PlayerData.START_PLAYER_MONEY
            };
        }
        
        public void SaveCharacteristics(Race raсe, Characteristics characteristics)
        {
            _data.SaveCharacteristics(raсe, characteristics);
            _currentCharacteristics = new Characteristics
            {
                Agility = characteristics.Agility,
                Strength = characteristics.Strength,
                Endurance = characteristics.Endurance,
                Charisma = characteristics.Charisma,
                Intelligence = characteristics.Intelligence
            };
            SetCharacteristics(raсe);
        }

        public void SetCharacteristics(Race raсe)
        {
            _currentCharacteristics = new Characteristics
            {
                Agility = raсe.Characteristics.Agility,
                Strength = _data.BaseCharacteristics.Strength + raсe.Characteristics.Strength,
                Endurance = _data.BaseCharacteristics.Endurance + raсe.Characteristics.Endurance,
                Charisma = _data.BaseCharacteristics.Charisma + raсe.Characteristics.Charisma,
                Intelligence = _data.BaseCharacteristics.Intelligence + raсe.Characteristics.Intelligence
            };
            _data.LiftingCapacity = Mathf.RoundToInt( (_currentCharacteristics.Endurance + _currentCharacteristics.Strength) * PlayerData.WEIGHT_COEFFICIENT);
            _data.MaxHP = _data.HP =  (int)(_currentCharacteristics.Endurance +(_currentCharacteristics.Endurance * PlayerData.HP_COEFFICIENT));
            Save();
        }

        public int CalculatePrice(int price)
        {
            var multiplier = 1f - (_currentCharacteristics.Charisma - 9) * PlayerData.CHARISMA_PERCENTAGE; 
            return Mathf.CeilToInt(price * multiplier);
        }
        
        public bool AddToStorage(IIdentifiable item, bool isSaved = true, bool isEquiped = false)
        {
           var result =  _data.AddToStorage(item, isSaved, isEquiped);
           Save();
           return result;
        }

        public bool RemoveFromStorage(int index, bool isEquiped = false)
        {
            var result =  _data.RemoveFromStorage(index, isEquiped);
            Save();
            return result;
        }

        public bool Equip(IIdentifiable item, bool isSaved = false, bool fromStorage = true)
        {
            if (item == null) return false;

            if (item.Type == ItemType.Potions) return item is Potions potion && UsePotions(potion);
            
            if (item.Type == ItemType.Other)
            {
                //todo дописать если используем предмет из озер
                return false;
            }
            var result =  _data.Equip(item, isSaved, fromStorage);
            Save();
            return result;
        }
        
        public bool DeEquip(int index, bool isRemoved = false)
        {
            return _data.DeEquip(index, isRemoved);
        }

        private bool UsePotions(Potions potion)
        {
            switch (potion.SubType)
            {
                case PotionsType.HP:
                    if (_data.HP >= _data.MaxHP) return false;
                    _data.HP = Mathf.Min(_data.HP + potion.Impact, _data.MaxHP);
                    Save();
                    return true;

                case PotionsType.Stealth:
                    // TODO: Применить эффект скрытности
                    return false;

                case PotionsType.Initiative:
                    // TODO: Применить эффект инициативы
                    return false;

                default:
                    Debug.LogWarning($"Unknown potion type: {potion.SubType}");
                    return false;
            }
        }
    }
}