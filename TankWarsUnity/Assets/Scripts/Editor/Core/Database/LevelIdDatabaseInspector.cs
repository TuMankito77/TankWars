namespace TankWars.Editor.Core.Databases
{
    using TankWars.Runtime.Core.Databases; 
    using UnityEditor;
    using UnityEngine;

#if UNITY_EDITOR
    [CustomEditor(typeof(LevelIdDatabase))]
    public class LevelIdDatabaseInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Regenerate Level Id Enum file"))
            {
                LevelIdDatabase levelIdDatabase = Resources.Load<LevelIdDatabase>("LevelIdDatabase");
                levelIdDatabase.GenerateLevelIdEnumFile(); 
            }
        }
    }
#endif
}