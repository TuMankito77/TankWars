namespace TankWars.Runtime.Core.UI.Menus
{
    using TankWars.Runtime.Core.UI.Buttons;
    using UnityEngine;

    public class BaseMenu : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup menuCanvasGroup = null; 

        public virtual void TransitionIn()
        {
            menuCanvasGroup.alpha = 1;
            menuCanvasGroup.blocksRaycasts = true;
            menuCanvasGroup.interactable = true; 
        }

        public virtual void TransitionOut()
        {
            menuCanvasGroup.alpha = 0;
            menuCanvasGroup.blocksRaycasts = false;
            menuCanvasGroup.interactable = false;
        }

        public virtual void InitializeMenu()
        {

        }
        
        public virtual void TerminateMenu()
        {

        }

        public virtual void SetFirstMenuButtonSelected()
        {
            CustomButton[] menuButtons = GetComponentsInChildren<CustomButton>(); 

            foreach(CustomButton button in menuButtons)
            {
                if(button.IsInteractable)
                {
                    button.SetButtonAsSelected(); 
                }

                break;
            }
        }
    }
}
