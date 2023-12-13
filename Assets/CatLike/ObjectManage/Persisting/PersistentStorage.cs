using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ObjectManagerDemo
{
    /// <summary>
    ///  持久化存储对象,负责保存一个PersistableObject状态数据到文件中
    /// </summary>
    public class PersistentStorage : MonoBehaviour
    {
        // Start is called before the first frame update
        //数据保存的路径
        public string savePath;

        private void Awake()
        {
            savePath = Path.Combine(Application.persistentDataPath, "saveData.dat");
        }
    
        //保存PersistableObject对象到文件中
        public void Save(PersistableObject obj)
        {
            using (var writer = new BinaryWriter(File.Open(savePath,FileMode.Create)))
            {
                obj.Save(new GameDataWriter(writer));
            }
        }
        
        /// <summary>
        /// 从文件中加载PersistableObject对象信息
        /// </summary>
        /// <param name="obj"></param>
        public void Load(PersistableObject obj)
        {
            using (var reader = new BinaryReader(File.Open(savePath,FileMode.Open)))
            {
                obj.Load(new GameDataReader(reader));
            }
        }

    }

}

