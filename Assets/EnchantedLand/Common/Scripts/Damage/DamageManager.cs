using Common.Scripts.GoogleSheets;
using Cysharp.Threading.Tasks;
using EnchantedLand.Common.DataModels;
using Zenject;

namespace EnchantedLand.Common.Damage
{
    public class DamageManager : ConfiguratedManager <Damage, DamageDTO>
    {
        protected override string SheetUrl => "https://docs.google.com/spreadsheets/d/e/2PACX-1vRgOkzycG2abPB1w6LScMWBTGLVAx62vSXtimAjeuA2t2Qp5KyRUScbnuoGO8m9H3hl0J2YRJEn3BTR/pub?gid=869588206&single=true&output=csv";
        
        [Inject]
        protected override void Construct(GoogleSheetsService googleSheetsService)
        {
            base.Construct(googleSheetsService);
            Load().Forget();
        }
        
        protected override Damage Create(DamageDTO dto)
        {
            return new Damage
            {
                ID = dto.ID,
                Title = dto.Title,
                Type = (DamageType)dto.Type,
                Max = dto.Max,
                Min = dto.Min,
                Opposition = dto.Opposition,
                Radius = dto.Radius,
            };
        }
    }
}