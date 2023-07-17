namespace TankWars.Runtime.Gameplay.Visuals
{
    using UnityEngine;

    public class StudioDisplayView : MonoBehaviour
    {
        [SerializeField]
        private MeshFilter placeHolderMeshFilter = null;

        [SerializeField]
        private MeshRenderer placeHolderMeshRenderer = null;

        [SerializeField]
        private Transform studioCenterPoint = null; 

        protected virtual void UpdatePlaceHolderAppearance(Mesh mesh, Material material)
        {
            placeHolderMeshFilter.mesh = mesh;
            placeHolderMeshRenderer.material = material;
        }

        protected virtual void UpdatePlaceHolderMesh(Mesh mesh)
        {
            placeHolderMeshFilter.mesh = mesh; 
        }

        protected virtual void UpdatePlaceHolderMaterial(Material material)
        {
            placeHolderMeshRenderer.material = material; 
        }

        protected virtual void Show()
        {
            gameObject.SetActive(true); 
        }

        protected virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
