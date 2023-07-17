namespace TankWars.Runtime.Gameplay.Guns
{
    using TankWars.Runtime.Gameplay; 
    using TankWars.Runtime.Gameplay.ObjectManagement; 
    using TankWars.Runtime.Core.Events; 
    using UnityEngine;
    using System.Collections;

    public class Bullet : PoolObject
    {
        public const string BULLET_TAG = "bullet";
        private const string TRIGGER_IGNORE_TAG = "TriggerIgnore";

        [SerializeField, Min(1)]
        private float damageAmount = 10f;

        [SerializeField, Min(1)]
        private float movementSpeed = 10f;

        [SerializeField]
        private ParticleSystem explotionParticleSystem = null; 

        [SerializeField]
        private Collider bulletCollider = null;

        public float DamageAmount => damageAmount; 
        public int DamagedGameObjectInstanceId { get; private set; } = int.MinValue; 

        #region Unity Methods

        private void Update()
        {
            MoveForward(); 
        }

        private void OnEnable()
        {
            StartCoroutine(DisableBullet()); 
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == TRIGGER_IGNORE_TAG)
            {
                return;
            }

            OnBulletDestoyed(); 
            SpawnExplotionParticleEffects(); 
            gameObject.SetActive(false);
            DamagedGameObjectInstanceId = other.gameObject.GetInstanceID(); 
            EventManager.Instance.Dispatch(GameplayEvent.EntityDamaged, this);
        }

        #endregion

        //NOTE: Override this method in the future so that we can have bullets that behave different in terms of movement
        //For instance, pursuing bullet
        protected virtual void MoveForward()
        {
            transform.Translate(transform.forward * movementSpeed * Time.deltaTime, Space.World); 
        }

        protected virtual void OnBulletDestoyed()
        {

        }

        private void SpawnExplotionParticleEffects()
        {
            if(explotionParticleSystem != null)
            {
                Instantiate(explotionParticleSystem); 
            }
        }

        private IEnumerator DisableBullet()
        {
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false); 
        }
    }
}
