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
        public int saveVersion = 1;
        
        public MyShapeFactory shapeFactory;

        [SerializeField]
        private ASpawnZone spawnZone;
        
        /// <summary>
        /// 持久化存储对象,负责保存一个PersistableObject状态数据到文件中
        /// </summary>
        public PersistentStorage persisStorage;

        
        public KeyCode createKey = KeyCode.C;
        public KeyCode newGameKey = KeyCode.N;

        public KeyCode saveKey = KeyCode.S;
        
        public KeyCode loadKey = KeyCode.L;

        private List<MyShape> mCreatedShapes = new List<MyShape>();

        public Camera UICamera;

        public Camera MainCamera;

        private void Awake()
        {
            persisStorage = GetComponent<PersistentStorage>();
            persisStorage.SaveVersion = saveVersion;
            
            Debug.Log("gravity = " +  Physics.gravity);
            Physics.gravity = new Vector3(0, 0, 9.81f);
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
            var shape = shapeFactory.GetRandom();
            var cubeTrans = shape.transform;
            
            cubeTrans.SetParent(spawnZone.transform);
            
            //位置在一个半径为5米球内
            cubeTrans.localPosition = spawnZone.SpawnPoint;
            cubeTrans.localRotation = Random.rotation;
            cubeTrans.localScale = Random.Range(0.5f, 0.7f) * Vector3.one;
        
            mCreatedShapes.Add(shape);
        }
        
        void NewGame()
        {
            foreach (var objTrans in mCreatedShapes)
            {
                GameObject.Destroy(objTrans.gameObject);
            }
            
            mCreatedShapes.Clear();
            
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
            //读取version
            int version = -reader.ReadInt();
            if (version > saveVersion)
            {
                Debug.LogError("数据版本号过高,不支持");
                return;
            }
            
            //读取对象个数
            int nCount = version < 0 ? version: reader.ReadInt();
            //依次实例化每个cube对象  并从文件中加载之前保存的对象信息
            for (int i = 0; i < nCount; i++)
            {
                int shapeId = version > 0 ? reader.ReadInt() : 0;
                int matId = reader.ReadInt();
                Color col = reader.ReadColor();
                MyShape persisObject = shapeFactory.GetShape(shapeId,col,matId);
                
                persisObject.transform.SetParent(spawnZone.transform);
                
                persisObject.Load(reader);
                
                //加入到列表中
                mCreatedShapes.Add(persisObject);
            }
        }
    
        /// <summary>
        /// 保存游戏对象状态到文件中
        /// </summary>
        /// <param name="writer"></param>
        public override void Save(GameDataWriter writer)
        {
            //先写入cube数量
            writer.WriteInt(mCreatedShapes.Count);
            //依次调用每一个PersistableObject对象的save方法保存
            foreach (var cube in mCreatedShapes)
            {
                var shape = cube.GetComponent<MyShape>();
                shape.Save(writer);
            }
        }
    }

}
