using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class InfoWindowView : BaseView
    {
       
        [Space]
        [SerializeField] private TextMeshProUGUI  _titleText;
        [SerializeField] private TextMeshProUGUI  _descriptionText;
        [Space]
        [SerializeField] private Button  _okButton;
        [SerializeField] private Image  _icon;
        
        public Button  OkButton => _okButton;
        public Image  Icon => _icon;
        
        public TextMeshProUGUI  TitleText => _titleText;
        public TextMeshProUGUI  DescriptionText => _descriptionText;
        
        protected override BaseMediator CreateMediator()
        {
            _mediator = new InfoWindowMediator();
            return _mediator;
        }
    }
}