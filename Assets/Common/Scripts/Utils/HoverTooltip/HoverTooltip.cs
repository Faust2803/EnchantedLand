using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Common.Scripts.Utils
{
    public class HoverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Tooltip")] [SerializeField] private GameObject tooltip;
        [SerializeField] private float showDelay = 0.7f;

        private CancellationTokenSource _cts;
        private bool _isHovering;

        private void Awake()
        {
            if (tooltip != null)
                tooltip.SetActive(false);
        }

        // UI: курсор вошёл в область элемента
        public void OnPointerEnter(PointerEventData eventData)
        {
            StartHover();
        }

        // UI: курсор покинул область элемента
        public void OnPointerExit(PointerEventData eventData)
        {
            StopHover();
        }

        // Для 3D-объектов вызывай эти методы из Raycast-скрипта
        public void StartHover()
        {
            if (_isHovering)
                return;

            _isHovering = true;
            _cts = new CancellationTokenSource();

            ShowAfterDelay(_cts.Token).Forget();
        }

        public void StopHover()
        {
            _isHovering = false;

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;

            if (tooltip != null)
                tooltip.SetActive(false);
        }

        private async UniTaskVoid ShowAfterDelay(CancellationToken token)
        {
            try
            {
                await UniTask.Delay(
                    System.TimeSpan.FromSeconds(showDelay),
                    cancellationToken: token);

                if (_isHovering && tooltip != null)
                    tooltip.SetActive(true);
            }
            catch (OperationCanceledException)
            {
                // Курсор ушёл до окончания задержки
            }
        }

        private void OnDisable()
        {
            StopHover();
        }
    }
}