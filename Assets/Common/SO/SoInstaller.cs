using UnityEngine;
using Zenject;

namespace SO.Scripts
{
    [CreateAssetMenu (fileName = "SoInstaller", menuName = "Configs/Create SO Installer", order = 0 )]
    public class SoInstaller : ScriptableObjectInstaller<SoInstaller>
    {
        [SerializeField] public WindowsConfig _windowsConfig;
        [SerializeField] public PanelsConfig _panelConfig;
        
        
        //For game
        [SerializeField] public AvatarConfig _avatarConfig;
        [SerializeField] public MonsterConfig _monsterConfig;
        [SerializeField] public СastleConfig _castleConfig;
        [SerializeField] public TripConfig _tripConfig;
        [SerializeField] public QuestConfig _questConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_windowsConfig).IfNotBound();
            Container.BindInstance(_panelConfig).IfNotBound();
            
            //For game
            Container.BindInstance(_avatarConfig).IfNotBound();
            Container.BindInstance(_monsterConfig).IfNotBound();
            Container.BindInstance(_castleConfig).IfNotBound();
            Container.BindInstance(_questConfig).IfNotBound();
            Container.BindInstance(_tripConfig).IfNotBound();
        }
    }
}