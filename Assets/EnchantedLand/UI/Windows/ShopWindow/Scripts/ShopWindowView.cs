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
        [SerializeField] private Image  _itemImage;
        [SerializeField] private TextMeshProUGUI  _titleText;
        [SerializeField] private TextMeshProUGUI  _descriptionText;
        [SerializeField] private TextMeshProUGUI  _priceText;
        [SerializeField] private TextMeshProUGUI  _weightItemText;
        [Space]
        [SerializeField] private Button  _finishButton;
        [Space]
        [SerializeField] private InfoPanel  _infoPanel;
        public List<Toggle>  ShopTabToggle => _shopTabToggle;
        
        public Button  InfoButton => _infoButton;
        public Button  ByeButton => _byeButton;
        public Button  LeftButton => _leftButton;
        public Button  RightButton => _rightButton;
        public Button  FinishButton => _finishButton;
        public InfoPanel  InfoPanel => _infoPanel;
        public Sprite  ItemImage
        { 
            set => _itemImage.sprite = value;
        } 

        public string TitleText
        {
            get => _titleText.text;
            set
            {
                _titleText.text = value;
            } 
        } 
        public string  DescriptionText
        {
            get => _descriptionText.text;
            set
            {
                _descriptionText.text = value;
            } 
        }
        
        public int  MoneyText
        {
            set
            {
                _moneyText.text = value.ToString();
            } 
        }
        
        public string  PriceText
        {
            get => _priceText.text;
            set
            {
                _priceText.text = value + " монет";
            } 
        }
        
        public string  WeightItemText
        {
            get => _weightItemText.text;
            set
            {
                _weightItemText.text = value+ "кг." ;
            } 
        }
        
        public int  WeightText
        {
            set
            {
                _weightText.text = value+ "кг." ;
            } 
        }

        protected override BaseMediator CreateMediator()
        {
            _mediator = new ShopWindowMediator();
            return _mediator;
        }
    }
}