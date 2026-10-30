using Unity.Cinemachine;
using UnityEngine;

namespace Game.Scripts.Camera
{
    public class CameraZoomExtension: CinemachineExtension
    {
        [Range(2f, 6f)]
        public float Zoom = 5f;

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (stage != CinemachineCore.Stage.Body) return;

            state.Lens.OrthographicSize = Zoom;
        }
    }
}