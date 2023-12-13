using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ObjectManagerDemo
{
    /// <summary>
    /// 直接继承PersistableObject 表示当前游戏状态也是一个持久化对象
    /// </summary>
    public class GameDemo : PersistableObject
    {
        // Start is called before the first frame update

        /// <summary>
        /// 持久化存储对象,负责保存一个PersistableObject状态数据到文件中
        /// </summary>
        public PersistentStorage persisStorage;
        
        public Transform prefab;

        public KeyCode createKey = KeyCode.C;
        public KeyCode newGameKey = KeyCode.N;

        public KeyCode saveKey = KeyCode.S;
        
        public KeyCode loadKey = KeyCode.L;

        private List<Transform> mCreatedObjects = new List<Transform>();

        private void Awake()
        {
            persisStorage = GetComponent<PersistentStorage>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(createKey))
            {
                CreateObject();
            }
            else if (Input.GetKeyDown(newGameKey))
            {
                NewGame();
            }
            else if (Input.GetKeyDown(saveKey))
            {
                SaveGame();
            }
            else if (Input.GetKeyDown(loadKey))
            {
                LoadGame();
            }
        }

        /// <summary>
        /// 每次随机创建一个cube对象,位置、旋转 缩放随机
        /// </summary>
        void CreateObject()
        {
            var cubeTrans = Instantiate(prefab).transform;
            //位置在一个半径为5米球内
            cubeTrans.localPosition = Random.insideUnitSphere * 5f;
            cubeTrans.localRotation = Random.rotation;
            cubeTrans.localScale = Random.Range(0.2f, 1.5f) * Vector3.one;
        
            mCreatedObjects.Add(cubeTrans);
        }
        
        void NewGame()
        {
            foreach (var objTrans in mCreatedObjects)
            {
                GameObject.Destroy(objTrans.gameObject);
            }
            
            mCreatedObjects.Clear();
            
        }
        
        void SaveGame()
        {
            persisStorage.Save(this);
            Debug.Log("Save Suc!");
        }
        
        void LoadGame()
        {
            NewGame();
            persisStorage.Load(this);
            
            Debug.Log("Load Suc!");
        }

        /// <summary>
        /// 从文件中加载游戏对象
        /// </summary>
        /// <param name="reader"></param>
        public override void Load(GameDataReader reader)
        {
            //读取对象个数
            int nCount = reader.ReadInt();
            //依次实例化每个cube对象  并从文件中加载之前保存的对象信息
            for (int i = 0; i < nCount; i++)
            {
                PersistableObject persisObject = Instantiate(prefab).GetComponent<PersistableObject>();
                persisObject.Load(reader);
                
                //加入到列表中
                mCreatedObjects.Add(persisObject.transform);
            }
        }
    
        /// <summary>
        /// 保存游戏对象状态到文件中
        /// </summary>
        /// <param name="writer"></param>
        public override void Save(GameDataWriter writer)
        {
            //先写入cube数量
            writer.WriteInt(mCreatedObjects.Count);
            //依次调用每一个PersistableObject对象的save方法保存
            foreach (var cube in mCreatedObjects)
            {
                var persisObject = cube.GetComponent<PersistableObject>();
                persisObject.Save(writer);
            }
        }
    }

}
