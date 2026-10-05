using System.Collections.Generic;
using Common.Scripts.GoogleSheets;
using Cysharp.Threading.Tasks;
using EnchantedLand.Common.DataModels;
using EnchantedLand.Common.Scripts.Equipment;
using UnityEngine;

namespace EnchantedLand.Common.Armor
{
    public class ArmorManager : EquipmentManager
    {
        private List<Armor> _weapon = new();
        public IReadOnlyList<Armor> Weapon => _weapon;
        public int Count => _weapon.Count;
        
        protected override async UniTask Load()
        {
            List<ArmorDTO> data = await _googleSheetsService.LoadSheet<ArmorDTO>(GoogleSheetsURL.Armor);

            if (data == null || data.Count == 0)
            {
                Debug.LogError("Armor data is empty.");
                return;
            }
            _weapon.Clear();
            foreach (ArmorDTO dto in data)
            {
                _weapon.Add(Create(dto));
            }
            Debug.Log($"Armor loaded: {_weapon.Count}");
        }
        
         private Armor Create(ArmorDTO dto)
        {
            return new Armor
            {
                ID = dto.ID,
                Image = _avatarImageService.GetArmor(dto.Image_ID).ImageInterface,
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                SlotId = (EquipmentSlotID)dto.Slot_ID,
                InShop = dto.In_Shop > 0,
                CurrentStrength = dto.Current_Strength,
                MaxStrength = dto.Max_Strength,
                Weight = dto.Weight,

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
                Resistance =  new Resistance
                {
                    Physical =  dto.Resistics_Physical,
                    Acid = dto.Resistics_Acid,
                    Fire = dto.Resistics_Fire,
                    Ice = dto.Resistics_Ice,
                    Electricity = dto.Resistics_Electricity,
                    Paralysis = dto.Resistics_Paralysis,
                    Death = dto.Resistics_Death,
                }
            };
        }

        public Armor GetById(int id)
        {
            foreach (Armor item in _weapon)
            {
                if (item.ID == id)
                    return item;
            }
            Debug.LogError($"Armor with ID {id} not found.");
            return null;
        }
    }
}