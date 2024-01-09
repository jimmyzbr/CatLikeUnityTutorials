using UnityEngine;

namespace EasyKit.ECS
{
    public class BaseStaticData : ScriptableObject
    {
        public string ID { get { return name; } }
        
        public virtual void Init() { }
    }
}