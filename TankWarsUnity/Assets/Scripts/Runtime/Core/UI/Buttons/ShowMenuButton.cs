namespace TankWars.Runtime.Core.UI
{
    using TankWars.Runtime.Core.UI.Buttons;
    using TankWars.Runtime.Core.UI.Menus;
    using TankWars.Runtime.Core.Events; 
    using UnityEngine;

    public class ShowMenuButton : CustomButton
    {
        [SerializeField]
        private BaseMenu menu = null;

        public BaseMenu Menu => menu;

        protected override void OnButtonClicked()
        {
            base.OnButtonClicked();
            EventManager.Instance.Dispatch(buttonId, this); 
        }
    }
}
