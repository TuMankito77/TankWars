namespace TankWars.Runtime.Core.UI.Buttons
{
    using TankWars.Runtime.Core.Events;
    using UnityEngine;
    using UnityEngine.UI;

    public class TankColorButton : CustomButton
    {
        [SerializeField]
        private Image colorImage = null;

        public Color color
        {
            get
            {
                return colorImage.color; 
            }
        }

        protected override void OnButtonClicked()
        {
            base.OnButtonClicked();
            EventManager.Instance.Dispatch(buttonId, color);
        }
    }
}
