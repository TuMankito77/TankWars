namespace TankWars.Editor.Tools
{
    using UnityEditor;
    using UnityEngine;
    public class PlayerPrefsHelper
    {
        [MenuItem("TankWars/ClearAllPlayerPrefs")]
        private static void ClearAllPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
