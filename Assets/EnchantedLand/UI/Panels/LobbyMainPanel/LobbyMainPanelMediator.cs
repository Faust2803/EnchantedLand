
using Tanks.Common.Scripts.Player;
using UI.Windows;

using Zenject;

namespace UI.Panels
{
    public class LobbyMainPanelMediator :BasePanelMediator <LobbyMainPanelView, LobbyMainPanelData>
    {
        private PlayerManager _playerManager;



         
        [Inject]
        private void Construct(PlayerManager playerManager)
        {
    
            _playerManager = playerManager;
        }
		
        protected override void ShowStart()
        {
            base.ShowStart();
            

            
            Target.StartButton.onClick.AddListener(OnStartButtonClicked);
            Target.ShopButton.onClick.AddListener(OnShopButtonClicked);
            Target.BankButton.onClick.AddListener(OnBankButton);

            
            
            OnCurrencvCange();
        }

        
        
        
        private void OnStartButtonClicked()
        {
            /*var result = await _sceneLoader.LoadScene(SceneName.Game.ToString(),
                new Progress<float>(value =>
                {
                    Debug.Log($"Loading: {value:P0}");
                })
            );

            if (result)
            {
                CloseSelf();
            }*/
            _uiManager.OpenWindow(WindowType.ConnectionWindow);
        }

        private void OnCurrencvCange()
        {
            Target.CurrencyText.text =_playerManager.Data.Currency.ToString();
        }

        private void OnShopButtonClicked()
        {
            _uiManager.OpenWindow(WindowType.ShopWindow);
        }

        protected override void CloseStart()
        {
            Target.StartButton.onClick.RemoveListener(OnStartButtonClicked);
            Target.ShopButton.onClick.RemoveListener(OnShopButtonClicked);
            Target.BankButton.onClick.RemoveListener(OnBankButton);
           
            base.CloseStart();
        }

        private void OnBankButton()
        {
            _uiManager.OpenWindow(WindowType.BankWindow);
        }
        
    }
    
}