namespace TankWars.Runtime.Core.Databases
{
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.SceneManagement; 
    using System;
    using TankWars.Runtime.Tools;
    using TankWars.Runtime.Gameplay.Levels;

    [CreateAssetMenu(fileName = "LevelIdDatabase", menuName = "Database/LevelIdDatabase")]
    public class LevelIdDatabase : ScriptableObject
    {
        private const string TEMPLATE_LEVEL_ID_SCRIPT_PATH = "Assets/Scripts/Runtime/Core/DataBases/Templates/TemplateLevelId.txt";
        private const string LEVEL_ID_ENUM_SCRIPT_PATH = "Assets/Scripts/Runtime/Gameplay/Levels/LevelId.cs";
        private const string TEMPLATE_LEVEL_ID = "#LevelId#";
        private const string LEVEL_ID_ENUM_START = "#";
        private const string LEVEL_ID_ENUM_END = ",";

        [Serializable]
        public class LevelSceneInfo
        {
            [SerializeField]
            private LevelId levelId = 0;

            [SerializeField]
            private SceneAsset scene = null;

            [SerializeField]
            private LevelConfiguration levelConfiguration = null;
            
            [SerializeField]
            private bool isUnlocked = false;

            public LevelId LevelId => levelId;
            public SceneAsset Scene => scene;
            public bool IsUnlocked => isUnlocked;
            public LevelConfiguration LevelConfiguration => levelConfiguration; 
        }

        [SerializeField]
        private List<string> levelIds = new List<string>();

        [SerializeField]
        private List<LevelSceneInfo> levelSceneInfo = new List<LevelSceneInfo>(); 

        private HashSet<string> uniqueLevelIds = new HashSet<string>();

        public List<LevelSceneInfo> LevelSceneInfoList => levelSceneInfo;

#if UNITY_EDITOR

        #region Unity Methods

        private void OnValidate()
        {
            Regex nonAlphaNumeric = new Regex(@"\W|_");

            for (int i = 0; i < this.levelIds.Count; i++)
            {
                this.levelIds[i] = nonAlphaNumeric.Replace(this.levelIds[i], string.Empty);
            }

            uniqueLevelIds = new HashSet<string>(levelIds);
        }

        #endregion

        public void GenerateLevelIdEnumFile()
        {
            TextAsset scriptTemplate = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(TEMPLATE_LEVEL_ID_SCRIPT_PATH);
            string templateText = scriptTemplate.text;

            int contextMenuStartIndex = templateText.IndexOf(LEVEL_ID_ENUM_START, StringComparison.InvariantCulture);
            int contextMenuEndIndex = templateText.IndexOf(LEVEL_ID_ENUM_END, contextMenuStartIndex, StringComparison.InvariantCulture) + LEVEL_ID_ENUM_END.Length;

            string customDataConstantTemplate = templateText.Substring(contextMenuStartIndex, contextMenuEndIndex - contextMenuStartIndex);
            StringBuilder customDataConstants = new StringBuilder();

            int currentIndex = 0;

            foreach (string uniqueLevelId in uniqueLevelIds)
            {
                string keyConstant = customDataConstantTemplate.Replace(TEMPLATE_LEVEL_ID, uniqueLevelId.ToConstantFormat());
                customDataConstants.Append(keyConstant);
                customDataConstants.Append(currentIndex == levelIds.Count - 1 ? string.Empty : "\n\t\t");
                ++currentIndex;
            }

            string contextMenuScript = templateText.Replace(customDataConstantTemplate, customDataConstants.ToString());

            if (File.Exists(LEVEL_ID_ENUM_SCRIPT_PATH))
            {
                File.SetAttributes(LEVEL_ID_ENUM_SCRIPT_PATH, FileAttributes.Normal);
            }

            File.WriteAllText(LEVEL_ID_ENUM_SCRIPT_PATH, contextMenuScript);
            UnityEditor.AssetDatabase.Refresh();
        }

#endif

        public string GetSceneName(LevelId levelId)
        {
            return levelSceneInfo.Find(levelSceneInfo => levelSceneInfo.LevelId == levelId).Scene.name;
        }

        public Scene GetScene(LevelId levelId)
        {
            SceneAsset sceneAsset = levelSceneInfo.Find(levelSceneInfo => levelSceneInfo.LevelId == levelId).Scene; 

            if (sceneAsset == null)
            {
                Debug.LogError($"The level id {levelId} is not linked to any scene on the LevelIdDatabase, please make sure that you added a new item to the list that has {levelId} as the levelId and that a scene asset has been assigned to that item on the scriptable object.");
                Debug.LogWarning("Returning the default object of the Scene class");
                return default(Scene);
            }

            string sceneName = levelSceneInfo.Find(levelSceneInfo => levelSceneInfo.LevelId == levelId).Scene.name;
            
            return SceneManager.GetSceneByName(sceneName); 
        }

        public LevelConfiguration GetLevelConfiguration(LevelId levelId)
        {
            return levelSceneInfo.Find(levelSceneInfo => levelSceneInfo.LevelId == levelId).LevelConfiguration;
        }
    }
}
