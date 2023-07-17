namespace TankWars.Editor.UI
{
    using TankWars.Runtime.Core.UI; 
    using TankWars.Runtime.Core.UI.Menus;
    using UnityEditor;

    [CustomEditor(typeof(UIManager))]
    public class UIManagerInspector : Editor
    {
        private string[] menuNames = null;
        private int menuNameSelectedIndex = 0;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            UIManager uiManagerInstance = target as UIManager;

            if (uiManagerInstance.Menus == null)
            {
                return;
            }

            menuNames = new string[uiManagerInstance.Menus.Count];
            int index = 0;

            foreach (BaseMenu menu in uiManagerInstance.Menus)
            {
                menuNames[index] = menu.gameObject.name;
                index++;
            }

            menuNameSelectedIndex = EditorGUILayout.Popup("First shown menu", uiManagerInstance.currentMenuOpenIndex, menuNames);
            
            if(uiManagerInstance.currentMenuOpenIndex == menuNameSelectedIndex)
            {
                return; 
            }

            uiManagerInstance.currentMenuOpenIndex = menuNameSelectedIndex;
            uiManagerInstance.ShowCurrentMenu(); 
            EditorUtility.SetDirty(target);
        }
    }
}
