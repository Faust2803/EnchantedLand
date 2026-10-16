using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Common.Scripts.GoogleSheets;
using EnchantedLand.Common.Damage;
using EnchantedLand.Common.DataModels;
using SO.Scripts;
using Tanks.Game.Scripts;
using Tanks.Game.Scripts.Entetyes.Loot;
using Zenject;

namespace EnchantedLand.Game
{
    public class EnemyesManager : ConfiguratedManager<Enemy, EnemyDTO>
    {
        protected override string SheetUrl => "https://docs.google.com/spreadsheets/d/e/2PACX-1vRgOkzycG2abPB1w6LScMWBTGLVAx62vSXtimAjeuA2t2Qp5KyRUScbnuoGO8m9H3hl0J2YRJEn3BTR/pub?gid=391256&single=true&output=tsv";

        private AvatarImageService _avatarImageService;
        private DamageManager _damageManager;
        private LootManager _lootManager;

        private bool _loadStarted;

        [Inject]
        protected void Construct(GoogleSheetsService googleSheetsService, AvatarImageService avatarImageService, DamageManager damageManager, LootManager lootManager)
        {
            base.Construct(googleSheetsService);

            _avatarImageService = avatarImageService;
            _damageManager = damageManager;
            _lootManager = lootManager;

            if (_damageManager.Count == 0)
                _damageManager.LoadCompleat += OnDependenciesLoadComplete;

            if (_lootManager.Count == 0)
                _lootManager.LoadCompleat += OnDependenciesLoadComplete;

            TryLoad();
        }

        private void OnDependenciesLoadComplete(bool result)
        {
            if (!result)
                return;

            TryLoad();
        }

        private void TryLoad()
        {
            if (_loadStarted)
                return;

            if (_damageManager.Count == 0 || _lootManager.Count == 0)
                return;

            _loadStarted = true;

            _damageManager.LoadCompleat -= OnDependenciesLoadComplete;
            _lootManager.LoadCompleat -= OnDependenciesLoadComplete;

            Load().Forget();
        }

        protected override Enemy Create(EnemyDTO dto)
        {
            var resistance = new Dictionary<DamageType, float>
            {
                { DamageType.Physical, dto.Resistics_Physical },
                { DamageType.Fire, dto.Resistics_Fire },
                { DamageType.Ice, dto.Resistics_Ice },
                { DamageType.Acid, dto.Resistics_Acid },
                { DamageType.Electricity, dto.Resistics_Electricity },
                { DamageType.Paralysis, dto.Resistics_Paralysis },
                { DamageType.Death, dto.Resistics_Death },
                { DamageType.Оther, dto.Resistics_Spesial }
            };

            var damageDistance = new List<int>(3);
            AddIfValid(damageDistance, dto.Attack_Distance_0);
            AddIfValid(damageDistance, dto.Attack_Distance_1);
            AddIfValid(damageDistance, dto.Attack_Distance_2);

            var damage = new List<Damage>(3);
            AddIfValid(damage, dto.Attack_ID_0, _damageManager.GetById);
            AddIfValid(damage, dto.Attack_ID_1, _damageManager.GetById);
            AddIfValid(damage, dto.Attack_ID_2, _damageManager.GetById);

            var loot = new List<Loot>(3);
            AddIfValid(loot, dto.Loot_ID_0, _lootManager.GetById);
            AddIfValid(loot, dto.Loot_ID_1, _lootManager.GetById);
            AddIfValid(loot, dto.Loot_ID_2, _lootManager.GetById);

            return new Enemy
            {
                ID = dto.ID,
                Title = dto.Title,
                Description = dto.Description,
                Type = (EnemyType)dto.Type,
                Armor = dto.Armor,
                HP = dto.HP,
                Level = dto.Level,
                Icon = _avatarImageService.GetEnemy(dto.Icon_ID).Image,
                AttakeBonus = dto.Attake_bonus,
                Initiative = dto.Initiative,
                Resistance = resistance,
                EscapeBonus = dto.Escape_Bonus,
                PoltrooneryBonus = dto.Poltroonery_Bonus,
                DamageDistance = damageDistance,
                Damage = damage,
                Loot = loot
            };
        }

        private void AddIfValid(List<int> list, int value)
        {
            if (value >= 0)
                list.Add(value);
        }

        private void AddIfValid<T>(
            List<T> list,
            int id,
            Func<int, T> getById)
        {
            if (id >= 0)
                list.Add(getById(id));
        }
    }
}
