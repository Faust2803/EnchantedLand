using System;
using EnchantedLand.Common.DataModels;
using EnchantedLand.Common.Race;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels.ParamsPanel
{
    public class ParamsPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _baseParamsText;
        [SerializeField] private TextMeshProUGUI _additionalParamsText;
        [SerializeField] private Button _plusButton;
        [SerializeField] private Button _minusButton;
        [SerializeField] private RaseSelector.RaseSelector _raseSelector;
        [SerializeField] private CharacteristicsID _characteristicId;
        

        private int _change;
        private int _raseParam;
        private int _free;
        
        private const int MAX_PARAMS_STARTED = 14;
        private const int MIN_PARAMS_STARTED = 4;
        
        
        public Action<CharacteristicsID, bool, int> OnChange;
        public int Result => _change;
        public CharacteristicsID CharacteristicId => _characteristicId;
        private void OnEnable()
        {
            _plusButton.onClick.AddListener(OnPlusButtonClick);
            _minusButton.onClick.AddListener(OnMinusButtonClick);
            _raseSelector.OnRaseSelected += RaseSelected;
            _free = 0;
            SetParams(LobbyMainPanelMediator.BASE_PARAMS_RESULT);
            ButtonsIsActive();
        }

        private void RaseSelected(Race obj)
        {
            switch (_characteristicId)
            {
                case CharacteristicsID.Strength:
                    _raseParam = obj.Characteristics.Strength;
                    break;
                case CharacteristicsID.Intelligence:
                    _raseParam = obj.Characteristics.Intelligence;
                    break;
                case CharacteristicsID.Agility:
                    _raseParam = obj.Characteristics.Agility;
                    break;
                case CharacteristicsID.Charisma:
                    _raseParam = obj.Characteristics.Charisma;
                    break;
                case CharacteristicsID.Endurance:
                    _raseParam = obj.Characteristics.Endurance;
                    break;
            }
            _additionalParamsText.text = _raseParam.ToString();
            SetParams(_change);
        }
        
        public void SetParams(int baseParams)
        {
            _change = baseParams;
            _baseParamsText.text = _change.ToString();
        }

        public void ButtonsIsActive(int free = 0)
        {
            _plusButton.interactable = _change < MAX_PARAMS_STARTED;
            _minusButton.interactable =_change > MIN_PARAMS_STARTED;
            _free = free;
            if (_free == 0)
            {
                _plusButton.interactable = false;
            }
        }

        private void OnMinusButtonClick()
        {
            if (_change > MIN_PARAMS_STARTED)
            {
                OnChange?.Invoke(_characteristicId, false, _change-1);
            }
        }

        private void OnPlusButtonClick()
        {
            if (_change < MAX_PARAMS_STARTED)
            {
                OnChange?.Invoke(_characteristicId, true, _change+1);
            }
        }

        private void OnDisable()
        {
            _plusButton.onClick.RemoveListener(OnPlusButtonClick);
            _minusButton.onClick.RemoveListener(OnMinusButtonClick);
        }
    }
}