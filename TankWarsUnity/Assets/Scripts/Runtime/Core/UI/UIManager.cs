namespace TankWars.Runtime.Core.UI
{
    using TankWars.Runtime.Core.UI.Menus;
    using TankWars.Runtime.Gameplay.Levels; 
    using TankWars.Runtime.Core.Events;
    using TankWars.Runtime.Gameplay;
    using TankWars.Runtime.Gameplay.Visuals;
    using TankWars.Runtime.Core.Tools;
    using TankWars.Runtime.Core.UI.Buttons;
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    public class UIManager : MonoBehaviour, IEventListener
    {
        [HideInInspector]
        public int currentMenuOpenIndex = 0; 
        
        [SerializeField]
        private List<BaseMenu> menus = null;

        public List<BaseMenu> Menus => menus; 

        private BaseMenu currentMenuOpen = null;
        
        #region Unity Methods

        private void Start()
        {
            ShowCurrentMenu();
            EventManager.Instance.Register(this, typeof(ButtonId), typeof(UIEvent), typeof(GameplayEvent));
        }

        private void OnDestroy()
        {
            EventManager.Instance.Unregister(this, typeof(ButtonId), typeof(UIEvent), typeof(GameplayEvent));
        }

        #endregion

        public void ShowCurrentMenu()
        {
            foreach (BaseMenu menu in menus)
            {
                menu.TransitionOut();
            }

            currentMenuOpen = menus[currentMenuOpenIndex];
            currentMenuOpen.TransitionIn();

            if(Application.isPlaying)
            {
                currentMenuOpen.InitializeMenu(); 
            }
        }

        private T GetMenu<T>() where T:BaseMenu
        {
            return menus.Find(menu => menu.GetType() == typeof(T)) as T;
        }

        #region IEventListener

        public void OnEventReceived(IComparable eventType, object data)
        {
            switch(eventType)
            {
                case ButtonId buttonId:
                    {
                        HandleButtonIdEvents(buttonId, data);
                        break; 
                    }

                case UIEvent uiEvent:
                    {
                        HandleUIEvents(uiEvent, data); 
                        break;
                    }

                case GameplayEvent gameplayEvent:
                    {
                        HandleGameplayEvents(gameplayEvent, data);
                        break; 
                    }

                default:
                    {
                        Debug.Log($"{GetType().Name}-{gameObject.name}: {EventManager.UNHANDLED_EVENT_TYPE_ERROR}");
                        break;
                    }
            }
        }

        private void HandleButtonIdEvents(ButtonId buttonId, object data)
        {
            switch (buttonId)
            {
                case ButtonId.SHOW_MENU:
                    {
                        currentMenuOpen.TransitionOut();
                        ShowMenuButton showMenuButton = (ShowMenuButton)data;
                        currentMenuOpen = showMenuButton.Menu;
                        currentMenuOpen.InitializeMenu(); 
                        currentMenuOpen.TransitionIn();
                        break;
                    }

                default:
                    {
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
                        currentMenuOpen.TransitionOut();
                        ShowCollectibleButton showCollectibleButton = (ShowCollectibleButton)data;
                        currentMenuOpen = showCollectibleButton.CollectibleView;
                        currentMenuOpen.InitializeMenu(); 
                        currentMenuOpen.TransitionIn();
                        break; 
                    }

                default:
                    {
                        break; 
                    }
            }
        }

        private void HandleGameplayEvents(GameplayEvent gameplayEvent, object data)
        {
            switch(gameplayEvent)
            {
                case GameplayEvent.OnLoadingOperationStarted:
                    {
                        LoadingMenu loadingMenu = GetMenu<LoadingMenu>(); 

                        if(loadingMenu == null)
                        {
                            Debug.LogError($"{gameObject.name} - {GetType()}: The loading menu has not been added to the list of menus, please make sure that you have created one and it's part of the list of menus.");
                            return; 
                        }

                        currentMenuOpen.TransitionOut();
                        currentMenuOpen = loadingMenu;
                        currentMenuOpen.InitializeMenu(); 
                        currentMenuOpen.TransitionIn();

                        AsyncOperationsHandler loadingLevelAsyncOperation = data as AsyncOperationsHandler; 
                        StartCoroutine(loadingMenu.StartLoadingBar(loadingLevelAsyncOperation));

                        break;
                    }
                case GameplayEvent.OnLoadingOperationFished:
                    {
                        LevelId currentLevel = (LevelId)data; 
                        currentMenuOpen.TransitionOut();
                        //We include the BaseMenu cast since the ? operator can only return two values that are of the same type. 
                        //Here, eventhough both classes inherit from BaseMenu, the compiler asks for the same type and does not accect polymorphism as an option. 
                        currentMenuOpen = currentLevel != LevelId.NONE ? (BaseMenu)GetMenu<HeadsUpDisplay>() : (BaseMenu)GetMenu<MainMenu>();
                        currentMenuOpen.InitializeMenu(); 
                        currentMenuOpen.TransitionIn(); 
                        break; 
                    }
            }
        }

        #endregion
    }
}
