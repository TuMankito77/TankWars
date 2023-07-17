namespace TankWars.Runtime.Gameplay.Visuals
{
    using TankWars.Runtime.Gameplay.Unlockables;
    using UnityEngine;
    using UnityEngine.UI;

    public class CustomizeTankView : MonoBehaviour
    {
        public const string TANK_SHADER_COLOR_PROPERTY_NAME = "_Main_Color"; 

        [SerializeField]
        private MeshFilter tankPlaceholderMeshFilter = null;

        [SerializeField]
        private MeshRenderer tankPlaceholderMeshRenderer = null;

        [SerializeField]
        private Image tankLockedImage = null; 

        [SerializeField]
        private Transform cameraRotaionParent = null;

        public Color TankColor { get; private set; } = Color.white;

        private readonly Color nonTransparentColor = new Color(255, 255, 255, 1);
        private readonly Color transparentColor = new Color(255, 255, 255, 0);

        public void UpdateTankPlaceHolder(TankInfoContainer tankInfoContainer)
        {
            tankPlaceholderMeshFilter.mesh = tankInfoContainer.Mesh;
            tankPlaceholderMeshRenderer.material = tankInfoContainer.Material;
            tankPlaceholderMeshRenderer.material.SetColor(TANK_SHADER_COLOR_PROPERTY_NAME, TankColor); 
            tankLockedImage.color = tankInfoContainer.IsObjectUnlocked ? transparentColor : nonTransparentColor; 
        }

        public void UpdateTankPlaceHolderColor(Color color)
        {
            TankColor = color;
            tankPlaceholderMeshRenderer.material.SetColor(TANK_SHADER_COLOR_PROPERTY_NAME, TankColor); 
        }

        public void Show()
        {
            gameObject.SetActive(true); 
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        //TODO: Create the controls for rotating the camera around
    }
}
