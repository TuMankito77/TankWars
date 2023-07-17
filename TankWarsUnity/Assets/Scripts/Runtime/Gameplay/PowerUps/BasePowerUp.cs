namespace TankWars.Runtime.Gameplay.PowerUps
{
    using TankWars.Runtime.Core.Events; 
    using UnityEngine;

    public class BasePowerUp : MonoBehaviour
    {
        public const string POWERUP_TAKER_TAG = "Player"; 

        [SerializeField, Range(1, 10)]
        private int duration = 5;

        [SerializeField]
        private float boostAmount = 5; 

        [SerializeField]
        private PowerUpType powerUpType = PowerUpType.None;

        [SerializeField]
        private GameObject disapperingParticleEffectPrefab = null;

        public int Duration => duration;
        public float BoostAmount => boostAmount; 
        public int TakerGameObjectInstanceId { get; private set; } = int.MinValue; 

        #region Unity Engine

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag != POWERUP_TAKER_TAG)
            {
                return; 
            }

            TakerGameObjectInstanceId = other.gameObject.GetInstanceID(); 
            
            OnPowerUpTaken();
            EventManager.Instance.Dispatch(powerUpType, this);
            Destroy(this.gameObject); 
        }

        #endregion

        protected virtual void OnPowerUpTaken()
        {
            if (disapperingParticleEffectPrefab != null)
            {
                Instantiate(disapperingParticleEffectPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
