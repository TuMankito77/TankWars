namespace TankWars.Runtime.Core.UI.Menus
{
    using TankWars.Runtime.Core.Events;
    using System;
    using UnityEngine;
    using UnityEngine.UI;
    using TankWars.Runtime.Gameplay.Visuals;

    public class CollectibleView : BaseMenu, IEventListener
    {
        [SerializeField]
        private Text collectibleNameTitle = null; 

        [SerializeField]
        private MeshFilter meshFilterPlaceholder = null;

        [SerializeField]
        private CollectibleStudioView collectibleStudioView = null; 

        #region Unity Methods

        private void Awake()
        {
            EventManager.Instance.Register(this, typeof(UIEvent)); 
        }

        private void OnDestroy()
        {
            EventManager.Instance.Unregister(this, typeof(UIEvent)); 
        }

        #endregion

        public override void TransitionIn()
        {
            base.TransitionIn();
            collectibleStudioView.Show();
        }

        public override void TransitionOut()
        {
            base.TransitionOut();
            collectibleStudioView.Hide();
        }

        public void OnEventReceived(IComparable eventType, object data)
        {
            switch(eventType)
            {
                case UIEvent uiEvent:
                    {
                        HandleUIEvents(uiEvent, data); 
                        break;
                    }

                default:
                    {
                        Debug.LogError($"{this.GetType().Name} - {gameObject.name}: {EventManager.UNHANDLED_EVENT_TYPE_ERROR}"); 
                        break;
                    }
            }
        }

        private void HandleUIEvents(UIEvent uiEvent, object data)
        {
            switch(uiEvent)
            {
                case UIEvent.OnCollectibleSelected:
                    {
                        ShowCollectibleButton showCollectibleButton = data as ShowCollectibleButton;
                        collectibleNameTitle.text = showCollectibleButton.KeyChainCollectibleLinked.Name; 
                        meshFilterPlaceholder.mesh = showCollectibleButton.KeyChainCollectibleLinked.Mesh;
                        collectibleStudioView.UpdateCollectiblePlaceHolder(showCollectibleButton.KeyChainCollectibleLinked);
                        break;
                    }
            }
        }
    }
}
