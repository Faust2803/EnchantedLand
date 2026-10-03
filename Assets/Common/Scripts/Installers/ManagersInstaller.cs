using Common.Scripts.GoogleSheets;
using Common.Scripts.Network;
using Common.Scripts.Tutorial;
using EnchantedLand.Common.Race;
using Managers;
using Tanks.Common.Scripts;
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
            
            Container.BindInterfacesAndSelfTo<NetworkService>().AsSingle();
        }
    }
}