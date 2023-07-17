namespace TankWars.Runtime.Core.Databases
{
    using TankWars.Runtime.Tools;
    using TankWars.Runtime.Gameplay.Enemy; 
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using UnityEngine;

    [CreateAssetMenu(fileName = "EnemyTypeDatabase", menuName = "Database/EnemyTypeDataBase")]
    public class EnemyTypeDatabase : ScriptableObject
    {
        private const string TEMPLATE_ENEMY_TYPE_SCRIPT_PATH = "Assets/Scripts/Runtime/Core/DataBases/Templates/TemplateEnemyType.txt";
        private const string ENEMY_TYPE_ENUM_SCRIPT_PATH = "Assets/Scripts/Runtime/Gameplay/Enemy/EnemyType.cs";
        private const string TEMPLATE_ENEMY_TYPE = "#EnemyType#";
        private const string ENEMY_TYPE_ENUM_START = "#";
        private const string ENEMY_TYPE_ENUM_END = ",";

        [Serializable]
        public struct EnemyTypeSettings
        {
            [SerializeField]
            public EnemyType enemyType;

            [SerializeField, Min(5f)]
            public float aimingSpeed;

            [SerializeField, Min(1f)]
            public float movementSpeed;

            [SerializeField, Min(30)]
            public int healthAmount;

            [SerializeField, Min(1)]
            public int bulletsPerShot;
        }

        [SerializeField]
        private List<string> enemyTypes = null;

        [SerializeField]
        private List<EnemyTypeSettings> enemyTypeSettings = null;

        private HashSet<string> uniqueEnemyTypes = new HashSet<string>();

        public List<EnemyTypeSettings> EnemyTypeSettingsList => enemyTypeSettings;

#if UNITY_EDITOR

        #region Unity Methods

        private void OnValidate()
        {
            Regex nonAlphaNumeric = new Regex(@"\W|_");

            for (int i = 0; i < this.enemyTypes.Count; i++)
            {
                this.enemyTypes[i] = nonAlphaNumeric.Replace(this.enemyTypes[i], string.Empty);
            }

            uniqueEnemyTypes = new HashSet<string>(enemyTypes);
        }

        #endregion

        public void GenerateEnemyTypeEnumFile()
        {
            TextAsset scriptTemplate = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(TEMPLATE_ENEMY_TYPE_SCRIPT_PATH);
            string templateText = scriptTemplate.text;

            int contextMenuStartIndex = templateText.IndexOf(ENEMY_TYPE_ENUM_START, StringComparison.InvariantCulture);
            int contextMenuEndIndex = templateText.IndexOf(ENEMY_TYPE_ENUM_END, contextMenuStartIndex, StringComparison.InvariantCulture) + ENEMY_TYPE_ENUM_END.Length;

            string customDataConstantTemplate = templateText.Substring(contextMenuStartIndex, contextMenuEndIndex - contextMenuStartIndex);
            StringBuilder customDataConstants = new StringBuilder();

            int currentIndex = 0;

            foreach (string uniqueEnemyType in uniqueEnemyTypes)
            {
                string keyConstant = customDataConstantTemplate.Replace(TEMPLATE_ENEMY_TYPE, uniqueEnemyType.ToConstantFormat());
                customDataConstants.Append(keyConstant);
                customDataConstants.Append(currentIndex == uniqueEnemyTypes.Count - 1 ? string.Empty : "\n\t\t");
                ++currentIndex;
            }

            string contextMenuScript = templateText.Replace(customDataConstantTemplate, customDataConstants.ToString());

            if (File.Exists(ENEMY_TYPE_ENUM_SCRIPT_PATH))
            {
                File.SetAttributes(ENEMY_TYPE_ENUM_SCRIPT_PATH, FileAttributes.Normal);
            }

            File.WriteAllText(ENEMY_TYPE_ENUM_SCRIPT_PATH, contextMenuScript);
            UnityEditor.AssetDatabase.Refresh();
        }

#endif

        public EnemyTypeSettings GetEnemyTypeSettings(EnemyType enemyType)
        {
            return enemyTypeSettings.Find(enemyTypeSettings => enemyTypeSettings.enemyType == enemyType);
        }
    }
}
