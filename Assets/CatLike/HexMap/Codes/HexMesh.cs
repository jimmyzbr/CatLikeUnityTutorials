using System;
using System.Collections.Generic;
using UnityEngine;

namespace CatLike.HexMap.Codes
{
    [RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
    public class HexMesh : MonoBehaviour
    {
        private Mesh mMesh;
        private List<Vector3> mVerticies;
        private List<Color> mColors;
        private List<int> mTriangles;

        private MeshCollider mMeshCollider;
        
        private void Awake()
        {
            mVerticies = new List<Vector3>();
            mTriangles = new List<int>();
            mColors = new List<Color>();
            
            mMesh = new Mesh();
            mMesh.name = "HexMesh";
            GetComponent<MeshFilter>().mesh = mMesh;

            mMeshCollider = gameObject.AddComponent<MeshCollider>();
        }

        /// <summary>
        /// 根据Cell构建地图网格
        /// </summary>
        /// <param name="cells"></param>
        public void BuildMesh(HexCell[] cells)
        {
            mMesh.Clear();
            mVerticies.Clear();
            mTriangles.Clear();
            mColors.Clear();

            for (int i = 0; i < cells.Length; i++)
            {
                var cell = cells[i];
                //每个cell创建6个三角形
                //TriangulateCells(cell);
                TriangulateCellByDirection(cell);
            }
            

            mMesh.vertices = mVerticies.ToArray();
            mMesh.triangles = mTriangles.ToArray();
            mMesh.colors = mColors.ToArray();
            
            mMesh.RecalculateNormals();
            
            mMeshCollider.sharedMesh = mMesh;
        }

        void TriangulateCells(HexCell cell)
        {
            var center = cell.transform.localPosition;
            for (int j = 0; j < 6; j++)
            {
                Color[] colors = new Color[]
                {
                    cell.CellColor,
                    cell.CellColor,
                    cell.CellColor,
                };
                AddTriangle(center, center + HexMetrics.corners[j], center + HexMetrics.corners[j+1],colors);
            }
        }

        /// <summary>
        /// 根据格子的6个方向来生成三角形
        /// </summary>
        /// <param name="cell"></param>
        void TriangulateCellByDirection(HexCell cell)
        {
            var center = cell.transform.localPosition;
            
            for (var d = HexDirection.NE; d <= HexDirection.NW ; d++)
            {
                var dirConners = HexMetrics.GetConners(d);

                Color[] colors = GetTriangleColors(cell, d);
                AddTriangle(center, center + dirConners[0], center + dirConners[1],colors);
            }
        }


        /// <summary>
        /// 获取Cell某个方向上混合之后的三角形颜色
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="direction"></param>
        Color[] GetTriangleColors(HexCell cell, HexDirection direction)
        {
            //获取相邻格子
            HexCell neighborCell = cell.GetNeighborCell(direction);
            if (neighborCell == null)
                neighborCell = cell;
            
            //上一个方向的相邻格子
            HexCell preNeighborCell = cell.GetNeighborCell(direction.Pre());
            if (preNeighborCell == null)
                preNeighborCell = cell;
            
            //下一个方向的相邻格子
            HexCell nextNeighborCell = cell.GetNeighborCell(direction.Next());
            if (nextNeighborCell == null)
                nextNeighborCell = cell;
            
            var color1 = cell.CellColor;
            // var color2 = neighborCell.CellColor;
            // var color3 = neighborCell.CellColor;
            //六边形边缘的颜色应该是两个相邻单元格的平均值
            //var edgeColor = (cell.CellColor + neighborCell.CellColor) * 0.5f;
            
            //顶点的颜色由自身和其余两个相邻格子的颜色决定
            //第一个conner的颜色
            var color2 = (cell.CellColor + preNeighborCell.CellColor + neighborCell.CellColor) / 3f;
            //第二个conner的颜色
            var color3 = (cell.CellColor + neighborCell.CellColor + nextNeighborCell.CellColor) / 3f;
            
            return new[] { color1, color2, color3 };
        }

        void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3,Color[] colors)
        {
            var index = mVerticies.Count;
            mVerticies.Add(v1);
            mVerticies.Add(v2);
            mVerticies.Add(v3);
            
            mColors.Add(colors[0]);
            mColors.Add(colors[1]);
            mColors.Add(colors[2]);
            
            mTriangles.Add(index);
            mTriangles.Add(index + 1);
            mTriangles.Add(index + 2);
        }
        
    }
}