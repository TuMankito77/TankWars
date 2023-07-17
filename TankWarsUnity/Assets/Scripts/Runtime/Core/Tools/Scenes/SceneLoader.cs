namespace TankWars.Runtime.Core.Tools.Scenes
{
    using System;
    using UnityEngine; 
    using UnityEngine.SceneManagement; 

    public class SceneLoader 
    {
        public void LoadScene(string sceneName, out AsyncOperation asyncOperation, Action<AsyncOperation> OnSceneLoaded = null, Action OnSceneLoadedFailure = null)
        {
            AsyncOperation loadSceneAsyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            
            if(loadSceneAsyncOperation == null)
            {
                OnSceneLoadedFailure?.Invoke();
                asyncOperation = null; 
                return; 
            }

            asyncOperation = loadSceneAsyncOperation; 

            if(loadSceneAsyncOperation.isDone)
            {
                OnSceneLoaded?.Invoke(loadSceneAsyncOperation);
                return; 
            }

            loadSceneAsyncOperation.completed += OnSceneLoaded;
        }

        public void UnloadScene(string sceneName, out AsyncOperation asyncOperation, Action<AsyncOperation> OnSceneUnloaded = null, Action OnSceneUnloadedFailre = null)
        {
            AsyncOperation unloadSceneAsyncOperation = SceneManager.UnloadSceneAsync(sceneName, UnloadSceneOptions.None);
            
            if(unloadSceneAsyncOperation == null)
            {
                OnSceneUnloadedFailre?.Invoke();
                asyncOperation = null; 
                return; 
            }

            asyncOperation = unloadSceneAsyncOperation; 

            if(unloadSceneAsyncOperation.isDone)
            {
                OnSceneUnloaded?.Invoke(unloadSceneAsyncOperation);
                return; 
            }

            unloadSceneAsyncOperation.completed += OnSceneUnloaded; 
        }
    }
}
