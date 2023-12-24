
using UnityEngine;

namespace ObjectManagerDemo
{
    /// <summary>
    /// 出生区域
    /// </summary>
    public abstract class ASpawnZone : MonoBehaviour
    {
        /// <summary>
        /// 出生点
        /// </summary>
        public  abstract  Vector3 SpawnPoint { get; }
    }
    
}