using Common.Scripts.GoogleSheets;
using Common.Scripts.Network;
using Common.Scripts.Tutorial;
using EnchantedLand.Common.Armor;
using EnchantedLand.Common.Damage;
using EnchantedLand.Common.Race;
using EnchantedLand.Common.Weapon;
using EnchantedLand.Game;
using Managers;
using Tanks.Common.Scripts.Player;
using Util;
using Zenject;
namespace Installers
{
    public class ManagersInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            //Bind Manager
            Container.BindInterfacesAndSelfTo<GoogleSheetsService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<UiManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SceneLoader>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<TutorialManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<RaceManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DamageManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ArmorManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<WeaponManager>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<EnemyesManager>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<NetworkService>().AsSingle();
        }
    }
}