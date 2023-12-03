namespace EasyKit
{
    /// <summary>
    /// 可回收对象
    /// </summary>
    public interface IPooledObject
    {
        bool Init(object args = null);

        void OnDispose();
        
        void Spawning();

        void DeSpawning();
    }
}
