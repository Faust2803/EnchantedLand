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

            LoadRaces().Forget();
        }

        private async UniTask LoadRaces()
        {
            List<RaceDTO> data = await _googleSheetsService.LoadSheet<RaceDTO>(GoogleSheetsURL.Race);

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
                Image = _avatarImageService.GeAvatar(dto.ImageID).ImageInterface,
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

        public bool TryGetRaceById(int id, out Race race)
        {
            foreach (Race item in _races)
            {
                if (item.Id == id)
                {
                    race = item;
                    return true;
                }
            }

            race = null;
            return false;
        }
    }
}