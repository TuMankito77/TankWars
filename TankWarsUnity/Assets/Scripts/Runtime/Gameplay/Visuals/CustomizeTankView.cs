namespace TankWars.Runtime.Gameplay.Visuals
{
    using UnityEngine;
    using UnityEngine.UI;

    public class CustomizeTankView : StudioDisplayView
    {
        [SerializeField]
        private Image tankLockedImage = null; 

        [SerializeField]
        private Transform cameraRotaionParent = null;

        public override string PlaceHolderColorPropertyName => "_Main_Color";

        public Color TankColor { get; private set; } = Color.white;

        private readonly Color nonTransparentColor = new Color(255, 255, 255, 1);
        private readonly Color transparentColor = new Color(255, 255, 255, 0);

        public override void UpdatePlaceHolderAppearance(Mesh mesh, Material material)
        {
            base.UpdatePlaceHolderAppearance(mesh, material);
            placeHolderMeshRenderer.material.SetColor(PlaceHolderColorPropertyName, TankColor);
        }

        public override void UpdatePlaceHolderMaterialColor(Color color)
        {
            base.UpdatePlaceHolderMaterialColor(color);
            TankColor = color;
        }

        public void UpdateTankLockedIcon(bool isTankUnlocked)
        {
            tankLockedImage.color = isTankUnlocked ? transparentColor : nonTransparentColor;
        }

        //TODO: Create the controls for rotating the camera around
        //TODO: Move the functionality for rotating the camera around to the parent class
    }
}
