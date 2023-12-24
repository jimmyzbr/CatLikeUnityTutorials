using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ObjectManagerDemo
{
    /// <summary>
    /// 球形出生点
    /// </summary>
    public class SphereSpawnZone : ASpawnZone
    {
        //只生成在球体表面
        [SerializeField]
        bool surfaceOnly;
        
        public override Vector3 SpawnPoint {
            get
            {
                var localPos = surfaceOnly ? UnityEngine.Random.onUnitSphere : UnityEngine.Random.insideUnitSphere;
             
                //把随机出来的点从SpawnZone的局部坐标变换到世界坐标
                return transform.TransformPoint(localPos);
            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireSphere(Vector3.zero, 1f);
        }
    }
}
