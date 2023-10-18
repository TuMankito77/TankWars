namespace TankWars.Runtime.Gameplay.Visuals
{
    using UnityEngine;

    public class StudioDisplayView : MonoBehaviour
    {
        [SerializeField]
        private MeshFilter placeHolderMeshFilter = null;

        [SerializeField]
        protected MeshRenderer placeHolderMeshRenderer = null;

        [SerializeField]
        private Transform studioCenterPoint = null;

        public virtual string PlaceHolderColorPropertyName => "MainColor"; 
        
        public virtual void UpdatePlaceHolderAppearance(Mesh mesh, Material material)
        {
            placeHolderMeshFilter.mesh = mesh;
            placeHolderMeshRenderer.material = material;
        }

        public virtual void UpdatePlaceHolderMesh(Mesh mesh)
        {
            placeHolderMeshFilter.mesh = mesh; 
        }

        public virtual void UpdatePlaceHolderMaterial(Material material)
        {
            placeHolderMeshRenderer.material = material; 
        }

        public virtual void UpdatePlaceHolderMaterialColor(Color color)
        {
            placeHolderMeshRenderer.material.SetColor(PlaceHolderColorPropertyName, color); 
        }

        public virtual void Show()
        {
            gameObject.SetActive(true); 
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
