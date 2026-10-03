using System;
using System.Collections.Generic;
using System.IO;
using EnchantedLand.Common.DataModels;
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
        


        
        public static Action<int> OnCurrencyChanged;
        
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
                ValidateData();
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
                RaseId = 0,
                CurrentCharacteristics = new Characteristics(),
                Currency = 200
            };
        }

        // =========================================================
        // VALIDATION
        // =========================================================

        private void ValidateData()
        {
            
        }

        // =========================================================
        // TANK
        // =========================================================

        public bool ChangeCurrency(int value)
        {
            if (value < 0)
            {
                if (Data.Currency + value < 0)
                {
                    return false;
                }
                else
                {
                    Data.Currency += value;
                    Save();
                    OnCurrencyChanged?.Invoke(Data.Currency);
                    return true;
                }
            } 
            else
            {
                Data.Currency += value;
                Save();
                OnCurrencyChanged?.Invoke(Data.Currency);
                return true;
            }
        }
        
    }
}