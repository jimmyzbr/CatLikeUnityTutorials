using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyKit.ECS
{
    [Serializable]
    public class EntityStaticData : BaseStaticData
    {
        [HideInInspector]
        public List<ComponentStaticDataBase> ComponentStaticData;
        
        public T GetComponentData<T>() where T : ComponentStaticDataBase
        {
            if (ComponentStaticData == null || ComponentStaticData.Count <= 0)
                return default(T);
            
            foreach (var componentData in ComponentStaticData)
            {
                if (componentData.GetType() == typeof(T))
                {
                    return (T)componentData;
                }
            }
            return default(T);
        }

        public int GetComponentDataID<T>() where T : ComponentStaticDataBase
        {
            for (int i = 0; i < ComponentStaticData.Count; i++)
            {
                if (ComponentStaticData[i] is T)
                    return i;
            }
            return -1;
        }

        public bool HasComponentData(Type type)
        {
            if (ComponentStaticData == null || ComponentStaticData.Count <= 0)
                return false;
            
            foreach (var feature in ComponentStaticData)
            {
                if (feature.GetType() == type)
                {
                    return true;
                }
            }
            return false;
        }
    }
}