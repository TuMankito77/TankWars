namespace TankWars.Runtime.Core.Databases
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using TankWars.Runtime.Core.ManagerSystem;
    using UnityEngine;

    public class DatabaseManager : BaseManager
    {
        [SerializeField]
        private ScriptableObject[] databases = null;

        private Dictionary<Type, ScriptableObject> databaseLookup = null;

        #region Unity Methods

        private void OnValidate()
        {
            databaseLookup = new Dictionary<Type, ScriptableObject>(); 

            foreach(ScriptableObject database in databases)
            {
                if(!databaseLookup.ContainsKey(database.GetType()))
                {
                    databaseLookup.Add(database.GetType(), database); 
                }
                else
                {
                    Debug.LogError("You are trying to add two databases of the same type, please meke sure that you have only one database per type on the array of this Game Object."); 
                }
            }
        }

        #endregion

        public T GetDatabase<T>() where T:ScriptableObject
        {
            if(databaseLookup.ContainsKey(typeof(T)))
            {
                return databaseLookup[typeof(T)] as T; 
            }
                
            return null; 
        }
    }
}
