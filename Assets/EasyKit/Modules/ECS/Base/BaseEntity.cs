using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EasyKit.ECS
{
    [Serializable]
    public class BaseEntity : BaseLifeCycle
    {
        public static int _IdCounter = 0;
        
        public int entityId;

        public int factionID;
        
        public bool isDestroyed;
        
        public Dictionary<Type, BaseComponent> Components { get; private set; }

        public int ComponentCount
        {
            get { return Components == null ? 0 : Components.Count; }
        }
        
        public EntityStaticData StaticData { get; private set; }
        
        private SystemsManager mSystemsManager;
        
        
        public BaseEntity()
        {
            entityId = ++_IdCounter;
        }
        
        public void Setup(EntityStaticData staticData, SystemsManager systemsManager)
        {
            StaticData = staticData;
            Components = new Dictionary<Type, BaseComponent>(4);
            this.mSystemsManager = systemsManager;
        }

        public override void DoStart()
        {
            base.DoStart();
            
            if (IsRunning)
            {
                Debug.LogError("Entity already running!");
                return;
            }
            IsRunning = true;
            
            if(mSystemsManager.EntitySystem != null)
                mSystemsManager.EntitySystem.RegisterEntity(this);
            
            foreach (var kvp in Components)
            {
                var component = kvp.Value;
                if (component.AutoRun)
                    component.DoStart();
            }
        }

        public virtual void DoDestroy()
        {
            if (isDestroyed)
                return;
            
            DoStop();
            
            foreach (var pair in Components)
            {
                pair.Value.DoDestroy();
            }
            
            if(mSystemsManager.EntitySystem != null)
                mSystemsManager.EntitySystem.UnRegisterEntity(this);
            
            isDestroyed = true;
        }

        public virtual void DoStop()
        {
            if (!IsRunning)
                return;
            IsRunning = false;
            
            //停止所有的组件
            foreach (var pair in Components)
            {
                pair.Value.DoStop();
            }
        }
        
        public void ChangeFaction(int faction)
        {
            var oldFaction = factionID;
            factionID = faction;
            var keys = Components.Keys.ToArray();
            foreach (var key in keys)
            {
                Components[key].OnFactionChanged(oldFaction);
            }
        }
        
        public bool HasComponent<T>() where T : BaseComponent
        {
            return Components != null && Components.ContainsKey(typeof(T));
        }

        public bool HasComponent(Type t)
        {
            return Components != null && Components.ContainsKey(t);
        }
        
        
        public T GetComponent<T>() where T : BaseComponent
        {
            return (T)GetComponent(typeof(T));
        }

        public BaseComponent GetComponent(Type type)
        {
            try
            {
                return Components[type];
            }
            catch (Exception e)
            {
                
        #if UNITY_EDITOR
                var txt = new System.Text.StringBuilder();
                foreach (var feature in Components.Values)
                {
                    txt.AppendLine(feature.GetType().ToString());
                }
                string outName = entityId.ToString();
                
                Debug.LogError($"Component of Type {type.ToString()} could not be found. Entity ({outName}) only has:\n{txt}");
        #endif
                return null;
            }
        }
        
        
        public bool AddComponent(BaseComponent component)
        {
            var type = component.GetType();
            if (HasComponent(type))
            {
                Debug.LogError("Feature of type " + ECSHelper.GetTypeName(type) + " is already attached.");
                return false;
            }

            Components.Add(component.GetType(), component);
            component.DoAwake();
            
            if (IsRunning)
            {
                component.DoStart();
            }
            return true;
        }

        public T RemoveComponent<T>() where T : BaseComponent
        {
            var type = typeof(T);
            if (Components != null && Components.ContainsKey(type))
            {
                var component = Components[type];
                if (component.IsRunning)
                    component.DoStop();
                
                Components.Remove(type);
                return (T)component;
            }
            
            Debug.LogError("Component of type " + type.ToString() + " not found.");
            return null;
        }
        
    }
}