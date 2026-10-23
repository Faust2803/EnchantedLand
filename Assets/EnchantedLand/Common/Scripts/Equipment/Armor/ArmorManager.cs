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
            
            var neededCharacteristics = new Dictionary<CharacteristicsID, int>();
            neededCharacteristics[CharacteristicsID.Strength] = dto.Need_Strength;
            neededCharacteristics[CharacteristicsID.Intelligence] = dto.Need_Intelligence;
            neededCharacteristics[CharacteristicsID.Agility] = dto.Need_Agility;
            neededCharacteristics[CharacteristicsID.Charisma] = dto.Need_Charisma;
            neededCharacteristics[CharacteristicsID.Endurance] = dto.Need_Endurance;
            
            var influenceCharacteristics = new Dictionary<CharacteristicsID, int>();
            influenceCharacteristics[CharacteristicsID.Strength] = dto.Influence_Strength;
            influenceCharacteristics[CharacteristicsID.Intelligence] = dto.IInfluence_Intelligence;
            influenceCharacteristics[CharacteristicsID.Agility] = dto.Influence_Agility;
            influenceCharacteristics[CharacteristicsID.Charisma] = dto.Influence_Charisma;
            influenceCharacteristics[CharacteristicsID.Endurance] = dto.Influence_Endurance;
            
            var skills = new Dictionary<SkillsID, int>();
            skills[SkillsID.Accuracy] = dto.Accuracy;
            skills[SkillsID.Armor] = dto.Armor;
            skills[SkillsID.Destruction] = dto.Desttuction;
            skills[SkillsID.Initiative] = dto.Initiative;
            skills[SkillsID.Stealth] = dto.Stealth;
            
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
            
                NeededCharacteristics = neededCharacteristics,
                InfluenceCharacteristics = influenceCharacteristics,
                Skills = skills,
                Resistance = resistance,
                Count = dto.Count
            };
        }
    }
}