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
            Subscribe();
            if (_tutorialManager.CurrentTutorialIndex == TutorialStep.EquipItems )
            {
                _uiManager.ForceOpenWindow(WindowType.TutorialWindow);
            }
        }
        
        private void OnStorageChanged(int index, IIdentifiable item, bool isAded)
        {
            _playerManager.Save();
        }
        

        private void Subscribe()
        {
            _playerManager.Data.OnStorageChanged += OnStorageChanged;
        }
        
        private void Unsubscribe()
        {
            _playerManager.Data.OnStorageChanged -= OnStorageChanged;
        }
        
        protected override void CloseStart()
        {
            Unsubscribe();
            base.CloseStart();
        }
    }
}