using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectManagerDemo
{
    /// <summary>
    /// 可持久化对象，负责从文件中保存和加载对象状态
    /// </summary>
    [DisallowMultipleComponent]
    public class PersistableObject : MonoBehaviour
    {
        //保存transform信息
        public virtual void Save(GameDataWriter writer)
        {
            writer.WriteVec3(transform.localPosition); 
            writer.WriteQuat(transform.localRotation);
            writer.WriteVec3(transform.localScale);
        }

        //加载transform信息
        public virtual void Load(GameDataReader reader)
        {
            transform.localPosition = reader.ReadVec3();
            transform.localRotation = reader.ReadQuat();
            transform.localScale = reader.ReadVec3();
        }
        
    }

}

