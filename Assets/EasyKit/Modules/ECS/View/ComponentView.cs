using UnityEngine;

namespace EasyKit.ECS
{
    public abstract class ComponentViewBase : MonoBehaviour,IRunLogic
    {
        public abstract void Setup(BaseComponent component);
        public bool IsRunning { get; protected set; }
        
        public bool IsVisible { get; protected set; }

        public virtual void ShowHide(bool visible)
        {
            IsVisible = visible;
        }
        
        public abstract void OnViewSelected();
        
        public abstract void OnViewUnSelected();

        public virtual void UpdateView()
        {
        }
        
        public virtual void Dispose()
        {
            IsRunning = false;
        }
    }

    public abstract class ComponentView<T> : ComponentViewBase where T : BaseComponent
    {
        public T Component { get; set; }		
        
        public override void Setup(BaseComponent component)
        {
            Component = (T)component;
            OnViewUnSelected();
            IsRunning = true;
        }
        
    }
}