
using UnityEngine;

namespace Game.Scripts
{
    [RequireComponent(typeof(Camera))]
    public class CameraController2D : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform _target;
        [SerializeField] private float _followSpeed = 5f;

        [Header("Map")]
        [SerializeField] private SpriteRenderer _map;

        [Header("Zoom")]
        [SerializeField] private float _zoomSpeed = 2f;
        [SerializeField] private float _minZoom = 3f;
        [SerializeField] private float _maxZoom = 10f;
        [SerializeField] private float _zoomSmooth = 5f;

        private Camera _camera;
        private float _targetZoom;

        private bool _isDragging;
        private bool _wasPinching;

        private Vector3 _dragStartMouseScreenPosition;
        private Vector3 _dragStartCameraPosition;
        private Vector3 _currentDragScreenPosition;
        private float _dragStartZoom;

        private float _pinchStartDistance;
        private float _pinchStartZoom;

        private void Awake()
        {
            _camera = GetComponent<Camera>();

            if (!_camera.orthographic)
            {
                Debug.LogError("Camera должна быть Orthographic!");
                enabled = false;
                return;
            }

            if (_map == null)
            {
                Debug.LogError("Назначь SpriteRenderer карты!");
                enabled = false;
                return;
            }

            _targetZoom = Mathf.Clamp(
                _camera.orthographicSize,
                _minZoom,
                _maxZoom
            );

            _camera.orthographicSize = _targetZoom;
        }

        private void LateUpdate()
        {
            bool hasTouch = Input.touchCount > 0;

            if (hasTouch)
            {
                HandleTouchInput();
            }
            else
            {
                _wasPinching = false;

                HandleMouseInput();

                if (!_isDragging)
                    HandleZoom();
            }

            if (_isDragging)
            {
                DragCamera(_currentDragScreenPosition);
            }
            else if (!hasTouch && _target != null)
            {
                FollowTarget();
            }

            transform.position = ClampToMap(transform.position);
        }

