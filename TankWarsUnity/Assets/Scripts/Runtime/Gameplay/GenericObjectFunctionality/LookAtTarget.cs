namespace TankWars.Runtime.Gameplay.GenericObjectFunctionality
{
    using UnityEngine;

    public class LookAtTarget : MonoBehaviour
    {
        [System.Serializable]
        private struct RotationAxes
        {
            public bool xAxis;
            public bool yAxis;
            public bool zAxis;
        }

        [SerializeField]
        private Transform target = null;

        [SerializeField]
        private RotationAxes rotationAxes = new RotationAxes()
            {
                xAxis = true,
                yAxis = true,
                zAxis = true
            };

        [SerializeField]
        private bool isInverted = false;
        
        [SerializeField]
        private bool lerpWhileLookingAtTarget = false;

        [SerializeField, Min(0.1f)]
        private float lerpingSpeed = 1f;

        private Vector3 ForwardVector
        {
            get
            {
                return new Vector3
                {
                    x = rotationAxes.xAxis ? target.transform.forward.x : 0, 
                    y = rotationAxes.yAxis ? target.transform.forward.y : 0, 
                    z = rotationAxes.zAxis ? target.transform.forward.z : 0 
                };
            }
        }

        #region Unity Methods

        private void Update()
        {
            UpdateLookRotation();
        }

        #endregion

        private void UpdateLookRotation()
        {
            if(ForwardVector.magnitude == 0)
            {
                return; 
            }

            Vector3 fowardVector = isInverted ? ForwardVector : ForwardVector * -1;
            Quaternion targetRotation = Quaternion.LookRotation(fowardVector);
            transform.rotation = lerpWhileLookingAtTarget ?
                Quaternion.Lerp(transform.localRotation, targetRotation, lerpingSpeed * Time.deltaTime) :
                targetRotation;
        }
    }
}
