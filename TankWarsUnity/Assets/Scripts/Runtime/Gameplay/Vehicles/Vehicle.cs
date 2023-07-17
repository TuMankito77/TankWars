namespace TankWars.Runtime.Gameplay.Vehicles
{
    using UnityEngine;
    using TankWars.Runtime.Gameplay.Effects;

    public class Vehicle : MonoBehaviour
    {
        public const string VEHICLE_RIGIDBODY_FIELD_NAME = nameof(vehicleRigidBody);

        [SerializeField, Range(1, 10)]
        protected float maxSpeed = 1;

        [SerializeField, Range(1, 10)]
        protected float maxAcceleration = 10;

        [SerializeField, Range(1, 10)]
        protected float maxTurningSpeed = 10f;

        [SerializeField, Range(1, 10)]
        protected float maxTurningAcceleration = 5f;

        [SerializeField]
        private Rigidbody vehicleRigidBody = null;

        private float currentSpeed = 0;
        private float currentTurningSpeed = 0;

        public void MoveTowards(Vector3 position, bool isInLocalCoordinates)
        {
            Vector3 worldSpaceDirection = isInLocalCoordinates ?
                 new Vector3(vehicleRigidBody.position.x + position.x, vehicleRigidBody.position.y, vehicleRigidBody.position.z + position.y) :
                 new Vector3(position.x, vehicleRigidBody.position.y, position.z);

            Vector3 forwardVectorDirection = worldSpaceDirection - transform.position;
            forwardVectorDirection.Normalize();
            Quaternion targetRoation = Quaternion.LookRotation(forwardVectorDirection);

            float maxSpeedChange = maxAcceleration * Time.deltaTime;
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, maxSpeedChange); 
            Vector3 velocityVector = new Vector3(transform.forward.x * currentSpeed, vehicleRigidBody.velocity.y, transform.forward.z * currentSpeed);
            vehicleRigidBody.velocity = Vector3.MoveTowards(vehicleRigidBody.velocity, velocityVector, currentSpeed);

            float maxTurningSpeedChange = maxTurningAcceleration * Time.deltaTime;
            currentTurningSpeed = Mathf.MoveTowards(currentTurningSpeed, maxTurningSpeed, maxTurningSpeedChange);
            vehicleRigidBody.rotation = Quaternion.RotateTowards(vehicleRigidBody.rotation, targetRoation, currentTurningSpeed);
        }
    }
}
