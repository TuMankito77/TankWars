namespace TankWars.Runtime.Gameplay.GenericObjectFunctionality
{
    using UnityEngine;

    public class CopyTargetRotation : MonoBehaviour
    {
        [SerializeField]
        private Transform target = null;

        [SerializeField]
        private bool updateConstantly = false; 
        
        [SerializeField]
        private bool isLerpEnabled = false;

        [SerializeField, Min(0.1f)]
        private float lerpSpeed = 1f;

        #region Unity Methods

        private void Start()
        {
            UpdateRotation();
        }

        private void Update()
        {
            if (!updateConstantly) return;
            Debug.Assert(target != null, $"{GetType()}-{gameObject}: The target at which the object should look is null, please assign a reference to it in the inspector."); 

            UpdateRotation(); 
        }

        #endregion

        private void UpdateRotation()
        {
            transform.rotation = isLerpEnabled ?
                Quaternion.Lerp(transform.rotation, target.rotation, lerpSpeed * Time.deltaTime) :
                target.rotation; 
        }
    }
}
