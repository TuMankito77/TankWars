namespace TankWars.Runtime.Gameplay.AI
{
    using UnityEngine;

    public abstract class BaseEnemy : MonoBehaviour
    {
        [SerializeField, Min(1f)]
        protected float attackingDistance = 3f;

        [SerializeField]
        protected bool drawDebugSpeheres = false;

        #region Unity Methods

        protected virtual void OnDrawGizmos()
        {
            if (!drawDebugSpeheres) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackingDistance);
        }

        #endregion
    }
}
