namespace TankWars.Runtime.Gameplay.StorableClasses
{
    using TankWars.Runtime.Core.Databases;
    using TankWars.Runtime.Core.StorageSystem;
    using System.Collections.Generic;
    using System; 
    using UnityEngine;
    using TankWars.Runtime.Gameplay.Levels;
    using Newtonsoft.Json;

    public class GameInformation : IStorable
    {
        public const string STORABLE_KEY = "GameInformation";

        [JsonProperty]
        private Dictionary<string, bool> isLevelUnlocked = new Dictionary<string, bool>();

        [JsonProperty]
        private GameDifficulty difficulty = GameDifficulty.Easy;

        [JsonProperty]
        private float musicVolume = 1f;

        [JsonProperty]
        private float soundEffectsVolume = 1f;

        [JsonIgnore]
        public GameDifficulty Difficulty => difficulty;

        [JsonIgnore]
        public float MusicVolume => musicVolume;

        [JsonIgnore]
        public float SoundEffectsVolume => soundEffectsVolume;

        #region IStorable

        [JsonIgnore]
        public string Key => STORABLE_KEY;

        [JsonIgnore]
        public Type StorableType => GetType();

        #endregion

        [JsonConstructor]
        public GameInformation()
        {

        }

        public GameInformation(LevelIdDatabase levelIdDatabase)
        {
            foreach(LevelIdDatabase.LevelSceneInfo levelSceneInfo in levelIdDatabase.LevelSceneInfoList)
            {
                isLevelUnlocked.Add(levelSceneInfo.LevelId.ToString(), levelSceneInfo.IsUnlocked); 
            }
        }

        public void UnlockLevel(LevelId levelId)
        {
            if (isLevelUnlocked.ContainsKey(levelId.ToString()))
            {
                isLevelUnlocked[levelId.ToString()] = true;
            }
        }

        public bool IsLevelUnlocked(LevelId levelId)
        {
            if(isLevelUnlocked.TryGetValue(levelId.ToString(), out bool isUnlocked))
            {
                return isUnlocked; 
            }

            Debug.LogError($"The {levelId} level id is not contained inside the isLevelUnlocked dictionary, please add it."); 
            return false; 
        }

        public void ChangeGameDifficulty(GameDifficulty sourceDifficulty)
        {
            difficulty = sourceDifficulty; 
        }

        public void UpdateMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp(volume, 0, 1); 
        }

        public void UpdateSoundEffectsVolume(float volume)
        {
            soundEffectsVolume = Mathf.Clamp(volume, 0, 1);
        }
    }
}
