using System;
using System.Collections.Generic;
using UnityEngine;

namespace CatLike.HexMap.Codes
{
    public class HexCell : MonoBehaviour
    {
        public HexCoord HexCoord;
        public Color CellColor;

        /// <summary>
        /// 邻居格子6个
        /// </summary>

        public HexCell[] neighbors;

        /// <summary>
        /// 设置某一个方向的邻居cell
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="cell"></param>
        public void SetNeighbor(HexDirection dir, HexCell cell)
        {
            neighbors[(int)dir] = cell;
            //同时把自己设置为邻居的邻居
            cell.neighbors[(int)dir.Opposite()] = this;
        }

        /// <summary>
        /// 获取某一个方向的相邻格子 可能会空
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public HexCell GetNeighborCell(HexDirection direction)
        {
            return neighbors[(int)direction];
        }
    }
}