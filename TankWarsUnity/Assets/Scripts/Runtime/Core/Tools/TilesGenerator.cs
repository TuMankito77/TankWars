namespace TankWars.Runtime.Tools
{
    using Unity.AI.Navigation;
    using UnityEngine;

    public class TilesGenerator : MonoBehaviour
    {
        [SerializeField]
        private GameObject tilePrefab = null;

        [SerializeField]
        private Vector2 tileSize = Vector2.one;

        [SerializeField]
        private Vector2Int gridSize = Vector2Int.zero;

        public void GenerateGrid()
        {
            NavMeshModifier[] navMeshTiles = GetComponentsInChildren<NavMeshModifier>();
            
            for(int i = 0; i < navMeshTiles.Length; i++)
            {
                DestroyImmediate(navMeshTiles[i].gameObject);
            }

            Vector3 startPositition = transform.position;

            for(int i = 0; i < gridSize.x; i++)
            {
                for(int j = 0; j < gridSize.y; j++)
                {
                    GameObject tile = Instantiate(tilePrefab, transform);
                    NavMeshModifier navMeshModifier = tile.GetComponent<NavMeshModifier>();
                    
                    if(navMeshModifier == null)
                    {
                        navMeshModifier = tile.AddComponent<NavMeshModifier>();
                    }

                    navMeshModifier.overrideArea = true;
                    tile.transform.position = startPositition;
                    
                    float xAxisOffset = (tileSize.x / 2) +
                                        (tileSize.x * i) -
                                        (tileSize.x * gridSize.x / 2f);

                    float yAxisOffset = (tileSize.y / 2) +
                                        (tileSize.y * j) -
                                        (tileSize.y * gridSize.y / 2f);
                  
                    tile.transform.position += transform.right * xAxisOffset + transform.forward * yAxisOffset;
                    tile.transform.rotation = transform.rotation;
                }
            }
        }

        public void DeleteGrid()
        {
            NavMeshModifier[] navMeshTiles = GetComponentsInChildren<NavMeshModifier>();

            for (int i = 0; i < navMeshTiles.Length; i++)
            {
                DestroyImmediate(navMeshTiles[i].gameObject);
            }
        }
    }
}

