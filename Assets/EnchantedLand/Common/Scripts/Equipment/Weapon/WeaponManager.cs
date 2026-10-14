
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

                Damage = new List<Damage.Damage>(),
                Count = dto.Count
            };
            
            if (dto.Damage_ID_0 >= 0)
            {
                weapon.Damage.Add(_damageService.GetById(dto.Damage_ID_0));
            }
            if (dto.Damage_ID_1 >= 0)
            {
                weapon.Damage.Add(_damageService.GetById(dto.Damage_ID_1));
            }
            if (dto.Damage_ID_2 >= 0)
            {
                weapon.Damage.Add(_damageService.GetById(dto.Damage_ID_2));
            }
            return weapon;
        }
    }
}