namespace TankWars.Runtime.Gameplay.Effects
{
    using UnityEngine;

    public class CollisionBounce : MonoBehaviour
    {
        public const string RIGID_BODY_FIELD_NAME = nameof(rigidBody); 
        public const string FLOOR_TAG = "Floor";

        [SerializeField]
        private Rigidbody rigidBody = null;

        [SerializeField, Range(1, 10)]
        private float bouncyness = 5f; 

        #region Unity Methods

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == FLOOR_TAG)
            {
                return;
            }

            Vector3 collisionNormal = collision.contacts[0].normal;
            Vector3 mirorForwardVector = Vector3.Reflect(transform.forward, collisionNormal);
            rigidBody.AddForce(mirorForwardVector * bouncyness, ForceMode.Impulse);
        }

        #endregion
    }
}
