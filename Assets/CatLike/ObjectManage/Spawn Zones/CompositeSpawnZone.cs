
using UnityEngine;

namespace ObjectManagerDemo
{
    /// <summary>
    /// 组合区域
    /// </summary>
    public class CompositeSpawnZone : ASpawnZone
    {
        /// <summary>
        /// 所有的生成区域
        /// </summary>
        [SerializeField]
        ASpawnZone[] spawnZones;

        /// <summary>
        /// 随一个生成区域,再从区域出随机一个出生点
        /// </summary>
        public override Vector3 SpawnPoint {
            get
            {
                var spawnZone = spawnZones[Random.Range(0, spawnZones.Length)];
                return spawnZone.SpawnPoint;
            }
        }
        
        
    }
    
}