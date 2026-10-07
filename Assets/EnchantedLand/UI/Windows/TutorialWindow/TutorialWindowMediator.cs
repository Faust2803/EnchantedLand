
using Common.Scripts.Network;
using Common.Scripts.Tutorial;
using Managers;
using Tanks.Common.Scripts.Player;
using Util;
using Zenject;

namespace UI.Windows
{
    public class TutorialWindowMediator :BaseWindowMediator <TutorialWindowView, TutorialWindowData>
    {
        private TutorialManager _tutorialManager;
        
        [Inject]
        private void Construct(TutorialManager tutorialManager)
        {
            _tutorialManager = tutorialManager;
        }
        
        protected override void ShowStart()
        {
            base.ShowStart();
            
            Target.BankButton.onClick.AddListener(OnOkButton);
            Target.CancelButton.onClick.AddListener(OnCancelButtonClicked);
            
            
            var tutorial = _tutorialManager.CurrentTutorial;
            
            Target.TitleText.text = tutorial.Title;
            Target.DescriptionText.text = tutorial.Description;
        }

        protected override void CloseStart()
        {
            Target.BankButton.onClick.AddListener(OnOkButton);
            base.CloseStart();
        }
        
        private void OnOkButton()
        {
            _tutorialManager.NextTutorial();
            CloseSelf();
        }
        private void OnCancelButtonClicked()
        {
            CloseSelf();
        }
    }
}