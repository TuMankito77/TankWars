namespace TankWars.Editor.Gameplay.Levels
{
    using System.Collections.Generic;
    using TankWars.Runtime.Core.Databases;
    using TankWars.Runtime.Gameplay;
    using TankWars.Runtime.Gameplay.Levels;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(LevelConfiguration))]
    public class LevelConfigurationInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            //NOTE: We are using Resources.Load() instead of the CoreManagers Singleton due to a limitation 
            //in the MonoSingleton class which is that objects deriving from this class can only be accessed 
            //when the game is running. 
            SoundIdDatabase soundIdDatabase = Resources.Load<SoundIdDatabase>(SoundIdDatabase.SOUND_ID_ASSET_NAME);

            if (soundIdDatabase.SoundIds.Count <= 0)
            {
                return;
            }

            List<string> soundIds = soundIdDatabase.SoundIds;

            LevelConfiguration levelConfigurationInstance = target as LevelConfiguration;

            int soundIdSelectedIndex = 0;

            if (soundIds.Contains(levelConfigurationInstance.BackgroundMusicId))
            {
                int currentIndex = 0;

                foreach (string soundId in soundIds)
                {
                    if (soundId == levelConfigurationInstance.BackgroundMusicId)
                    {
                        soundIdSelectedIndex = currentIndex;
                        break;
                    }

                    currentIndex++;
                }
            }

            soundIdSelectedIndex = EditorGUILayout.Popup("Level Music Sond ID", soundIdSelectedIndex, soundIds.ToArray());

            if (levelConfigurationInstance.BackgroundMusicId == soundIds[soundIdSelectedIndex])
            {
                return;
            }

            levelConfigurationInstance.BackgroundMusicId = soundIds[soundIdSelectedIndex];
            EditorUtility.SetDirty(target);
        }
    }
}
