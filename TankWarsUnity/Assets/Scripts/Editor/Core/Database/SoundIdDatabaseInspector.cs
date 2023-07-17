namespace TankWars.Editor.Core.Databases
{

#if UNITY_EDITOR
    using TankWars.Runtime.Core.Databases;  
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(SoundIdDatabase))]
    public class SoundIdDatabaseInspector : Editor 
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Regenerate Sound ID class file"))
            {
                SoundIdDatabase soundIdDatabase = Resources.Load<SoundIdDatabase>("SoundIdDatabase");
                soundIdDatabase.GenerateSoundIdClassFile(); 
            }
        }
    }

#endif

}
