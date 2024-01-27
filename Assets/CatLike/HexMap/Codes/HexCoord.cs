using System;
using UnityEngine;

namespace CatLike.HexMap.Codes
{
    /// <summary>
    /// 六边形Cube坐标 Q+R+S = 0
    /// </summary>
    [Serializable]
    public struct HexCoord
    {
        public int X;   //q
        public int Z;   //R 

        /// <summary>
        /// Y维度 坐标
        /// </summary>
        public int Y   //S
        {
            get
            {
                return -X - Z;
            }
        }
        public HexCoord(int x, int z)
        {
            X = x;
            Z = z;
        }

        
        /// <summary>
        /// 从常规偏移坐标转化为HexCoord
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static HexCoord FromOffsetCoord(int x, int z)
        {
           return new HexCoord(x - z / 2, z);
        }

        /// <summary>
        /// 世界坐标转化为立方体坐标
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static HexCoord FromPosition(Vector3 pos)
        {
            float x = pos.x / (HexMetrics.innerRadius * 2);   //X 坐标是内半径2倍
            // 只有当Z = 0的时候 Y是X坐标镜像
            float y = -x;
            //在Cube坐标系中 往Z方向走2格子（三个外半径的距离） x方向上的格子数加1
            float offset = pos.z / (HexMetrics.outerRadius * 3f);
            x -= offset;
            y -= offset;
            
            int iX = Mathf.RoundToInt(x);
            int iY = Mathf.RoundToInt(y);
            int iZ = Mathf.RoundToInt(-x -y);

            if (iX + iY + iZ != 0)
            {
                Debug.LogError("rounding error");
                //丢弃四舍五入增量最大的坐标,用其他另外两个坐标重新计算HexCoord
                float dX = Mathf.Abs(x - iX);
                float dY = Mathf.Abs(y - iY);
                float dZ = Mathf.Abs(-x -y - iZ);
                
                if (dX > dY && dX > dZ) //x舍弃量最大,使用iY 和iZ 重建
                {
                    iX = -iY - iZ;
                }
                else if (dZ > dY) {
                    iZ = -iX - iY;
                }
            }
            return new HexCoord(iX, iZ);
        }

        public override string ToString()
        {
            return $"({X},{Y},{Z})";
        }
    }
}