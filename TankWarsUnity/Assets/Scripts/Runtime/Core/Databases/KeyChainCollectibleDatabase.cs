namespace TankWars.Runtime.Core.Databases
{
    using TankWars.Runtime.Gameplay.Unlockables; 
    using UnityEngine;

    [CreateAssetMenu(fileName = "KeyChainCollectible", menuName = "TankWars/KeyChainCollectible")]
    public class KeyChainCollectibleDatabase : ScriptableObject
    {
        [SerializeField]
        private KeyChainCollectible[] keyChainCollectibles = null;

        public KeyChainCollectible[] KeyChainCollectibles => keyChainCollectibles; 
    
        //TODO:
        //- Create a prefab for the Database Manager
        //- Test that everything is working correctly
        //- Update the CreateAssetMenu property for the TankInfoContainerDatabase and the KeyChainCollectibleDatabase.
        //- Erase all the TODO lists.
    }
}
