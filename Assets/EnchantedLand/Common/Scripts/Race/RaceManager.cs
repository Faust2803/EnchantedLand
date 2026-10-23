using System.Collections.Generic;
using Common.Scripts.GoogleSheets;
using Cysharp.Threading.Tasks;
using EnchantedLand.Common.DataModels;
using SO.Scripts;
using Zenject;

namespace EnchantedLand.Common.Race
{
    public class RaceManager : ConfiguratedManager <Race, RaceDTO>
    {
        protected override string SheetUrl => "https://docs.google.com/spreadsheets/d/e/2PACX-1vRgOkzycG2abPB1w6LScMWBTGLVAx62vSXtimAjeuA2t2Qp5KyRUScbnuoGO8m9H3hl0J2YRJEn3BTR/pub?gid=1215578610&single=true&output=csv";
        private AvatarImageService _avatarImageService;

        [Inject]
        private void Construct(GoogleSheetsService googleSheetsService, AvatarImageService avatarImageService)
        {
            base.Construct(googleSheetsService);
            _avatarImageService = avatarImageService;
            
            Load().Forget();
        }
        
        protected override Race Create(RaceDTO dto)
        {
            var characteristics  = new Dictionary<CharacteristicsID, int>();
            characteristics[CharacteristicsID.Strength] = dto.Strength;
            characteristics[CharacteristicsID.Intelligence] = dto.Intelligence;
            characteristics[CharacteristicsID.Agility] = dto.Agility;
            characteristics[CharacteristicsID.Charisma] = dto.Charisma;
            characteristics[CharacteristicsID.Endurance] = dto.Endurance;

            return new Race
            {
                ID = dto.Id,
                Image = _avatarImageService.GeAvatar(dto.ImageID).Image,
                Title = dto.Title,
                Description = dto.Description,

                Characteristics = characteristics
            };
        }
    }
}