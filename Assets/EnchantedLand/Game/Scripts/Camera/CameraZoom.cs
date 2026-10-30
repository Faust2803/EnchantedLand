using UnityEngine;

namespace Game.Scripts.Camera
{
    public class CameraZoom : MonoBehaviour
    {

        [Header("Camera Zoom Settings")]
        [SerializeField] private CameraZoomExtension _zoomExtension;

        [SerializeField] private float _zoomSpeed = 11f;
        [SerializeField] private float _minZoom = 2f;
        [SerializeField] private float _maxZoom = 6f;
        [SerializeField] private float _smoothness = 10f;

        private float _targetZoom;

        private void Start()
        {
            _targetZoom = _zoomExtension.Zoom;
        }

        private void Update()
        {
            float scroll = Input.mouseScrollDelta.y;

            if (scroll != 0f)
            {
                _targetZoom = Mathf.Clamp(
                    _targetZoom - scroll * _zoomSpeed * Time.deltaTime,
                    _minZoom,
                    _maxZoom
                );
            }

            _zoomExtension.Zoom = Mathf.Lerp(
                _zoomExtension.Zoom,
                _targetZoom,
                _smoothness * Time.deltaTime
            );
        }
    }
}
