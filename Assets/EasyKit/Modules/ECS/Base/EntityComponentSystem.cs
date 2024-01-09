using System;
using System.Collections.Generic;

namespace EasyKit.ECS
{
    public abstract class EntityComponentSystem<T> : BaseSystem where T: BaseComponent
    {
        public List<T> Components;
        public List<List<T>> ComponentsByFaction;
        
        public Action OnComponentAdded;
        public Action OnComponentRemoved;

        public EntityComponentSystem(int factions) : base()
        {
            ComponentsByFaction = new List<List<T>>(factions);
            ComponentsByFaction.Add(new List<T>(32));
            Components = new List<T>(16);

            for (int i = 1; i < factions; i++)
            {
                ComponentsByFaction.Add(new List<T>(8));
            }
        }

        public virtual void RegisterComponent(T component)
        {
            if (Components.Contains(component))
            {
                UnityEngine.Debug.LogError(typeof(T).ToString() + " already registered.");
            }
            else
            {				
                Components.Add(component);
                ComponentsByFaction[component.FactionID].Add(component);
            }
        }
        
        public virtual void UnRegisterComponent(T component, int faction = -1)
        {
            if (faction < 0) 
                faction = component.FactionID;

            ComponentsByFaction[faction].Remove(component);
            Components.Remove(component);
            
        }

        public virtual void UpdateFaction(T component)
        {
            for (int i = 0; i < ComponentsByFaction.Count; i++)
            {
                if (i == component.FactionID)
                {
                    if (!ComponentsByFaction[i].Contains(component)) 
                        ComponentsByFaction[i].Add(component);
                }
                else if (ComponentsByFaction[i].Contains(component))
                    ComponentsByFaction[i].Remove(component);
            }
        }
    }
}