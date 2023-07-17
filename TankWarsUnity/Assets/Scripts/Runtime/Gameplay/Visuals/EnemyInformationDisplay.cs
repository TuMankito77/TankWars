namespace TankWars.Runtime.Gameplay.Visuals
{
    using UnityEngine.UI; 
    using UnityEngine;
    using TankWars.Runtime.Gameplay.Enemy;
    using TankWars.Runtime.Core.Camera;

    public class EnemyInformationDisplay : MonoBehaviour
    {
        private const int IMAGE_FILL_ORIGIN_LEFT = 0;

        [SerializeField]
        private Image healthBarImage = null;

        [SerializeField]
        private Canvas uiCanvas = null; 

        [SerializeField]
        private EnemyGameplayInformation enemyGameplayInformation = null; 

        #region Unity Methods

        private void Start()
        {
            GameObject gameplayCameraGO = GameObject.FindGameObjectWithTag(CameraRenderModeController.GAMEPLAY_CAMERA_TAG);
            uiCanvas.worldCamera = gameplayCameraGO.GetComponent<Camera>(); 
            enemyGameplayInformation.onEnemyHealthChange += UpdateHealthBarImage; 
        }

        private void OnValidate()
        {
            Debug.Assert(healthBarImage != null, $"{GetType().Name}-{gameObject.name}: You must assign an image component to the Health Bar Image field.");

            healthBarImage.type = Image.Type.Filled;
            healthBarImage.fillMethod = Image.FillMethod.Horizontal;
            healthBarImage.fillOrigin = IMAGE_FILL_ORIGIN_LEFT;
        }

        #endregion

        private void UpdateHealthBarImage(float health)
        {
            float currentHealth = health / enemyGameplayInformation.MaxHealthAmount;
            healthBarImage.fillAmount = Mathf.Clamp01(currentHealth); 
        }
    }
}
