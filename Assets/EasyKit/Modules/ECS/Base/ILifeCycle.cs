using System;

namespace EasyKit.ECS
{
    public interface IUpdate
    {
        void DoUpdate(float deltaTime);
    }

    public interface IAwake
    {
        void DoAwake();
    }

    public interface IStart
    {
        void DoStart();
    }

    public interface IDestroy
    {
        void DoDestroy();
    }

    public interface ILifeCycle : IUpdate, IAwake, IStart, IDestroy
    {
        bool IsRunning { get;}
    }


    [Serializable]
    public class BaseLifeCycle : ILifeCycle
    {
        public virtual bool IsRunning { get; set; }
        
        public virtual void DoAwake()
        {
        }

        public virtual void DoStart()
        {
        }

        public virtual void DoUpdate(float deltaTime)
        {
        }

        public virtual void DoDestroy()
        {
        }
    }
    
    interface IRunLogic
    {
        bool IsRunning { get; }
    }
}