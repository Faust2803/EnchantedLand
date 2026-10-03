using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels
{
    public class LoadingPanelView : BasePanelView
    {
        [Space]
        [SerializeField] private Button  _startButton;
        [SerializeField] private Slider  _progressSlider;
        
        public Button  StartButton => _startButton;
        public Slider  ProgressSlider => _progressSlider;
        
        protected override BaseMediator CreateMediator()
        {
            _mediator = new LoadingPanelMediator();
            return _mediator;
        }
    }
}