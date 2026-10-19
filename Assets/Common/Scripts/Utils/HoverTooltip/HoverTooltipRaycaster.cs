using UnityEngine;

namespace Common.Scripts.Utils
{
    public class HoverTooltipRaycaster : MonoBehaviour
    {
        [SerializeField] private Camera targetCamera;
        [SerializeField] private float maxDistance = 100f;

        private HoverTooltip _current;

        private void Update()
        {
            var ray = targetCamera.ScreenPointToRay(Input.mousePosition);

            HoverTooltip next = null;

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
                next = hit.collider.GetComponentInParent<HoverTooltip>();

            if (next == _current)
                return;

            _current?.StopHover();
            _current = next;
            _current?.StartHover();
        }

        private void OnDisable()
        {
            _current?.StopHover();
            _current = null;
        }
    }
}