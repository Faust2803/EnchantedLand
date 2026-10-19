using Common.Scripts.Tutorial;
using EnchantedLand.Common.Armor;
using EnchantedLand.Common.Scripts;
using EnchantedLand.Common.Weapon;
using Tanks.Common.Scripts.Player;
using Zenject;

namespace UI.Windows
{
    public class PlayerUIMediator :BaseWindowMediator <PlayerUIWindowView, UIData>
    {
        
        private PlayerManager _playerManager;
        private ArmorManager _armorManager;
        private WeaponManager _weaponManager;
        private TutorialManager _tutorialManager;
        
        [Inject]
        private void Construct(
            PlayerManager playerManager,
            ArmorManager armorManager,
            WeaponManager weaponManager,
            TutorialManager tutorialManager)
        {
            _playerManager = playerManager;
            _armorManager = armorManager;
            _weaponManager = weaponManager;
            _tutorialManager = tutorialManager;
        }
        
        protected override void ShowStart()
        {
            base.ShowStart();
        }
        
        protected override void OpenFinish()
        {
            base.OpenFinish();
            if (_tutorialManager.CurrentTutorialIndex == TutorialStep.EquipItems )
            {
                _uiManager.ForceOpenWindow(WindowType.TutorialWindow);
            }
        }

        
        
        protected override void CloseStart()
        {

            base.CloseStart();
        }
    }
}