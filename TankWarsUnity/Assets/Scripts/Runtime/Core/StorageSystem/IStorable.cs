namespace TankWars.Runtime.Core.StorageSystem
{
    using System; 

    public interface IStorable
    {
        public string Key { get; } 
        public Type StorableType { get; }
    }
}
