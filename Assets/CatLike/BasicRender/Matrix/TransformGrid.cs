using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RenderDemo
{
    public class TransformGrid : MonoBehaviour
    {
        public GameObject cubePrefab;

        public int gridSize = 10;

        private Transform[] mGrid;

        private List<ATransformation> mTransList;

        private void Awake()
        {
            mTransList = new List<ATransformation>();
            
            mGrid = new Transform[gridSize * gridSize * gridSize];
            for (int i = 0, z = 0; z < gridSize; z++)
            {
                for (int y = 0; y < gridSize; y++)
                {

                    for (int x = 0; x < gridSize; x++,i++)
                    {
                        mGrid[i] = CreateGridPoint(x, y, z);
                    }
                }
            }
        }

        private Transform CreateGridPoint(int x, int y, int z)
        {
            Transform t = GameObject.Instantiate(cubePrefab).transform;
            t.localPosition = GetCoordinates(x, y, z);
            t.GetComponent<MeshRenderer>().material.color = new Color(
                1.0f * x / gridSize, 1.0f * y / gridSize, 1.0f * z / gridSize);
            return t;
        }

        public Vector3 GetCoordinates(int x, int y, int z)
        {
            //把立方体的中心和组件gameobject的位置重合
            
            var offset = (gridSize - 1) * 0.5f;
            return new Vector3(x - offset, y - offset, z - offset);
        }

        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            //获取所有的ATransformation组件
            GetComponents<ATransformation>(mTransList);
         
            //更新组件变换之后的位置
            for (int i = 0, z = 0; z < gridSize; z++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    for (int x = 0; x < gridSize; x++,i++)
                    {
                        mGrid[i].localPosition = TransformPoint(x, y, z);
                    }
                }
            }
        }

        public Vector3 TransformPoint(int x, int y, int z)
        {
            //获取原始的位置
            var point = GetCoordinates(x, y, z);
            
            //对于point依次执行每一个变换操作
            for (int i = 0; i < mTransList.Count; i++)
            {
                point = mTransList[i].Apply(point);
            }
            
            //返回变换之后的点
            return point;
        }
    }

}
