namespace TankWars.Runtime.Gameplay.Levels
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "LevelSettings", menuName = "TankWars/LevelSettings")]
    public class LevelConfiguration : ScriptableObject
    {
        [SerializeField]
        private LightingSettings lightingSettings = null;

        [SerializeField]
        private Material skyboxMaterial = null; 

        public string BackgroundMusicId { get; set; } = string.Empty;
        public LightingSettings LightingSettings => lightingSettings; 
    }
}
