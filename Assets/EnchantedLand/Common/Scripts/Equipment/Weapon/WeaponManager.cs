
using EnchantedLand.Common.Armor;
using EnchantedLand.Common.DataModels;

namespace EnchantedLand.Common.Weapon
{
    public class WeaponManager : EquipmentManager<Weapon, WeaponDTO>
    {
        protected override string SheetUrl => "https://docs.google.com/spreadsheets/d/e/2PACX-1vRgOkzycG2abPB1w6LScMWBTGLVAx62vSXtimAjeuA2t2Qp5KyRUScbnuoGO8m9H3hl0J2YRJEn3BTR/pub?gid=2002456010&single=true&output=csv";
        
        protected override Weapon Create(WeaponDTO dto)
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
    }
}