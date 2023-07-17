namespace TankWars.Runtime.Core.UI.Buttons
{
    using System;
    using TankWars.Runtime.Core.Databases;
    using UnityEngine;
    using UnityEngine.UI;

    public class CustomButton : MonoBehaviour
    {
        public event Action onButtonPressed = null; 

        [SerializeField]
        protected ButtonId buttonId = ButtonId.NONE;

        private Button buttonComponent = null; 

        #region Unity Methods

        protected virtual void Awake()
        {
            buttonComponent = GetComponent<Button>();
            buttonComponent.onClick.AddListener(OnButtonClicked); 
        }

        protected virtual void OnDestroy()
        {
            buttonComponent.onClick.RemoveListener(OnButtonClicked); 
        }

        #endregion

        protected virtual void OnButtonClicked()
        {
            onButtonPressed?.Invoke(); 
        }

        public void SetButtonInteractable(bool isInteractable)
        {
            buttonComponent.interactable = isInteractable; 
        }
    }
}
