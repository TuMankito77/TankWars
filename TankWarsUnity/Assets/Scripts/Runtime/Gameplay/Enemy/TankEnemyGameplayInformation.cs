namespace TankWars.Runtime.Gameplay.Enemy
{
    using TankWars.Runtime.Gameplay.Vehicles;
    using UnityEngine;

    public class TankEnemyGameplayInformation : EnemyGameplayInformation
    {
        [SerializeField]
        private Tank tank = null;

        protected override int GOWithColliderForDetectingImpactIntanceId => tank.gameObject.GetInstanceID();
       
        #region Unity Methods

        protected override void Start()
        {
            base.Start();
            tank.SetTankSpeed(enemyTypeSettings.movementSpeed);
            tank.SetTankAimingSpeed(enemyTypeSettings.aimingSpeed);
        }

        #endregion
    }
}
