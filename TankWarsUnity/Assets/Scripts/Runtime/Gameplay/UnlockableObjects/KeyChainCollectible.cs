namespace TankWars.Runtime.Gameplay.Unlockables
{
    using UnityEngine;

    [System.Serializable]
    public class KeyChainCollectible : UnlockableObject
    {
        [SerializeField]
        private Sprite image;
        
        public Sprite Image => image; 
    }
}
