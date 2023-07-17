namespace TankWars.Runtime.Core
{
    using UnityEngine;

    /// <summary>
    /// Derive from this class if you are going to attach the script to a GameObject
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>  
    {
        private static T instance = null; 

        public static T Instance
        {
            get
            {
                if(instance == null)
                {
                    Debug.LogError($"{typeof(T).Name} singleton is null!");
                }

                return instance; 
            }
        }

        private void Awake()
        {
            if(instance != null && instance != this)
            {
                Destroy(gameObject); 
                return; 
            }

            instance = this as T;
            Init();
        }

        /// <summary>
        /// Use this method to call all the commands that you would call on the Awake method.
        /// </summary>
        protected virtual void Init()
        {

        }
    }
}
