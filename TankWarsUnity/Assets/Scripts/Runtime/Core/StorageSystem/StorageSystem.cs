namespace TankWars.Runtime.Core.StorageSystem
{
    using UnityEngine;
    using Newtonsoft.Json;

    public class StorageAccessor 
    {
        public void Save<T>(T classInformation) where T : IStorable
        {
            string classInformationJson = JsonConvert.SerializeObject(classInformation);
            PlayerPrefs.SetString(classInformation.Key, classInformationJson); 
        }

        public T Load<T>(string key) where T : IStorable
        {
            string classInformationJson = PlayerPrefs.GetString(key);
            T classInformation = JsonConvert.DeserializeObject<T>(classInformationJson);
            return classInformation; 
        }

        public bool DoesInformationExist(string key)
        {
            return PlayerPrefs.HasKey(key); 
        }
    }
}
