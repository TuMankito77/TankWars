namespace TankWars.Editor.Core.Databases
{
    using TankWars.Runtime.Core.Databases;
    using UnityEditor;
    using UnityEngine;

#if UNITY_EDITOR
    [CustomEditor(typeof(ButtonIdDatabase))]
    public class ButtonIdDatabaseInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Regenerate Button Id Enum file"))
            {
                ButtonIdDatabase buttonIdDatabase = Resources.Load<ButtonIdDatabase>("ButtonIdDatabase");
                buttonIdDatabase.GenerateButtonIdEnumFile();
            }
        }
    }
#endif
}