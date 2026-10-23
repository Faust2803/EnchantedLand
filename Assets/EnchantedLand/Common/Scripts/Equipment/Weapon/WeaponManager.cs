
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EnchantedLand.Common.Armor;
using EnchantedLand.Common.DataModels;
using EnchantedLand.Common.Scripts;

namespace EnchantedLand.Common.Weapon
{
    public class WeaponManager : EquipmentManager<Weapon, WeaponDTO>
    {
        protected override string SheetUrl => "https://docs.google.com/spreadsheets/d/e/2PACX-1vRgOkzycG2abPB1w6LScMWBTGLVAx62vSXtimAjeuA2t2Qp5KyRUScbnuoGO8m9H3hl0J2YRJEn3BTR/pub?gid=2002456010&single=true&output=csv";

        public WeaponManager()
        {
            Type = ItemType.Weapon;
        }
        
        protected override Weapon Create(WeaponDTO dto)
        {
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
            
            var weapon =  new Weapon
            {
                ID = dto.ID,
                PreviewImage = _avatarImageService.GeWeapon(dto.Image_ID).PreviewImage,
                EquipedImage = _avatarImageService.GeWeapon(dto.Image_ID).EquipedImage,
                StorageImage = _avatarImageService.GeWeapon(dto.Image_ID).StorageImage,
                Type = ItemType.Weapon,
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
                
                NeededCharacteristics = neededCharacteristics,
                InfluenceCharacteristics = influenceCharacteristics,
                Skills = skills,

                Damage = new List<Damage.Damage>(),
                Count = dto.Count
            };
            
            if (dto.Damage_ID_0 >= 0)
            {
                weapon.Damage.Add(DamageManager.GetById(dto.Damage_ID_0));
            }
            if (dto.Damage_ID_1 >= 0)
            {
                weapon.Damage.Add(DamageManager.GetById(dto.Damage_ID_1));
            }
            if (dto.Damage_ID_2 >= 0)
            {
                weapon.Damage.Add(DamageManager.GetById(dto.Damage_ID_2));
            }
            return weapon;
        }
    }
}