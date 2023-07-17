namespace TankWars.Runtime.Gameplay.ObjectManagement
{
    using UnityEngine; 
    using System.Collections.Generic;

    public class ObjectPoolContainer
    {
        private List<PoolObject> poolObjects = null;
        private Transform poolTransform = null;
        private PoolObject poolObjectPrefab = null;

        public ObjectPoolContainer(int poolSize, PoolObject sourcePoolObjectPrefab, Transform sourcePoolTransform)
        {
            poolObjects = new List<PoolObject>();
            poolTransform = sourcePoolTransform;
            poolObjectPrefab = sourcePoolObjectPrefab; 

            for(int i = 0; i < poolSize; i++)
            {
                PoolObject poolObject = GameObject.Instantiate(poolObjectPrefab);
                poolObjects.Add(poolObject);
                poolObject.transform.parent = poolTransform;
                poolObject.transform.position = poolTransform.position; 
                poolObject.gameObject.SetActive(false); 
            }
        }

        public void EmptyObjectPoolContainer()
        {
            foreach(PoolObject poolObject in poolObjects)
            {
                GameObject.Destroy(poolObject.gameObject); 
            }

            poolObjects.Clear();
        }

        public void DisposeObjectPool()
        {
            if(poolTransform != null)
            {
                GameObject.Destroy(poolTransform.gameObject); 
            }

            poolObjects.Clear(); 
        }

        public PoolObject ActivatePoolObject()
        {
            foreach(PoolObject poolObject in poolObjects)
            {
                if(!poolObject.gameObject.activeSelf)
                {
                    poolObject.gameObject.SetActive(true); 
                    return poolObject; 
                }
            }

            PoolObject newPoolObject = GameObject.Instantiate(poolObjectPrefab);
            poolObjects.Add(newPoolObject);
            newPoolObject.transform.parent = poolTransform;
            return newPoolObject; 
        }

        public void DeactivatePoolObject(PoolObject poolObject)
        {
            if(poolObjects.Contains(poolObject))
            {
                poolObjects.Find(obj => obj == poolObject).gameObject.SetActive(false); 
            }
        }
    }
}
