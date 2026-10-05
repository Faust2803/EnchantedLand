using System;
using System.Collections.Generic;
using Common.Scripts.GoogleSheets;
using Cysharp.Threading.Tasks;
using EnchantedLand.Common.Race;
using SO.Scripts;
using UnityEngine;
using Zenject;

namespace EnchantedLand.Common.Damage
{
    public class DamageService
    {
        private GoogleSheetsService _googleSheetsService;
        private AvatarImageService _avatarImageService;
        private List<DamageDTO> _damageDTO = new();
        public IReadOnlyList<DamageDTO> DamageDTO => _damageDTO;
        private string SheetUrl => "https://docs.google.com/spreadsheets/d/e/2PACX-1vRgOkzycG2abPB1w6LScMWBTGLVAx62vSXtimAjeuA2t2Qp5KyRUScbnuoGO8m9H3hl0J2YRJEn3BTR/pub?gid=869588206&single=true&output=csv";
        
        public int Count => _damageDTO.Count;

        public Action<bool> LoadCompleat;

        [Inject]
        private void Construct(GoogleSheetsService googleSheetsService, AvatarImageService avatarImageService)
        {
            _googleSheetsService = googleSheetsService;
            _avatarImageService = avatarImageService;

            Load().Forget();
        }
        
        private async UniTask Load()
        {
            List<DamageDTO> data = await _googleSheetsService.LoadSheet<DamageDTO>(SheetUrl);

            if (data == null || data.Count == 0)
            {
                Debug.LogError("Damage data is empty.");
                LoadCompleat?.Invoke(false);
                return;
            }
            _damageDTO.Clear();
            foreach (DamageDTO dto in data)
            {
                _damageDTO.Add(dto);
               
            }
            LoadCompleat?.Invoke(true);
            Debug.Log($"Damage loaded: {_damageDTO.Count}");
        }
        
        public DamageDTO GetById(int id)
        {
            foreach (DamageDTO damage in _damageDTO)
            {
                if (damage.ID == id)
                    return damage;
            }
            Debug.LogError($"Damage with ID {id} not found.");
            return null;
        }

    }
}