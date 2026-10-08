using System;
using EnchantedLand.Common.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class InfoPanel : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _title;
        [SerializeField] TextMeshProUGUI _description;
        [SerializeField] Button _closeButton;


        public void SetInfo(IIdentifiable info)
        {
            switch (info.Type)
            {
                case ItemType.None:
                    break;
                case ItemType.Armor:
                    break;
                case ItemType.Weapon:
                    break;
                case ItemType.Amulete:
                    break;
                case ItemType.Other:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _title.text = info.Title;
            _description.text = info.Description;
            gameObject.SetActive(true);
            
        }
        private void OnEnable()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        private void OnCloseButtonClicked()
        {
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        }
    }
}