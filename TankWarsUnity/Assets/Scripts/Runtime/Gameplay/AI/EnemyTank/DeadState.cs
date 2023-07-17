namespace TankWars.Runtime.Gameplay.AI.EnemyTank
{
    using System.Collections.Generic;
    using TankWars.Runtime.Core.AI.FiniteStateMachine;
    using UnityEngine;

    public class DeadState : State<EnemyTankAI>
    {
        public DeadState(EnemyTankAI sourceToControl, StateMachine<EnemyTankAI> sourceStateMachine) : base(sourceToControl, sourceStateMachine)
        {

        }

        public override void Enter()
        {
            entityController.Collider.gameObject.SetActive(false);
            List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
            MeshRenderer rootGameObjectMeshRenderer = entityController.GetComponent<MeshRenderer>(); 
            
            if(rootGameObjectMeshRenderer != null)
            {
                meshRenderers.Add(rootGameObjectMeshRenderer);
            }

            meshRenderers.AddRange(entityController.GetComponentsInChildren<MeshRenderer>());
            
            foreach(MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.gameObject.SetActive(false); 
            }
        }

        public override void LogicUpdate()
        {
            
        }

        public override void PhysicsUpdate()
        {
            
        }

        public override void Exit()
        {

        }
    }

}

