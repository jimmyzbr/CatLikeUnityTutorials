using System;
using UnityEngine;

namespace EasyKit.ECS
{
    [Serializable]
    public abstract class ComponentStaticDataBase  : ScriptableObject
    {
        [HideInInspector]
        public EntityStaticData entity;
        
        public bool autoRun = true;

        public abstract BaseComponent GetRuntimeComponent();

        public virtual void Init() { }

        public virtual void OnStaticDataChanged()
        {
        }
    }
    
    [Serializable]
    public abstract  class ComponentStaticData<T> : ComponentStaticDataBase
        where T : BaseComponent,new()
    {
        public sealed override BaseComponent GetRuntimeComponent()
        {
            return new T();
        }
    }
}