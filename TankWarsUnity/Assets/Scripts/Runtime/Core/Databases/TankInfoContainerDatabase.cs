namespace TankWars.Runtime.Core.Databases
{
    using TankWars.Runtime.Gameplay.Unlockables; 
    using UnityEngine;

    [CreateAssetMenu(fileName = "TankInfo", menuName = "TankWars/TankInfo")]
    public class TankInfoContainerDatabase : ScriptableObject
    {
        [SerializeField]
        private TankInfoContainer[] tankInfoContainers = null;

        public TankInfoContainer[] TankInfoContainers => tankInfoContainers; 
    }
}
