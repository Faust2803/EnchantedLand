
using EnchantedLand.Common.Scripts;

namespace Tanks.Game.Scripts.Entetyes.Loot
{
    public class LootManager: ConfiguratedManager <Loot, LootDTO>
    {
        protected override string SheetUrl => "https://docs.google.com/spreadsheets/d/e/2PACX-1vRgOkzycG2abPB1w6LScMWBTGLVAx62vSXtimAjeuA2t2Qp5KyRUScbnuoGO8m9H3hl0J2YRJEn3BTR/pub?gid=1067077135&single=true&output=csv";
        
        protected override Loot Create(LootDTO dto)
        {
            return new Loot
            {
                ID = dto.ID,
                Type = (ItemType)dto.Type,
                Count = dto.Count,
                ElementID = dto.Element_ID,
                Prosentage = dto.Prosentage,
            };
        }
    }
}