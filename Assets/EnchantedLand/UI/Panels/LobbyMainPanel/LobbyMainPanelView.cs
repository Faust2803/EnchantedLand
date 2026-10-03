
using System.Collections.Generic;
using Common.Scripts.Utils;
using TMPro;
using UnityEngine;

using UnityEngine.UI;
using Utils;

namespace UI.Panels
{
    public class LobbyMainPanelView : BasePanelView
    {
        [Space]
        [SerializeField] private Button  _startButton;
        [SerializeField] private List<ParamsPanel.ParamsPanel>  _paramsPanel;
        [SerializeField] private Button  _shopButton;
        [SerializeField] private TextMeshProUGUI  _freeAndSkilledText;
        [SerializeField] private TextMeshProUGUI  _description;
        [SerializeField] private RaseSelector.RaseSelector _raseSelector;
        [SerializeField] private ScrollRect _scrollRect;
        
        public  RaseSelector.RaseSelector RaseSelector => _raseSelector;
        public  TextMeshProUGUI Description => _description;
        public  TextMeshProUGUI FreeAndSkilledText => _freeAndSkilledText;
        public Button  StartButton => _startButton;
        public ScrollRect ScrollRect => _scrollRect;
        public List<ParamsPanel.ParamsPanel>  ParamsPanel => _paramsPanel;
        
        
        protected override BaseMediator CreateMediator()
        {
            _mediator = new LobbyMainPanelMediator();
            return _mediator;
        }
    }
}