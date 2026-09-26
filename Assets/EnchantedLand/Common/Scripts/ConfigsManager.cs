using SO.Scripts;

namespace Tanks.Common.Scripts
{
    public class ConfigsManager
    {
        private readonly AvatarConfig _avatarConfig;
        private readonly MonsterConfig _monsterConfig;
        private readonly СastleConfig _сastleConfig;
        private readonly TripConfig _tripConfig;
        private readonly QuestConfig _questConfig;
        
        public AvatarConfig AvatarConfig => _avatarConfig;
        public MonsterConfig MonsterConfig  => _monsterConfig;
        public TripConfig TripConfig => _tripConfig;
        public QuestConfig QuestConfig => _questConfig;
        public СastleConfig CastleConfig => _сastleConfig;
        
        

        public ConfigsManager(
            AvatarConfig avatarConfig,
            MonsterConfig monsterConfig,
            СastleConfig сastleConfig,
            TripConfig tripConfig,
            QuestConfig questConfig
            )
        {
            _avatarConfig = avatarConfig;
            _monsterConfig = monsterConfig;
            _сastleConfig = сastleConfig;
            _tripConfig = tripConfig;
            _questConfig = questConfig;
            
        }
        
        
    }
}