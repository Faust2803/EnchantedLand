using System.Collections.Generic;
using Common.Scripts.GoogleSheets;
using Cysharp.Threading.Tasks;
using EnchantedLand.Common.Armor;
using EnchantedLand.Common.DataModels;
using EnchantedLand.Common.Scripts.Equipment;
using UnityEngine;

namespace EnchantedLand.Common.Weapon
{
    public class WeaponManager : EquipmentManager
    {
        private List<Common.Weapon.Weapon> _weapon = new();
        public IReadOnlyList<Common.Weapon.Weapon> Weapon => _weapon;
        public int Count => _weapon.Count;
        
        protected override async UniTask Load()
        {
            List<WeaponDTO> data = await _googleSheetsService.LoadSheet<WeaponDTO>(GoogleSheetsURL.Weapon);

            if (data == null || data.Count == 0)
            {
                Debug.LogError("Weapon data is empty.");
                return;
            }
            _weapon.Clear();
            foreach (WeaponDTO dto in data)
            {
                _weapon.Add(Create(dto));
            }
            Debug.Log($"Weapon loaded: {_weapon.Count}");
        }
        
        private Weapon Create(WeaponDTO dto)
        {
            return new Weapon
            {
                ID = dto.ID,
                Image = _avatarImageService.GeWeapon(dto.Image_ID).ImageInterface,
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                Projectile = dto.Projectile > 0,
                SlotId = (EquipmentSlotID)dto.Slot_ID,
                InShop = dto.In_Shop > 0,
                TwoHands = dto.Two_Hands > 0,
                CurrentStrength = dto.Current_Strength,
                MaxStrength = dto.Max_Strength,
                Weight = dto.Weight,
                Distance = dto.Distance,
                
                NeededCharacteristics = new Characteristics
                {
                    Strength = dto.Need_Strength,
                    Intelligence = dto.Need_Intelligence,
                    Agility = dto.Need_Agility,
                    Charisma = dto.Need_Charisma,
                    Endurance = dto.Need_Endurance
                },
                InfluenceCharacteristics = new Characteristics
                {
                    Strength = dto.Influence_Strength,
                    Intelligence = dto.IInfluence_Intelligence,
                    Agility = dto.Influence_Agility,
                    Charisma = dto.Influence_Charisma,
                    Endurance = dto.Influence_Endurance
                },
                Skills = new Skills
                {
                    Armor = dto.Armor,
                    Accuracy = dto.Accuracy,
                    Destruction = dto.Desttuction,
                    Initiative = dto.Initiative,
                    Stealth = dto.Stealth
                },
                Damage = new [] 
                {
                    _damageService.GetById(dto.Damage_ID_0),
                    _damageService.GetById(dto.Damage_ID_1),
                    _damageService.GetById(dto.Damage_ID_2)
                }
            };
        }

        public Weapon GetById(int id)
        {
            foreach (Weapon item in _weapon)
            {
                if (item.ID == id)
                    return item;
            }
            Debug.LogError($"weapon with ID {id} not found.");
            return null;
        }
    }
}