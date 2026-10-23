using System;
using System.Collections.Generic;
using System.IO;
using EnchantedLand.Common.Armor;
using EnchantedLand.Common.DataModels;
using EnchantedLand.Common.Race;
using EnchantedLand.Common.Scripts;
using EnchantedLand.Common.Scripts.Potions;
using EnchantedLand.Common.Weapon;
using SO.Scripts;
using UnityEngine;

namespace Common.Scripts.Player
{
    public class PlayerManager
    {
        private const string FileName = "player_data.json";
       
        private PlayerData _data;
        private string SavePath => Path.Combine(Application.persistentDataPath, FileName);
        public PlayerData Data => _data;
        public int CurrentMapId { get;  set; }
        public int CurrentPlayerCount { get;  set; }



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
                Debug.Log($"<color=#00FF00>Player Data Loaded</color>");
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

        public void CreateDefaultData()
        {
            _data = new PlayerData
            {
                BaseCharacteristics = new Characteristics(),
                RaсeId = 0,
                Currency = PlayerData.START_PLAYER_MONEY
            };
        }
        
        public void SaveCharacteristicsByRace(Race raсe, Dictionary<CharacteristicsID, int> characteristics)
        {
            if (raсe == null || characteristics == null) return;
            _data.SaveRaceAndBaseCharacteristics(raсe, characteristics);
            SetCharacteristics(raсe.Characteristics, true, false);
            Save();
        }

        public void SetCharacteristics(Dictionary<CharacteristicsID, int> characteristics, bool add, bool save = true)
        {
            if (characteristics == null) return;
            if (_data.СurrentCharacteristics.Count == 0)
            {
                _data.СurrentCharacteristics[CharacteristicsID.Intelligence] = _data.BaseCharacteristics.Intelligence;
                _data.СurrentCharacteristics[CharacteristicsID.Endurance] = _data.BaseCharacteristics.Endurance;
                _data.СurrentCharacteristics[CharacteristicsID.Agility] = _data.BaseCharacteristics.Agility;
                _data.СurrentCharacteristics[CharacteristicsID.Charisma] = _data.BaseCharacteristics.Charisma;
                _data.СurrentCharacteristics[CharacteristicsID.Strength] = _data.BaseCharacteristics.Strength;
            }
            ApplyModifiers(_data.СurrentCharacteristics, characteristics, add);
            RecalculateDerivedStats();
            if (save) Save();
        }

        private void RecalculateDerivedStats()
        {
            _data.СurrentCharacteristics.TryGetValue(CharacteristicsID.Endurance, out int endurance);
            _data.СurrentCharacteristics.TryGetValue(CharacteristicsID.Strength, out int strength);
            _data.LiftingCapacity = Mathf.RoundToInt((endurance + strength) * PlayerData.WEIGHT_COEFFICIENT);
            int previousMaxHP = _data.MaxHP;
            _data.MaxHP = Mathf.RoundToInt(endurance * (1f + PlayerData.HP_COEFFICIENT));
            // При первой инициализации устанавливаем полное здоровье.
            if (previousMaxHP <= 0 || _data.HP <= 0)
                _data.HP = _data.MaxHP;
            else
                _data.HP = Mathf.Min(_data.HP, _data.MaxHP);
        }

        public void SetSkills(Dictionary<SkillsID, int> skills, bool isEquip)
        {
            if (skills == null) return;
            if (_data.Skills.Count == 0)
            {
                _data.Skills.Add(SkillsID.Accuracy, 0);
                _data.Skills.Add(SkillsID.Armor, 0);
                _data.Skills.Add(SkillsID.Initiative, 0);
                _data.Skills.Add(SkillsID.Destruction, 0);
                _data.Skills.Add(SkillsID.Stealth, 0);
            }
            ApplyModifiers(_data.Skills, skills, isEquip);
        }

