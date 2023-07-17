namespace TankWars.Runtime.Core.Audio
{
    using UnityEngine; 

    public class AudioRequest 
    {
        public const int INVALID_PERFORMER_ID = -1; 

        public int PerformerId { get; private set; } = INVALID_PERFORMER_ID; 
        public string SoundId { get; private set; } = string.Empty;
        public bool WillPlayOnce { get; private set; } = true;
        public bool IsSpatialSound { get; private set; } = false;
        public Vector3 SoundLocation { get; private set; } = default(Vector3); 
        public VolumeType VolumeType { get; private set; } = VolumeType.SoundEffects; 

        public AudioRequest WithSoundId(string soundId)
        {
            SoundId = soundId;
            return this; 
        }

        public AudioRequest WithWillPlayOnce(bool willPlayOnce)
        {
            WillPlayOnce = willPlayOnce;
            return this; 
        }

        public AudioRequest WithIsSpatialSound(bool isSpatialSound)
        {
            IsSpatialSound = isSpatialSound;
            return this;
        }

        public AudioRequest WithSoundLocation(Vector3 soundLocation)
        {
            SoundLocation = soundLocation;
            return this; 
        }

        public AudioRequest WithPerformerId(int performerId)
        {
            PerformerId = performerId;
            return this; 
        }

        public AudioRequest WithVolumeType(VolumeType volumeType)
        {
            VolumeType = volumeType;
            return this; 
        }
    }
}

