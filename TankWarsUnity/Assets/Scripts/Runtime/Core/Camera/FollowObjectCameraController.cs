namespace TankWars.Runtime.Core.Camera
{
    using System.Collections;
    using UnityEngine;

    public class FollowObjectCameraController : MonoBehaviour
    {
        [SerializeField]
        private Transform target = null;

        [SerializeField]
        private Vector3 offset = Vector3.zero;

        [SerializeField, Min(1)]
        private float lerpSpeed = 5f;

        [SerializeField, Min(0.01f)]
        private float acceptableRange = 0.01f;

        #region Unity Methods

        private void FixedUpdate()
        {
            if (target == null)
            {
                return;
            }

            if(!Application.isPlaying)
            {
                transform.position = target.position + offset;
                return; 
            }

            float distanceToTarget = Vector3.Distance(target.transform.position + offset, transform.position);

            if (distanceToTarget > acceptableRange)
            {
                transform.position = Vector3.Lerp(transform.position, target.transform.position + offset, lerpSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = target.transform.position + offset; 
            }
        }

        #endregion

        public void ChangeTarget(Transform newTaget)
        {
            target = newTaget;
        }
    }
}

