namespace TankWars.Runtime.Gameplay.Unlockables
{
    using TankWars.Runtime.Core;
    using TankWars.Runtime.Gameplay.Levels; 
    using TankWars.Runtime.Core.ManagerSystem;
    using UnityEngine;

    public abstract class UnlockableObject 
    {
        [SerializeField]
        private string name;

        [SerializeField]
        private LevelId linkedLevelId;

        [SerializeField]
        private Mesh mesh;

        [SerializeField]
        private Material material;

        public string Name => name;
        public Mesh Mesh => mesh;
        public Material Material => material;
        public bool IsObjectUnlocked => GameManager.GameInformation.IsLevelUnlocked(linkedLevelId);

        private GameManager GameManager => CoreManagers.Instance.GetManager<GameManager>(); 
    }
}
