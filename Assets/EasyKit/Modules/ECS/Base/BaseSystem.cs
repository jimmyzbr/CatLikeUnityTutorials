namespace EasyKit.ECS
{
    public abstract class BaseSystem : ILifeCycle
    {
        public string SystemName { get; protected set; }
        
        public bool IsRunning { get; protected set; }
        

        public BaseSystem()
        {
            SystemName = GetType().ToString();
        }
        
        public abstract void DoUpdate(float deltaTime);

        public abstract void DoAwake();

        public abstract void DoStart();
        
        public abstract void DoDestroy();
        

    }
}