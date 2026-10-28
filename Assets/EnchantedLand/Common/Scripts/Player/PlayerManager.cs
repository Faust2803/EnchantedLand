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
                else
                {
                    Debug.Log($"<color=#00FF00>Player Data Loaded</color>");
                    _data.IntiData();
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

        public void CreateDefaultData()
        {
            _data = new PlayerData
            {
                BaseCharacteristics = new Characteristics(),
                RaсeId = 0,
                Currency = PlayerData.START_PLAYER_MONEY
            };
            _data.IntiData();
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
            ApplyModifiers(_data.СurrentCharacteristics, characteristics, add);
            RecalculateDerivedStats();
            if (save) Save();
        }

        private void RecalculateDerivedStats()
        {
            _data.СurrentCharacteristics.TryGetValue(CharacteristicsID.Endurance, out int endurance);
            _data.СurrentCharacteristics.TryGetValue(CharacteristicsID.Strength, out int strength);
            _data.СurrentCharacteristics.TryGetValue(CharacteristicsID.Agility, out int agility);

            _data.LiftingCapacity = Mathf.RoundToInt((endurance + strength) * PlayerData.WEIGHT_COEFFICIENT);
            int previousMaxHP = _data.MaxHP;
            _data.MaxHP = Mathf.RoundToInt(endurance * (1f + PlayerData.HP_COEFFICIENT));

            // При первой инициализации устанавливаем полное здоровье.
            if (previousMaxHP <= 0 || _data.HP <= 0)
            {
                _data.HP = _data.MaxHP;
            }
            else
            {
                _data.HP = Mathf.Min(_data.HP, _data.MaxHP);
            }
            
            _data.AddSkills[SkillsID.Armor] = GetSkillModifier(agility);
            _data.AddSkills[SkillsID.Accuracy] = GetSkillModifier(agility);
            _data.AddSkills[SkillsID.Destruction] = GetSkillModifier(strength);

            _data.AddResistics = GetResistanceModifier(endurance);
        }

        private int GetSkillModifier(int characteristic)
        {
            return characteristic switch
            {
                0 => -3,
                1 or 3 or 4 => -2,
                5 or 6 or 7 => -1,
                >= 8 and <= 11 => 0,
                12 or 13 or 14 => 1,
                15 or 16 or 17 => 2,
                18 => 3,
                _ => 0
            };
        }

        private float GetResistanceModifier(int endurance)
        {
            return endurance switch
            {
                0 => -0.8f,
                1 => -0.7f,
                3 => -0.6f,
                4 => -0.5f,
                5 => -0.4f,
                6 => -0.3f,
                7 => -0.2f,
                8 => -0.1f,
                9 => 0f,
                10 => 0.1f,
                11 => 0.2f,
                12 => 0.3f,
                13 => 0.4f,
                14 => 0.5f,
                15 => 0.6f,
                16 => 0.7f,
                17 => 0.8f,
                18 => 0.9f,
                _ => 0f
            };
        }

        public void SetSkills(Dictionary<SkillsID, int> skills, bool isEquip)
        {
            if (skills == null) return;
            
            ApplyModifiers(_data.Skills, skills, isEquip);
        }

        public void SetResistics(Dictionary<DamageType, float> resistics, bool isEquip)
        {
            if (resistics == null) return;
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

        public bool Equip(IIdentifiable item, out string error,bool isSaved = false, bool fromStorage = true)
        {
            error = String.Empty;
            if (item == null) return false;
           
            switch (item.Type)
            {
                case ItemType.Potions:
                    return item is Potions potion && UsePotions(potion);

                case ItemType.Other:
                    // TODO: Реализовать использование предметов из слотов.
                    return false;

                case ItemType.Armor:
                    if ((item as Armor).SlotId == EquipmentSlotID.Shield)
                    {
                        for (var i = 0; i < _data.Equiped.Count; i++)
                        {
                            if (_data.Equiped[i] != null && _data.Equiped[i].SlotId == EquipmentSlotID.Weapon && (_data.Equiped[i] as Weapon).TwoHands)
                            {
                                error = $"Вы пытаетесь экипировать {item.Title}. Освободите вторую руку в ней находится {_data.Equiped[i].Title} - двуручное оружие.";
                                return false;
                            }
                        }
                    }
                    break;
                case ItemType.Weapon:
                    if ((item as Weapon).TwoHands)
                    {
                        for (var i = 0; i < _data.Equiped.Count; i++)
                        {
                            if (_data.Equiped[i] != null && _data.Equiped[i].SlotId == EquipmentSlotID.Shield)
                            {
                                error = $"Вы пытаетесь экипировать {item.Title} - двуручное оружие. Освободите вторую руку, в ней находится {_data.Equiped[i].Title }.";
                                return false;
                            }
                        }
                    }
                    break;
                default:
                    return false;
            }

            if (CheckCharacteristics(item, out error))
            {
                return false;
            }

            var result = _data.Equip(item, isSaved, fromStorage);
            if (result)
            {
                SetData(item , true);
            }
            else
            {
                error = "Нет свободного слота для этого предмета";
            }

            return result;
        }
        
        public bool DeEquip(int i, IIdentifiable item, bool isRemoved = false)
        {
            var result = _data.DeEquip(i, item, isRemoved);
            if (result)
            {
                SetData(item , false);
                Save();
            }
            return result;
        }
        
        public bool CheckCharacteristics(IIdentifiable item, out string error)
        {
            error = String.Empty;
            
            var characteristics = item switch
            {
                Armor armor => armor.NeededCharacteristics,
                Weapon weapon => weapon.NeededCharacteristics,
                _ => null
            };
            
            
            if (characteristics == null) return false;
            foreach (var characteristic in characteristics)
            {
                if (!_data.СurrentCharacteristics.TryGetValue(characteristic.Key, out int current))
                {
                    error = "Ошибка характеристики";
                    return false;
                }

                if (characteristic.Value > current)
                {
                    switch (characteristic.Key)
                    {
                        case CharacteristicsID.Strength:
                            error = $"Недостаточно характеристики 'Сила' {current}/{characteristic.Value}";
                            break;
                        case CharacteristicsID.Intelligence:
                            error = $"Недостаточно характеристики 'Интеллект' {current}/{characteristic.Value}";
                            break;
                        case CharacteristicsID.Agility:
                            error = $"Недостаточно характеристики 'Ловкость' {current}/{characteristic.Value}";
                            break;
                        case CharacteristicsID.Charisma:
                            error = $"Недостаточно характеристики 'Харизма' {current}/{characteristic.Value}";
                            break;
                        case CharacteristicsID.Endurance:
                            error = $"Недостаточно характеристики 'Выносливость' {current}/{characteristic.Value}";
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    
                    
                    return false;
                }
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