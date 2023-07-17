namespace TankWars.Editor.Core.Databases
{
    using TankWars.Runtime.Core.Databases;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor; 

#if UNITY_EDITOR

    [CustomEditor(typeof(EnemyTypeDatabase))]
    public class EnemyTypeDatabaseInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Regenerate Enemy Type Enum file"))
            {
                EnemyTypeDatabase enemyTypeDatabase = Resources.Load<EnemyTypeDatabase>("EnemyTypeDatabase");
                enemyTypeDatabase.GenerateEnemyTypeEnumFile();
            }
        }
    }

#endif
}
