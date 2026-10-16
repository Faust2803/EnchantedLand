using System.Collections.Generic;
using Common.Scripts.GoogleSheets;
using Cysharp.Threading.Tasks;
using EnchantedLand.Common.DataModels;
using SO.Scripts;
using UnityEngine;
using Zenject;

namespace EnchantedLand.Common.Race
{
    public class RaceManager
    {
        private const string GoogleSheetsURL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRgOkzycG2abPB1w6LScMWBTGLVAx62vSXtimAjeuA2t2Qp5KyRUScbnuoGO8m9H3hl0J2YRJEn3BTR/pub?gid=1215578610&single=true&output=csv";
        private GoogleSheetsService _googleSheetsService;
        private AvatarImageService _avatarImageService;
        private List<Race> _races = new();
        public IReadOnlyList<Race> Races => _races;
        public int Count => _races.Count;

        [Inject]
        private void Construct(GoogleSheetsService googleSheetsService, AvatarImageService avatarImageService)
        {
            _googleSheetsService = googleSheetsService;
            _avatarImageService = avatarImageService;

            Load().Forget();
        }

        private async UniTask Load()
        {
            List<RaceDTO> data = await _googleSheetsService.LoadSheet<RaceDTO>(GoogleSheetsURL);

            if (data == null || data.Count == 0)
            {
                Debug.LogError("Race data is empty.");
                return;
            }
            _races.Clear();
            foreach (RaceDTO dto in data)
            {
                _races.Add(CreateRace(dto));
            }
            Debug.Log($"Races loaded: {_races.Count}");
        }

        private Race CreateRace(RaceDTO dto)
        {
            return new Race
            {
                Id = dto.Id,
                Image = _avatarImageService.GeAvatar(dto.ImageID).Image,
                Title = dto.Title,
                Description = dto.Description,

                Characteristics = new Characteristics
                {
                    Strength = dto.Strength,
                    Intelligence = dto.Intelligence,
                    Agility = dto.Agility,
                    Charisma = dto.Charisma,
                    Endurance = dto.Endurance
                }
            };
        }

        public Race GetRaceById(int id)
        {
            foreach (Race race in _races)
            {
                if (race.Id == id)
                    return race;
            }
            Debug.LogError($"Race with ID {id} not found.");
            return null;
        }
    }
}