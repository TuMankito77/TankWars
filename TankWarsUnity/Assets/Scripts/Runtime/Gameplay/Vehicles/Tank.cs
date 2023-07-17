namespace TankWars.Runtime.Gameplay.Vehicles
{
    using TankWars.Runtime.Core.Events;
    using TankWars.Runtime.Gameplay;
    using TankWars.Runtime.Gameplay.PowerUps;
    using TankWars.Runtime.Gameplay.Guns;
    using UnityEngine;
    using System;
    using TankWars.Runtime.Core;

    public class Tank : Vehicle
    {
        public const string GUN_TURRET_FIELD_NAME = nameof(gunTurret); 
        private const float MIN_VEHICLE_SPEED = 1;
        
        [SerializeField]
        private GunTurret gunTurret = null;

        public void AimTurret(Vector3 target, bool isInLocalCoordinates)
        {
            gunTurret.AimTowards(target, isInLocalCoordinates);
        }

        public void Fire()
        {
            gunTurret.Fire();
        }

        public void SetTankSpeed(float amount)
        {
            maxSpeed = Mathf.Max(MIN_VEHICLE_SPEED, amount);
        }

        public void SetTankAimingSpeed(float amount)
        {
            gunTurret.RotationSpeed = Mathf.Max(5, amount);
        }
    }
}
