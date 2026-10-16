using EnchantedLand.Common.Armor;

namespace EnchantedLand.Common.Scripts.Others
{
    public class OthersEquipmentManager: EquipmentManager<OthersEquipment, OthersEquipmentDTO>
    {
        protected override string SheetUrl => "https://docs.google.com/spreadsheets/d/e/2PACX-1vRgOkzycG2abPB1w6LScMWBTGLVAx62vSXtimAjeuA2t2Qp5KyRUScbnuoGO8m9H3hl0J2YRJEn3BTR/pub?gid=855152700&single=true&output=csv";
        
        public OthersEquipmentManager()
        {
            Type = ItemType.Other;
        }
        
        protected override OthersEquipment Create(OthersEquipmentDTO dto)
        {
            return new OthersEquipment
            {
                ID = dto.ID,
                PreviewImage = _avatarImageService.GetOtherEquipment(dto.Image_ID).PreviewImage,
                EquipedImage = _avatarImageService.GetOtherEquipment(dto.Image_ID).EquipedImage,
                StorageImage = _avatarImageService.GetOtherEquipment(dto.Image_ID).StorageImage,
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
                SubType = (OthersEquipmentType)dto.Type,
                Impact = dto.Impact,
            };
        }
    }
}