using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ObjectManagerDemo
{
    /// <summary>
    /// 立方体出生点
    /// </summary>
    public class CubeSpawnZone : ASpawnZone
    {
        //只生成在球体表面
        [SerializeField]
        bool surfaceOnly;
        
        public override Vector3 SpawnPoint 
        {
            get
            {
                //在单位立方体内随机一个点

                Vector3 localScale = transform.localScale;
                Vector3 localPos;
                localPos.x = Random.Range(-0.5f, 0.5f) * localScale.x;
                localPos.y = Random.Range(-0.5f, 0.5f) * localScale.y;
                localPos.z = Random.Range(0.2f, 0.5f); 
                //localPos.z = 0;
                if (surfaceOnly)
                {
                    int axis = Random.Range(0, 3); //随机一个轴
                    //随机出来点的位置只能在其余两个轴组成的平面上
                    localPos[axis] = localPos[axis] < 0f ? -0.5f : 0.5f;
                }
                
                //把随机出来的点从SpawnZone的局部坐标变换到世界坐标
                //return transform.TransformPoint(localPos);
                return localPos;
            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
    }
}
