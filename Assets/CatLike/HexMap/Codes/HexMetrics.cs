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

        /// <summary>
        /// 根据方向获取该方向上的第一个角的坐标
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static Vector3 GetFirstConner(HexDirection dir)
        {
            return corners[(int)dir];
        }

        /// <summary>
        /// 根据方向获取该方向上的第二个角的坐标
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static Vector3 GetSecondConner(HexDirection dir)
        {
            return corners[(int)dir + 1];
        }

        public static Vector3[] GetConners(HexDirection dir)
        {
            return new Vector3[] { corners[(int)dir], corners[(int)dir + 1] };
        }
        

    }

}
