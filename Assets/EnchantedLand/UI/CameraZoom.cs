using UnityEngine;
using Unity.Cinemachine;

public class CameraZoom : MonoBehaviour
{
    private CinemachineCamera vcam;

    [Header("Camera Zoom Settings")]
    public float zoomSpeed = 5f;        // Швидкість зміни зуму
    public float minZoom = 3f;          // Максимальне наближення (менше значення = ближче)
    public float maxZoom = 12f;         // Максимальне віддалення (більше значення = далі)
    public float smoothness = 10f;      // Плавність зуму

    private float targetZoom;           // Цільовий рівень зуму
    void Start()
    {
        vcam = GetComponent<CinemachineCamera>();
        if (vcam != null)
        {
            targetZoom = vcam.Lens.OrthographicSize;
        }

}
        void Update()
        {
            if (vcam == null) return;

            // Використовуємо системний ScrollDelta, він стабільніший у нових версіях Unity
            float scrollInput = Input.mouseScrollDelta.y;

            if (scrollInput != 0f)
            {
                // Змінюємо цільовий зум
                targetZoom -= scrollInput * zoomSpeed * Time.deltaTime;
                targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
            }

            // 1. Отримуємо копію поточних налаштувань лінзи
            LensSettings lens = vcam.Lens;

            // 2. Плавно змінюємо розмір у цій копії
            lens.OrthographicSize = Mathf.Lerp(lens.OrthographicSize, targetZoom, smoothness * Time.deltaTime);

            // 3. ОБОВ'ЯЗКОВО записуємо оновлену лінзу назад у камеру
            vcam.Lens = lens;
        }
    
}
