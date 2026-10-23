
namespace UI.Windows
{
    public class InfoWindowMediator :BaseWindowMediator <InfoWindowView, InfoWindowData>
    {
        protected override void ShowStart()
        {
            base.ShowStart();
            Target.TitleText.text = Data.Title;
            Target.OkButton.onClick.AddListener(OnOkButtonClicked);
            Target.DescriptionText.text = Data.Description;
            Target.Icon.sprite = Data.Icon;
        }
        
        private void OnOkButtonClicked()
        {
            if (Data.OKAction != null)
            {
                Data.OKAction.Invoke();
            }
            CloseSelf();
        }
        
        protected override void CloseStart()
        {
            Target.OkButton.onClick.RemoveListener(OnOkButtonClicked);
            base.CloseStart();
        }
        
    }
}