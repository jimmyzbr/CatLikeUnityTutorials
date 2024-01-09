namespace EasyKit.ECS
{
    public interface ISaveable
    {
        ISaveable CollectData();
    }
    
    public abstract class IComponentData
    {
    }
    
    public abstract class ComponentParamsBase
    {
    }

    public abstract class ComponentParams<T> : ComponentParamsBase
        where T : BaseComponent
    {

    }
    
    public abstract class BaseComponent : ILifeCycle
    {
        public bool AutoRun { get; set; }
        
        public bool IsRunning { get; private set; }
        
        public BaseEntity entity { get; private set; }
        
        public int FactionID => entity.factionID;
        
        protected bool mIsDestroyed;
        
        public virtual void Setup(BaseEntity entity, ComponentParamsBase args = null)
        {
            this.entity = entity;
        }

        public virtual void DoAwake()
        {
        }

        public virtual void DoStart()
        {
            if (IsRunning) 
                UnityEngine.Debug.LogError("Component already running!");
            IsRunning = true;
        }

        public void DoDestroy()
        {
            mIsDestroyed = true;
            if(IsRunning)
                DoStop();
        }
        
        public virtual void DoStop()
        {
            if (!IsRunning)
                return;
            IsRunning = false;
        }
        
        public void DoUpdate(float deltaTime)
        {
        }
        
        public virtual void OnFactionChanged(int oldFaction) { }

    }

    public abstract class AComponent<T> : BaseComponent where T : IComponentData
    {
        
    }
}