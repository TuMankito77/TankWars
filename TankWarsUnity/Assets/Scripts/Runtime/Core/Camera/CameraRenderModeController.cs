namespace TankWars.Runtime.Core.Camera
{
    using UnityEngine;
    using UnityEngine.Rendering.Universal;

    [ExecuteInEditMode]
    public class CameraRenderModeController : MonoBehaviour
    {
        public const string GAMEPLAY_CAMERA_TAG = "GameplayCamera"; 

        private const string MAIN_CAMERA_TAG = "MainCamera";

        private void Start()
        {
            GameObject mainCameraGO = GameObject.FindGameObjectWithTag(MAIN_CAMERA_TAG);
            Camera gameplayCamera = GetComponent<Camera>(); 
            UniversalAdditionalCameraData gameplayCameraData = gameplayCamera.GetUniversalAdditionalCameraData(); 
            
            if(mainCameraGO == null)
            {
                gameplayCameraData.renderType = CameraRenderType.Base; 
                return;
            }

            gameplayCameraData.renderType = CameraRenderType.Overlay;
            Camera mainCamera = mainCameraGO.GetComponent<Camera>();
            UniversalAdditionalCameraData mainCameraData = mainCamera.GetUniversalAdditionalCameraData();
            mainCameraData.cameraStack.Insert(0, gameplayCamera);
        }
    }
}
