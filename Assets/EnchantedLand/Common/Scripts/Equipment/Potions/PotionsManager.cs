using EnchantedLand.Common.Armor;

namespace EnchantedLand.Common.Scripts.Potions
{
    public class PotionsManager : EquipmentManager<Potions, PotionsDTO>
    {
        protected override string SheetUrl => "https://docs.google.com/spreadsheets/d/e/2PACX-1vRgOkzycG2abPB1w6LScMWBTGLVAx62vSXtimAjeuA2t2Qp5KyRUScbnuoGO8m9H3hl0J2YRJEn3BTR/pub?gid=502183193&single=true&output=csv";
        
        public PotionsManager()
        {
            Type = ItemType.Potions;
        }
        
        protected override Potions Create(PotionsDTO dto)
        {
            return new Potions
            {
                ID = dto.ID,
                PreviewImage = _avatarImageService.GetPotions(dto.Image_ID).PreviewImage,
                EquipedImage = _avatarImageService.GetPotions(dto.Image_ID).EquipedImage,
                StorageImage = _avatarImageService.GetPotions(dto.Image_ID).StorageImage,
                Type = Type,
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                SlotId = EquipmentSlotID.None,
                InShop = dto.In_Shop > 0,
                CurrentStrength = 0,
                MaxStrength = 0,
                Weight = 1,
                Count = dto.Count,
                SubType = (PotionsType)dto.Type,
                Impact = dto.Impact,
            };
        }
    }
}