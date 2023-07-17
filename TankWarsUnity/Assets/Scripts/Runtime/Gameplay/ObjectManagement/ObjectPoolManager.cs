namespace TankWars.Runtime.Gameplay.ObjectManagement
{
    using UnityEngine;
    using System.Collections.Generic;
    using System;
    using TankWars.Runtime.Core.ManagerSystem;

    public class ObjectPoolManager : BaseManager
    {
        [Serializable]
        private struct ObjectPoolContainerInfo
        {
            public int poolSize;
            public PoolObject poolObjectPrefab;
            public Transform objectPoolContainerTransform;
        }

        [SerializeField]
        private List<ObjectPoolContainerInfo> objectPoolContainersInfo = null;

        private Dictionary<Type, ObjectPoolContainer> objectPoolContainers = null;

        public override void Init()
        {
            objectPoolContainers = new Dictionary<Type, ObjectPoolContainer>();
        }

        public void GeneratePoolObjects<T>(Transform poolContainerParentTransform = null) where T : PoolObject
        {
            if (objectPoolContainers.ContainsKey(typeof(T)))
            {
                Debug.Log($"{GetType()}-{gameObject.name}: The pool that you are trying to genarate already exists.");
                return;
            }

            ObjectPoolContainerInfo requestedObjectPoolInfo = default(ObjectPoolContainerInfo); 

            Predicate<ObjectPoolContainerInfo> wasPoolObjectTypeFound = delegate (ObjectPoolContainerInfo objectPoolContainerInfo)
            {
                if (!(objectPoolContainerInfo.poolObjectPrefab.GetType() == typeof(T)))
                {
                    return false; 
                }
                
                requestedObjectPoolInfo = objectPoolContainerInfo;
                return true; 
            };

            if (!objectPoolContainersInfo.Exists(wasPoolObjectTypeFound))
            {
                Debug.LogError($"{GetType()}-{gameObject.name}: The pool of type {typeof(T)} could not be generated due to not being in the list Object Pool Containers Info");
                return;
            }

            GameObject newPoolContainer = new GameObject($"{requestedObjectPoolInfo.poolObjectPrefab.GetType().Name} - Pool"); 
            ObjectPoolContainer objectPoolContainer = new ObjectPoolContainer(requestedObjectPoolInfo.poolSize, requestedObjectPoolInfo.poolObjectPrefab, newPoolContainer.transform);
            objectPoolContainers.Add(requestedObjectPoolInfo.poolObjectPrefab.GetType(), objectPoolContainer);

            if(poolContainerParentTransform)
            {
                newPoolContainer.transform.SetParent(poolContainerParentTransform); 
            }
        }

        public T GetPoolObject<T>() where T : PoolObject
        {
            if (objectPoolContainers.TryGetValue(typeof(T), out ObjectPoolContainer container))
            {
                return container.ActivatePoolObject() as T;
            }

            return null;
        }

        public void DisablePoolObject<T>(T poolObject) where T : PoolObject
        {
            if (objectPoolContainers.TryGetValue(typeof(T), out ObjectPoolContainer container))
            {
                container.DeactivatePoolObject(poolObject);
            }
        }

        public void EmptyObjectPool<T>()
        {
            if (objectPoolContainers.TryGetValue(typeof(T), out ObjectPoolContainer container))
            {
                container.EmptyObjectPoolContainer();
                objectPoolContainers.Remove(typeof(T)); 
            }
        }

        public void RemoveObjectPool<T>()
        {
            if (objectPoolContainers.TryGetValue(typeof(T), out ObjectPoolContainer container))
            {
                container.DisposeObjectPool();
                objectPoolContainers.Remove(typeof(T));
            }
        }
    }
}
