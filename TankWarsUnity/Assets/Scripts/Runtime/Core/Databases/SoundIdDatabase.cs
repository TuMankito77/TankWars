namespace TankWars.Runtime.Core.Databases
{
    using System;
    using System.IO;
    using System.Text; 
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using TankWars.Runtime.Tools;
    using UnityEngine;

    [CreateAssetMenu(fileName = SOUND_ID_ASSET_NAME, menuName = "Database/SoundIdDatabase")]
    public class SoundIdDatabase : ScriptableObject
    {
        public const string SOUND_ID_ASSET_NAME = "SoundIDDatabase"; 
        
        private const string TEMPLATE_SOUND_ID_SCRIPT_PATH = "Assets/Scripts/Runtime/Core/DataBases/Templates/TemplateSoundID.txt";
        private const string SOUND_ID_CLASS_SCRIPT_PATH = "Assets/Scripts/Runtime/Core/Audio/SoundID.cs";
        private const string TEMPLATE_SOUND_ID = "#SoundId#";
        private const string SOUND_ID_VARIABLE_START = "#";
        private const string SOUND_ID_LINE_END = ";";

        [Serializable]
        private class SoundIdAudioFilePair
        {
            [SerializeField]
            private string soundId = string.Empty;

            [SerializeField]
            private AudioClip audioFile = null;

            public string SoundId => soundId;
            public AudioClip AudioClip => audioFile; 
        }

        [SerializeField]
        private SoundIdAudioFilePair[] soundIdAudioFilePairs = null;

        private HashSet<string> uniqueSoundIds = null;

        public List<string> SoundIds
        {
            get
            {
                if(uniqueSoundIds == null)
                {
                    return new List<string>(); 
                }

                return new List<string>(uniqueSoundIds); 
            } 
        }

#if UNITY_EDITOR

        #region Unity Methods

        private void OnValidate()
        {
            Debug.Assert(AreAudioIdsUnique(), $"{GetType().Name}:There are duplicate audio ids, please make sure to put unique ids on the list.");
            uniqueSoundIds = new HashSet<string>(GetAudioIds(soundIdAudioFilePairs)); 
        }

        #endregion

        private bool AreAudioIdsUnique()
        {
            if(soundIdAudioFilePairs == null)
            {
                return true; 
            }

            for (int i = 0; i < soundIdAudioFilePairs.Length - 1; i++)
            {
                for(int j = i + 1; j < soundIdAudioFilePairs.Length; j++)
                {
                    if (soundIdAudioFilePairs[i].SoundId.Equals(soundIdAudioFilePairs[j].SoundId))
                    {
                        return false; 
                    }
                }
            }

            return true; 
        }

        private string[] GetAudioIds(SoundIdAudioFilePair[] elementIdAudioFilePairs)
        {
            Regex nonAlphaNumeric = new Regex(@"\W|_"); 

            string[] audioIds = new string[elementIdAudioFilePairs.Length]; 

            for(int i = 0; i < elementIdAudioFilePairs.Length; i++)
            {
                audioIds[i] = nonAlphaNumeric.Replace(elementIdAudioFilePairs[i].SoundId, string.Empty); 
            }

            return audioIds; 
        }

        public void GenerateSoundIdClassFile()
        {
            TextAsset scriptTemplate = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(TEMPLATE_SOUND_ID_SCRIPT_PATH);
            string templateText = scriptTemplate.text;

            int contextMenuStartIndex = templateText.IndexOf(SOUND_ID_VARIABLE_START, StringComparison.InvariantCulture);
            int contextMenuEndIndex = templateText.IndexOf(SOUND_ID_LINE_END, contextMenuStartIndex, StringComparison.InvariantCulture) + SOUND_ID_LINE_END.Length;

            string customDataConstantTemplate = templateText.Substring(contextMenuStartIndex, contextMenuEndIndex - contextMenuStartIndex);
            StringBuilder customDataConstants = new StringBuilder();

            int currentIndex = 0;

            foreach (string uniqueSoundId in uniqueSoundIds)
            {
                string soundIdVariableDeclaration = $"public const string {uniqueSoundId.ToConstantFormat()} = \"{uniqueSoundId}\"";
                string keyConstant = customDataConstantTemplate.Replace(TEMPLATE_SOUND_ID, soundIdVariableDeclaration);
                customDataConstants.Append(keyConstant);
                customDataConstants.Append(currentIndex == uniqueSoundIds.Count - 1 ? string.Empty : "\n\t\t");
                ++currentIndex;
            }

            string contextMenuScript = templateText.Replace(customDataConstantTemplate, customDataConstants.ToString());

            if (File.Exists(SOUND_ID_CLASS_SCRIPT_PATH))
            {
                File.SetAttributes(SOUND_ID_CLASS_SCRIPT_PATH, FileAttributes.Normal);
            }

            File.WriteAllText(SOUND_ID_CLASS_SCRIPT_PATH, contextMenuScript);
            UnityEditor.AssetDatabase.Refresh();
        }

#endif

        public AudioClip GetAudioClip(string soundId)
        {
            foreach(SoundIdAudioFilePair soundIdAudioFilePair in soundIdAudioFilePairs)
            {
                if (soundId == soundIdAudioFilePair.SoundId)
                {
                    return soundIdAudioFilePair.AudioClip; 
                }
            }

            return null; 
        }
    }

}
