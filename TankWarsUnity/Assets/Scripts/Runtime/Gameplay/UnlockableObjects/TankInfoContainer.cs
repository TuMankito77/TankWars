namespace TankWars.Runtime.Gameplay.Unlockables
{
    using TankWars.Runtime.Core;
    using TankWars.Runtime.Gameplay.Vehicles;
    using UnityEngine;

    [System.Serializable]
    public class TankInfoContainer : UnlockableObject
    {
        [SerializeField]
        private Tank tankPrefab; 

        [SerializeField]
        private float movementSpeed;

        [SerializeField]
        private float aimingSpeed;

        public float MovementSpeed => movementSpeed;
        public float AimingSpeed => aimingSpeed;
        public Tank TankPrefab => tankPrefab; 
    }
}
