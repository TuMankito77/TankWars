namespace TankWars.Runtime.Core.UI.Menus
{
    using TankWars.Runtime.Core.Databases;
    using TankWars.Runtime.Core.ManagerSystem;
    using TankWars.Runtime.Gameplay.Unlockables;
    using UnityEngine;

    public class CollectibleMenu : BaseMenu
    {
        [SerializeField]
        private ShowCollectibleButton showCollectibleButtonPrefab = null;

        [SerializeField]
        private Transform collectibleNameImagePairGroup = null;

        [SerializeField]
        private CollectibleView collectibleView = null;

        private ShowCollectibleButton[] showCollectibleButtons = null;

        private DatabaseManager DatabaseManager => CoreManagers.Instance.GetManager<DatabaseManager>(); 

        public override void TransitionIn()
        {
            CreateCollectibleButtons(); 

            base.TransitionIn();
        }

        public override void TransitionOut()
        {
            DestroyCollectibleButtons(); 

            base.TransitionOut();
        }

        private void CreateCollectibleButtons()
        {
            KeyChainCollectible[] keyChainCollectibles = DatabaseManager.GetDatabase<KeyChainCollectibleDatabase>().KeyChainCollectibles;
            showCollectibleButtons = new ShowCollectibleButton[keyChainCollectibles.Length]; 

            int index = 0; 

            foreach(KeyChainCollectible keyChainCollectible in keyChainCollectibles)
            {
                ShowCollectibleButton showCollectibleButton = Instantiate(showCollectibleButtonPrefab);
                showCollectibleButton.Initialize(keyChainCollectible, collectibleView);
                showCollectibleButton.SetButtonInteractable(keyChainCollectible.IsObjectUnlocked); 
                showCollectibleButton.gameObject.transform.SetParent(collectibleNameImagePairGroup, false);
                showCollectibleButtons[index] = showCollectibleButton;
                index++; 
            }
        }

        private void DestroyCollectibleButtons()
        {
            if(showCollectibleButtons == null)
            {
                return; 
            }

            foreach(ShowCollectibleButton showCollectibleButton in showCollectibleButtons)
            {
                Destroy(showCollectibleButton.gameObject); 
            }

            showCollectibleButtons = null; 
        }
    }
}
