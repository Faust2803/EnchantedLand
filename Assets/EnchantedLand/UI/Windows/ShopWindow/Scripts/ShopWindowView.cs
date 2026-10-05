using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class ShopWindowView : BaseView
    {
       
        [SerializeField] private List<Toggle>  _shopTabToggle;
        [Space]
        [SerializeField] private TextMeshProUGUI  _moneyText;
        [SerializeField] private TextMeshProUGUI  _weightText;
        [Space]
        [SerializeField] private Button  _infoButton;
        [Space]
        [SerializeField] private Button  _byeButton;
        [SerializeField] private Button  _leftButton;
        [SerializeField] private Button  _rightButton;
        [Space]
        [SerializeField] private RawImage  _itemImage;
        [SerializeField] private TextMeshProUGUI  _titleText;
        [SerializeField] private TextMeshProUGUI  _descriptionText;
        [Space]
        [SerializeField] private Button  _finishButton;
        public List<Toggle>  ShopTabToggle => _shopTabToggle;
        
        public Button  InfoButton => _infoButton;
        public Button  ByeButton => _byeButton;
        public Button  LeftButton => _leftButton;
        public Button  RightButton => _rightButton;
        public Button  FinishButton => _finishButton;
        public RawImage  ItemImage => _itemImage;
        public TextMeshProUGUI  MoneyText => _moneyText;
        public TextMeshProUGUI  WeightText => _weightText;
        public TextMeshProUGUI  TitleText => _titleText;
        public TextMeshProUGUI  DescriptionText => _descriptionText;
        protected override BaseMediator CreateMediator()
        {
            _mediator = new ShopWindowMediator();
            return _mediator;
        }
    }
}