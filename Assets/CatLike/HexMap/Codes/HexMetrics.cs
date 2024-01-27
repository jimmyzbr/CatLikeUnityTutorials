using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatLike.HexMap.Codes
{
    
    public static class HexMetrics
    {
        /// <summary>
        /// HexMap 外半径 默认10米
        /// </summary>
        public const float outerRadius = 10;
    
        /// <summary>
        /// Hexmap 内半径 ( outerRadius * cos30)
        /// </summary>
        public const float innerRadius = outerRadius * 0.866025404f ;

        /// <summary>
        /// 六边形(尖角朝上)的六个顶点坐标，按照顺时针顺序
        /// </summary>
        public static Vector3[] corners =
        {
            new Vector3(0,0,outerRadius),
            new Vector3(innerRadius,0,outerRadius * 0.5f),
            new Vector3(innerRadius,0,-outerRadius * 0.5f),
            new Vector3(0,0,-outerRadius),
            new Vector3(-innerRadius,0,-outerRadius * 0.5f),
            new Vector3(-innerRadius,0,outerRadius * 0.5f),
            new Vector3(0,0,outerRadius), //形成一个环 最后一个顶点和第一个顶点重合
        };

    }

}
