using EnchantedLand.Common.DataModels;


namespace EnchantedLand.Common.Armor
{
    public class ArmorManager : EquipmentManager<Armor, ArmorDTO>
    {
        protected override string SheetUrl => "https://docs.google.com/spreadsheets/d/e/2PACX-1vRgOkzycG2abPB1w6LScMWBTGLVAx62vSXtimAjeuA2t2Qp5KyRUScbnuoGO8m9H3hl0J2YRJEn3BTR/pub?gid=1575724902&single=true&output=csv";
        
        protected override Armor Create(ArmorDTO dto)
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
    }
}