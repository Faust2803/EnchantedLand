using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class TutorialWindowView : BaseView
    {
       
        [Space]
        [SerializeField] private TextMeshProUGUI  _titleText;
        [SerializeField] private TextMeshProUGUI  _descriptionText;
        [SerializeField] private Button  _bankButton;
        [SerializeField] private Button  _cancelButton;
        
        public Button  BankButton => _bankButton;
        public Button CancelButton => _cancelButton;
        public TextMeshProUGUI  DescriptionText => _descriptionText;
        public TextMeshProUGUI  TitleText => _titleText;
        protected override BaseMediator CreateMediator()
        {
            _mediator = new TutorialWindowMediator();
            return _mediator;
        }
    }
}