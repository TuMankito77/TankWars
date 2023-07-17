namespace TankWars.Runtime.Core.ManagerSystem
{
    using UnityEngine;
    using System.Collections.Generic;
    using System;

    public class CoreManagers : MonoSingleton<CoreManagers>
    {
        [SerializeField]
        private BaseManager[] managers = null;
        
        private Dictionary<Type, BaseManager> managersLookup = null;


        #region Unity Methods

        protected override void Init()
        {
            base.Init();
            DontDestroyOnLoad(gameObject);
            
            foreach(BaseManager manager in managers)
            {
                manager.Init();
            }
        }

        private void OnValidate()
        {
            if(managers == null)
            {
                return; 
            }

            managersLookup = new Dictionary<Type, BaseManager>();

            foreach (BaseManager manager in managers)
            {
                if (managersLookup.ContainsKey(manager.GetType()))
                {
                    Debug.LogWarning($"{GetType().Name}-{gameObject.name}: There is more than one manager of the type {manager.GetType()} , only the first one will be added to the lookup dictionary used to access them.");
                    continue;
                }

                managersLookup.Add(manager.GetType(), manager);
            }
        }

        #endregion

        public T GetManager<T>() where T : BaseManager
        {
            if(managersLookup.ContainsKey(typeof(T)))
            {
                return managersLookup[typeof(T)] as T; 
            }

            Debug.LogError($"{GetType()}-{gameObject.name}: The manager {typeof(T)} was not added to the list of managers, please add the prefab to the managers list in this prefab."); 
            return null; 
        }
    }
}
