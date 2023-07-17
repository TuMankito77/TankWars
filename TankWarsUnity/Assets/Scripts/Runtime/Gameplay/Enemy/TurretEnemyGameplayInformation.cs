namespace TankWars.Runtime.Gameplay.Enemy
{
    using TankWars.Runtime.Gameplay.Guns;
    using UnityEngine; 

    public class TurretEnemyGameplayInformation : EnemyGameplayInformation
    {
        [SerializeField]
        private GunTurret gunTurret = null;

        protected override int GOWithColliderForDetectingImpactIntanceId => gunTurret.gameObject.GetInstanceID();

        #region Unity Methods

        protected override void Start()
        {
            base.Start();
            gunTurret.RotationSpeed = enemyTypeSettings.aimingSpeed; 
        }

        #endregion
    }
}