        public void SetResistics(Dictionary<DamageType, float> resistics, bool isEquip)
        {
            if (resistics == null) return;
            if (_data.Resistics.Count == 0)
            {
                _data.Resistics.Add(DamageType.Acid, 0);
                _data.Resistics.Add(DamageType.Death, 0);
                _data.Resistics.Add(DamageType.Electricity, 0);
                _data.Resistics.Add(DamageType.Fire, 0);
                _data.Resistics.Add(DamageType.Ice, 0);
                _data.Resistics.Add(DamageType.Physical, 0);
                _data.Resistics.Add(DamageType.Оther, 0);
            }
            ApplyModifiers(_data.Resistics, resistics, isEquip);
        }

        private void ApplyModifiers<TKey>(Dictionary<TKey, int> target, Dictionary<TKey, int> modifiers, bool add)
        {
            foreach (var modifier in modifiers)
            {
                target.TryGetValue(modifier.Key, out int current);
                target[modifier.Key] = add ? current + modifier.Value : current - modifier.Value;
            }
        }
        
        private void ApplyModifiers<TKey>(Dictionary<TKey, float> target, Dictionary<TKey, float> modifiers, bool add)
        {
            foreach (var modifier in modifiers)
            {
                target.TryGetValue(modifier.Key, out float current);
                target[modifier.Key] = add ? current + modifier.Value : current - modifier.Value;
            }
        }

        public int CalculatePrice(int price)
        {
            _data.СurrentCharacteristics.TryGetValue(CharacteristicsID.Charisma, out int charisma);
            float multiplier = 1f - (charisma - 9) * PlayerData.CHARISMA_PERCENTAGE;
            return Mathf.CeilToInt(price * multiplier);
        }
        
        public bool AddToStorage(IIdentifiable item, bool isSaved = true, bool isEquiped = false)
        {
           var result =  _data.AddToStorage(item, isSaved, isEquiped);
           if (result)Save();
           return result;
        }

        public bool RemoveFromStorage(int index, bool isEquiped = false)
        {
            var result =  _data.RemoveFromStorage(index, isEquiped);
            if (result)Save();
            return result;
        }

        public bool Equip(IIdentifiable item, bool isSaved = false, bool fromStorage = true)
        {
            if (item == null) return false;
           
            switch (item.Type)
            {
                case ItemType.Potions:
                    return item is Potions potion && UsePotions(potion);

                case ItemType.Other:
                    // TODO: Реализовать использование предметов из слотов.
                    return false;

                case ItemType.Armor:
                case ItemType.Weapon:
                    break;
                default:
                    return false;
            }

            var characteristics = item switch
            {
                Armor armor => armor.NeededCharacteristics,
                Weapon weapon => weapon.NeededCharacteristics,
                _ => null
            };

            var checkResult = CheckCharacteristics(characteristics);

            if (characteristics == null || !checkResult) return false;

            var result = _data.Equip(item, isSaved, fromStorage);
            if (result)
            {
                SetData(item , true);
            }

            return result;
        }
        
        public bool DeEquip(int i, IIdentifiable item, bool isRemoved = false)
        {
            var result = _data.DeEquip(i, item, isRemoved);
            if (result)
            {
                SetData(item , false);
            }
            return result;
        }
        
        public bool CheckCharacteristics(Dictionary<CharacteristicsID, int> characteristics)
        {
            if (characteristics == null) return false;
            foreach (var characteristic in characteristics)
            {
                if (!_data.СurrentCharacteristics.TryGetValue(characteristic.Key, out int current))
                {
                    return false;
                }

                if (characteristic.Value > current) return false;
            }

            return true;
        }

        private void SetData(IIdentifiable item, bool isEquip)
        {
            switch (item)
            {
                case Armor armor:
                    SetCharacteristics(armor.InfluenceCharacteristics, isEquip, false);
                    SetSkills(armor.Skills, isEquip);
                    SetResistics(armor.Resistance, isEquip);
                    break;

                case Weapon weapon:
                    SetCharacteristics(weapon.InfluenceCharacteristics, isEquip, false);
                    SetSkills(weapon.Skills, isEquip);
                    break;
            }
            Save();
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