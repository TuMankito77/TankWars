namespace TankWars.Runtime.Core.Databases
{
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    using TankWars.Runtime.Tools;

    [CreateAssetMenu(fileName = "ButtonIdDatabase", menuName = "Database/ButtonIdDatabase")]
    public class ButtonIdDatabase : ScriptableObject
    {
        private const string TEMPLATE_BUTTON_ID_SCRIPT_PATH = "Assets/Scripts/Runtime/Core/DataBases/Templates/TemplateButtonId.txt";
        private const string BUTTON_ID_ENUM_SCRIPT_PATH = "Assets/Scripts/Runtime/Core/UI/Buttons/DataBases/Resources/ButtonId.cs";
        private const string TEMPLATE_BUTTON_ID = "#ButtonId#";
        private const string BUTTON_ID_ENUM_START = "#";
        private const string BUTTON_ID_ENUM_END = ",";

        [SerializeField]
        private List<string> buttonIds = new List<string>();

        private HashSet<string> uniqueButtonIds = new HashSet<string>();

#if UNITY_EDITOR

        #region Unity Methods

        private void OnValidate()
        {
            Regex nonAlphaNumeric = new Regex(@"\W|_");

            for (int i = 0; i < this.buttonIds.Count; i++)
            {
                this.buttonIds[i] = nonAlphaNumeric.Replace(this.buttonIds[i], string.Empty);
            }

            uniqueButtonIds = new HashSet<string>(buttonIds);
        }

        #endregion

        public void GenerateButtonIdEnumFile()
        {
            TextAsset scriptTemplate = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(TEMPLATE_BUTTON_ID_SCRIPT_PATH);
            string templateText = scriptTemplate.text;

            int contextMenuStartIndex = templateText.IndexOf(BUTTON_ID_ENUM_START, StringComparison.InvariantCulture);
            int contextMenuEndIndex = templateText.IndexOf(BUTTON_ID_ENUM_END, contextMenuStartIndex, StringComparison.InvariantCulture) + BUTTON_ID_ENUM_END.Length;

            string customDataConstantTemplate = templateText.Substring(contextMenuStartIndex, contextMenuEndIndex - contextMenuStartIndex);
            StringBuilder customDataConstants = new StringBuilder();

            int currentIndex = 0;

            foreach (string uniqueLevelId in uniqueButtonIds)
            {
                string keyConstant = customDataConstantTemplate.Replace(TEMPLATE_BUTTON_ID, uniqueLevelId.ToConstantFormat());
                customDataConstants.Append(keyConstant);
                customDataConstants.Append(currentIndex == buttonIds.Count - 1 ? string.Empty : "\n\t\t");
                ++currentIndex;
            }

            string contextMenuScript = templateText.Replace(customDataConstantTemplate, customDataConstants.ToString());

            if (File.Exists(BUTTON_ID_ENUM_SCRIPT_PATH))
            {
                File.SetAttributes(BUTTON_ID_ENUM_SCRIPT_PATH, FileAttributes.Normal);
            }

            File.WriteAllText(BUTTON_ID_ENUM_SCRIPT_PATH, contextMenuScript);
            UnityEditor.AssetDatabase.Refresh();
        }

#endif
    }
}
