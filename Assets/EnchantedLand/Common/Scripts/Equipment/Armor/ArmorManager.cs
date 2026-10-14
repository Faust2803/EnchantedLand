using System.Collections.Generic;
using EnchantedLand.Common.DataModels;
using EnchantedLand.Common.Scripts;
using UnityEngine;


namespace EnchantedLand.Common.Armor
{
    public class ArmorManager : EquipmentManager<Armor, ArmorDTO>
    {
        protected override string SheetUrl => "https://docs.google.com/spreadsheets/d/e/2PACX-1vRgOkzycG2abPB1w6LScMWBTGLVAx62vSXtimAjeuA2t2Qp5KyRUScbnuoGO8m9H3hl0J2YRJEn3BTR/pub?gid=1575724902&single=true&output=csv";
        
        public ArmorManager()
        {
            Type = ItemType.Armor;
        }
        
        protected override Armor Create(ArmorDTO dto)
        {
            var resistance = new Dictionary<DamageType, float>();
            resistance.Add(DamageType.Physical, dto.Resistics_Physical / 10);
            resistance.Add(DamageType.Fire, dto.Resistics_Fire / 10);
            resistance.Add(DamageType.Ice, dto.Resistics_Ice / 10);
            resistance.Add(DamageType.Acid, dto.Resistics_Acid / 10);
            resistance.Add(DamageType.Electricity, dto.Resistics_Electricity / 10);
            resistance.Add(DamageType.Paralysis, dto.Resistics_Paralysis / 10);
            resistance.Add(DamageType.Death, dto.Resistics_Death / 10);
            
            return new Armor
            {
                ID = dto.ID,
                PreviewImage = _avatarImageService.GetArmor(dto.Image_ID).PreviewImage,
                EquipedImage = _avatarImageService.GetArmor(dto.Image_ID).EquipedImage,
                StorageImage = _avatarImageService.GetArmor(dto.Image_ID).StorageImage,
                Type = ItemType.Armor,
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
                Resistance = resistance,
                Count = dto.Count
            };
        }
    }
}