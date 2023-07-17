namespace TankWars.Runtime.Core.UI.Menus
{
    using System.Collections;
    using TankWars.Runtime.Core.Tools;
    using UnityEngine;
    using UnityEngine.UI;

    public class LoadingMenu : BaseMenu
    {
        [SerializeField]
        private Slider loadingSlider = null; 

        public IEnumerator StartLoadingBar(AsyncOperationsHandler asyncOperationsHandler)
        {
            loadingSlider.value = 0; 

            while(!asyncOperationsHandler.AreAllOperationsDone())
            {
                loadingSlider.value = asyncOperationsHandler.GetTotalProgress();
                yield return null; 
            }

        }
    }
}
