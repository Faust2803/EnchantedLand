using System;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Scripts.Utils
{
    public class DoubleClickButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private float _doubleClickTime = 0.3f;

        public Action OnDoubleClick;

        private float _lastClickTime;

        private void Awake()
        {
            if (_button == null)
                _button = GetComponent<Button>();

            _button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            float currentTime = Time.unscaledTime;

            if (currentTime - _lastClickTime <= _doubleClickTime)
            {
                _lastClickTime = 0f;
                OnDoubleClick?.Invoke();
                return;
            }

            _lastClickTime = currentTime;
        }
    }
}