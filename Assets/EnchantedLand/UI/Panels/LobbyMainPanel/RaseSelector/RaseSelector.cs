using System;
using System.Collections.Generic;
using EnchantedLand.Common.Race;
using SO.Scripts;
using Tanks.Common.Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;


namespace UI.Panels.RaseSelector
{
    public class RaseSelector : MonoBehaviour
    {
        public Action<Race> OnRaseSelected;
        public List<Image>  _items;
        public int _currentRase;
        
        
        [SerializeField] private Button _rightButton;
        [SerializeField] private Button _leftButton;
        [SerializeField] private Transform _avatarConteyner;
        [SerializeField] private Image _imagePrefab;
        [SerializeField] private TextMeshProUGUI _title;
        
        [Inject] private RaceManager _raceManager;
        [Inject] private PlayerManager _playerManager;
        
        public Race CurrentRace => _raceManager.Races[_playerManager.Data.RaseId];

        private void Start()
        {
            _currentRase = _playerManager.Data.RaseId; 
            AddAllAvtar();
            OnRaseChanged();
        }

        private void AddAllAvtar()
        {
            for (var i = 0; i < _raceManager.Count; i++)
            {
                var go = AddAvtar(i);
                go.gameObject.SetActive(i == _currentRase);
            }
        }
        
        private Image AddAvtar(int index)
        {
            var item = Instantiate(_imagePrefab, _avatarConteyner);

            item.sprite = _raceManager.Races[index].Image;
            item.preserveAspect = true;

           
            _items.Add(item);
            
            return item;
        }

        private void OnEnable()
        {
            _rightButton.onClick.AddListener(OnRightButtonClick);
            _leftButton.onClick.AddListener(OnLeftButtonClick);
        }

        private void OnLeftButtonClick()
        {
           
            if (_currentRase == 0 )
            {
                _currentRase = _raceManager.Count - 1;
            }
            else
            {
                _currentRase--;
            }
            
            OnRaseChanged();
        }

        private void OnRightButtonClick()
        {
            if (_currentRase == _raceManager.Count - 1 )
            {
                _currentRase = 0;
            }
            else
            {
                _currentRase++;
            }
            OnRaseChanged();
        }

        private void OnDisable()
        {
            _rightButton.onClick.RemoveListener(OnRightButtonClick);
            _leftButton.onClick.RemoveListener(OnLeftButtonClick);
        }


        private void OnRaseChanged()
        {
            for (var i = 0; i < _raceManager.Count; i++)
            {
                _items[i].gameObject.SetActive(i == _currentRase);
            }
            _playerManager.Data.RaseId = _currentRase;
            _title.text = $"Расса {_raceManager.Races[_playerManager.Data.RaseId].Title}";
            
            OnRaseSelected?.Invoke(_raceManager.Races[_playerManager.Data.RaseId]); 
        }
    }
}