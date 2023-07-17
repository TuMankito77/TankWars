namespace TankWars.Runtime.Core.UI
{
    using TankWars.Runtime.Core.UI.Buttons;
    using TankWars.Runtime.Core.Events;
    using TankWars.Runtime.Gameplay.Unlockables;
    using UnityEngine;
    using UnityEngine.UI;
    using TankWars.Runtime.Core.UI.Menus;

    public class ShowCollectibleButton : CustomButton
    {
        [SerializeField]
        private Text collectibleName = null;

        [SerializeField]
        private Image collectibleImage = null;

        public KeyChainCollectible KeyChainCollectibleLinked { get; private set; } = null;
        public CollectibleView CollectibleView { get; private set; } = null; 

        public void Initialize(KeyChainCollectible keyChainCollectible, CollectibleView collectibleView)
        {
            CollectibleView = collectibleView; 
            KeyChainCollectibleLinked = keyChainCollectible;
            collectibleName.text = KeyChainCollectibleLinked.Name;
            collectibleImage.sprite = KeyChainCollectibleLinked.Image; 
        }

        protected override void OnButtonClicked()
        {
            base.OnButtonClicked();
            EventManager.Instance.Dispatch(UIEvent.OnCollectibleSelected, this);
        }
    }
}
