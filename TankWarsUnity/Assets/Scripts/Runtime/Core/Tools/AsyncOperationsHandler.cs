namespace TankWars.Runtime.Core.Tools
{
    using System; 
    using UnityEngine; 

    public class AsyncOperationsHandler : IDisposable
    {
        public AsyncOperation[] asyncOperations = null; 

        public AsyncOperationsHandler(params AsyncOperation[] sourceAsyncOperations)
        {
            asyncOperations = new AsyncOperation[sourceAsyncOperations.Length];

            int index = 0; 
            
            foreach(AsyncOperation asyncOperation in sourceAsyncOperations)
            {
                asyncOperations[index] = asyncOperation;
                index++; 
            }
        }

        public float GetTotalProgress()
        {
            float totalProgress = 0; 

            foreach(AsyncOperation asyncOperation in asyncOperations)
            {
                totalProgress += asyncOperation.progress; 
            }

            return totalProgress / asyncOperations.Length; 
        }

        public bool AreAllOperationsDone()
        {
            foreach(AsyncOperation asyncOperation in asyncOperations)
            {
                if(!asyncOperation.isDone)
                {
                    return false; 
                }
            }

            return true; 
        }

        public void Dispose()
        {
            asyncOperations = null; 
        }
    }
}
