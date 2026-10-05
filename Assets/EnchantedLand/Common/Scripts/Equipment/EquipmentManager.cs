using System.Collections.Generic;
using Common.Scripts.GoogleSheets;
using Cysharp.Threading.Tasks;
using EnchantedLand.Common.Damage;
using SO.Scripts;
using Zenject;

namespace EnchantedLand.Common.Scripts.Equipment
{
    public abstract class EquipmentManager
    {
        protected GoogleSheetsService _googleSheetsService;
        protected AvatarImageService _avatarImageService;
        protected DamageService _damageService;

        [Inject]
        private void Construct(GoogleSheetsService googleSheetsService, AvatarImageService avatarImageService, DamageService damageService)
        {
            _googleSheetsService = googleSheetsService;
            _avatarImageService = avatarImageService;
            _damageService = damageService;

            if (_damageService.Count > 0 )
            {
                Load().Forget();
            }
            else
            {
                _damageService.LoadCompleat += Load;
            }
        }

        private void Load(bool result)
        {
            Load().Forget();
        }

        protected abstract  UniTask Load();
    }
}