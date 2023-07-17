namespace TankWars.Runtime.Core.StorageSystem
{
    using Newtonsoft.Json;
    using System;
    using UnityEngine;

    public class PlayerInformation : IStorable
    {
        [JsonIgnore]
        public const string STORABLE_KEY = "PlayerInformation";

        [JsonProperty]
        private int tankInfoContainerIndex = 0;

        [JsonProperty]
        private float tankColorR = 0;

        [JsonProperty]
        private float tankColorG = 0;

        [JsonProperty]
        private float tankColorB = 0;

        [JsonProperty]
        private float tankColorA = 0; 

        [JsonProperty]
        private int points = 0;

        [JsonIgnore]
        public int TankInfoContaierIndex => tankInfoContainerIndex;

        [JsonIgnore]
        public Color TankColor 
        { 
            get
            {
                return new Color(tankColorR, tankColorG, tankColorB, tankColorA); 
            }
        } 

        [JsonIgnore]
        public int Points => points;

        public PlayerInformation()
        {

        }

        public PlayerInformation(PlayerInformationParameters playerInformationParameters)
        {
            tankInfoContainerIndex = playerInformationParameters.tankIndexSelected;
            tankColorR = playerInformationParameters.tankColorSelected.r;
            tankColorG = playerInformationParameters.tankColorSelected.g;
            tankColorB = playerInformationParameters.tankColorSelected.b;
            tankColorA = playerInformationParameters.tankColorSelected.a;
            points = playerInformationParameters.points;
        }

        #region IStorable

        [JsonIgnore]
        public string Key => STORABLE_KEY;

        [JsonIgnore]
        public Type StorableType => GetType();

        #endregion
    }
}
