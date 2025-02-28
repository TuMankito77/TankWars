namespace TankWars.Editor.Tools
{
    using UnityEngine;
    using UnityEditor;
    
    using TankWars.Runtime.Tools;
    
    [CustomEditor(typeof(TilesGenerator))]
    public class TilesGeneratorCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            TilesGenerator instanceRef = (TilesGenerator)target;

            if (instanceRef == null)
            {
                return;
            }

            if (GUILayout.Button("Generate Grid"))
            {
                instanceRef.GenerateGrid();
            }

            if (GUILayout.Button("Delete Grid"))
            {
                instanceRef.DeleteGrid();
            }
        }
    }
}