        // -------------------------
        // Mouse
        // -------------------------

        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (TryGetMouseWorldPosition(
                        Input.mousePosition,
                        out Vector3 mouseWorld)
                    && IsInsideMap(mouseWorld))
                {
                    BeginDrag(Input.mousePosition);
                }
            }

            if (Input.GetMouseButton(0) && _isDragging)
            {
                _currentDragScreenPosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
            }
        }

        // -------------------------
        // Touch
        // -------------------------

        private void HandleTouchInput()
        {
            // Два пальца: масштабирование.
            if (Input.touchCount >= 2)
            {
                HandlePinchZoom();
                return;
            }

            Touch touch = Input.GetTouch(0);

            // После масштабирования одним оставшимся пальцем
            // можно продолжить перемещать карту.
            if (_wasPinching)
            {
                _wasPinching = false;

                if (TryGetMouseWorldPosition(
                        touch.position,
                        out Vector3 mouseWorld)
                    && IsInsideMap(mouseWorld))
                {
                    BeginDrag(touch.position);
                }
                else
                {
                    _isDragging = false;
                }

                return;
            }

            if (touch.phase == TouchPhase.Began)
            {
                if (TryGetMouseWorldPosition(
                        touch.position,
                        out Vector3 mouseWorld)
                    && IsInsideMap(mouseWorld))
                {
                    BeginDrag(touch.position);
                }
            }

            if (_isDragging &&
                (touch.phase == TouchPhase.Moved ||
                 touch.phase == TouchPhase.Stationary))
            {
                _currentDragScreenPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Ended ||
                touch.phase == TouchPhase.Canceled)
            {
                _isDragging = false;
            }
        }

        private void HandlePinchZoom()
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            float currentDistance = Vector2.Distance(
                touch0.position,
                touch1.position
            );

            // Начало жеста двумя пальцами.
            if (!_wasPinching)
            {
                _wasPinching = true;
                _isDragging = false;

                _pinchStartDistance = currentDistance;
                _pinchStartZoom = _camera.orthographicSize;

                return;
            }

            if (_pinchStartDistance <= 0f)
                return;

            if (currentDistance <= 0f)
                return;

            // Пальцы раздвигаются — приближение.
            // Пальцы сближаются — отдаление.
            _targetZoom = Mathf.Clamp(
                _pinchStartZoom *
                (_pinchStartDistance / currentDistance),
                _minZoom,
                _maxZoom
            );

            _camera.orthographicSize = Mathf.Lerp(
                _camera.orthographicSize,
                _targetZoom,
                1f - Mathf.Exp(-_zoomSmooth * Time.deltaTime)
            );
        }

        // -------------------------
        // Drag
        // -------------------------

        private void BeginDrag(Vector3 screenPosition)
        {
            _isDragging = true;

            _dragStartMouseScreenPosition = screenPosition;
            _currentDragScreenPosition = screenPosition;
            _dragStartCameraPosition = transform.position;
            _dragStartZoom = _camera.orthographicSize;
        }

        private void DragCamera(Vector3 screenPosition)
        {
            Vector3 screenDelta =
                screenPosition - _dragStartMouseScreenPosition;

            float worldUnitsPerPixel =
                (_dragStartZoom * 2f) / Screen.height;

            Vector3 worldDelta = new Vector3(
                screenDelta.x * worldUnitsPerPixel,
                screenDelta.y * worldUnitsPerPixel,
                0f
            );

            Vector3 position =
                _dragStartCameraPosition - worldDelta;

            position.z = _dragStartCameraPosition.z;

            transform.position = ClampToMap(position);
        }

        // -------------------------
        // Zoom with mouse wheel
        // -------------------------

        private void HandleZoom()
        {
            float scroll = Input.mouseScrollDelta.y;

            if (Mathf.Abs(scroll) > 0.01f)
            {
                _targetZoom = Mathf.Clamp(
                    _targetZoom - scroll * _zoomSpeed,
                    _minZoom,
                    _maxZoom
                );
            }

            _camera.orthographicSize = Mathf.Lerp(
                _camera.orthographicSize,
                _targetZoom,
                1f - Mathf.Exp(-_zoomSmooth * Time.deltaTime)
            );
        }

        // -------------------------
        // Follow target
        // -------------------------

        private void FollowTarget()
        {
            Vector3 targetPosition = new Vector3(
                _target.position.x,
                _target.position.y,
                transform.position.z
            );

            float t = 1f - Mathf.Exp(
                -_followSpeed * Time.deltaTime
            );

            transform.position = ClampToMap(
                Vector3.Lerp(
                    transform.position,
                    targetPosition,
                    t
                )
            );
        }

        // -------------------------
        // Map bounds
        // -------------------------

        private bool TryGetMouseWorldPosition(
            Vector3 screenPosition,
            out Vector3 worldPosition)
        {
            Ray ray = _camera.ScreenPointToRay(screenPosition);

            Plane mapPlane = new Plane(
                Vector3.forward,
                new Vector3(
                    0f,
                    0f,
                    _map.bounds.center.z
                )
            );

            if (mapPlane.Raycast(ray, out float distance))
            {
                worldPosition = ray.GetPoint(distance);
                return true;
            }

            worldPosition = Vector3.zero;
            return false;
        }

        private bool IsInsideMap(Vector3 position)
        {
            Bounds bounds = _map.bounds;

            return position.x >= bounds.min.x
                && position.x <= bounds.max.x
                && position.y >= bounds.min.y
                && position.y <= bounds.max.y;
        }

        private Vector3 ClampToMap(Vector3 position)
        {
            Bounds bounds = _map.bounds;

            float halfHeight = _camera.orthographicSize;
            float halfWidth = halfHeight * _camera.aspect;

            float minX = bounds.min.x + halfWidth;
            float maxX = bounds.max.x - halfWidth;
            float minY = bounds.min.y + halfHeight;
            float maxY = bounds.max.y - halfHeight;

            position.x = minX > maxX
                ? bounds.center.x
                : Mathf.Clamp(position.x, minX, maxX);

            position.y = minY > maxY
                ? bounds.center.y
                : Mathf.Clamp(position.y, minY, maxY);

            return position;
        }
    }
}
