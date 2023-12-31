namespace TankWars.Runtime.Core
{
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        private static T instance = null;
        
        public static T Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new T(); 
                }

                return instance; 
            }
        }
    }
}

