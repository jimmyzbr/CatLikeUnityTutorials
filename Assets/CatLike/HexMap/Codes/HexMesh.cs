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
                
                //TriangulateCellByDirection(cell);
              
                //TriangulateSolidCellByDirection(cell);
                
                TriangulateCellFinal(cell);
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
        /// 根据格子的6个方向来生成实体区域的三角形
        /// </summary>
        /// <param name="cell"></param>
        void TriangulateSolidCellByDirection(HexCell cell)
        {
            var center = cell.transform.localPosition;
            
            for (var d = HexDirection.NE; d <= HexDirection.NW ; d++)
            {
                var corner1 = HexMetrics.GetFirstSolidConner(d);
                var corner2 = HexMetrics.GetSecondSolidConner(d);
                //Color[] colors = GetTriangleColors(cell, d);
                Color[] colors = { cell.CellColor, cell.CellColor, cell.CellColor };
                AddTriangle(center, center + corner1, center + corner2,colors);

#region 混合区域为梯形的情况

                /*
                var v1 = center + corner1;
                var v2 = center + corner2;
                var v3 = center +  HexMetrics.GetFirstConner(d);
                var v4 = center + HexMetrics.GetSecondConner(d);

                // var bridge = HexMetrics.GetBridge(d);
                // var v3 = v1 + bridge;
                // var v4 = v2 + bridge;
                
                
                //获取相邻格子
                HexCell neighborCell = cell.GetNeighborCell(d);
                if (neighborCell == null)
                    neighborCell = cell;
            
                //上一个方向的相邻格子
                HexCell preNeighborCell = cell.GetNeighborCell(d.Pre());
                if (preNeighborCell == null)
                    preNeighborCell = cell;
            
                //下一个方向的相邻格子
                HexCell nextNeighborCell = cell.GetNeighborCell(d.Next());
                if (nextNeighborCell == null)
                    nextNeighborCell = cell;
            
                var color1 = cell.CellColor;
                //顶点的颜色由自身和其余两个相邻格子的颜色决定
                //第一个conner的颜色
                var color3 = (cell.CellColor + preNeighborCell.CellColor + neighborCell.CellColor) / 3f;
                //第二个conner的颜色
                var color4 = (cell.CellColor + neighborCell.CellColor + nextNeighborCell.CellColor) / 3f;

                Color[] colorsQuad = { color1, color1, color3, color4 };
                
                //添加混合区域quad颜色
                AddQuad(v1,v2,v3,v4,colorsQuad);
                
                */
                
#endregion

#region 混合区域为矩形+ 左右两个三角形
                //混合区域为矩形+ 左右两个三角形
                //绘制中间的矩形
                var v1 = center + corner1;
                var v2 = center + corner2;
                var bridge = HexMetrics.GetBridge(d);
                var v3 = v1 + bridge;
                var v4 = v2 + bridge;

                //获取相邻格子
                HexCell neighborCell = cell.GetNeighborCell(d);
                if (neighborCell == null)
                    neighborCell = cell;
                
                //上一个方向的相邻格子
                HexCell preNeighborCell = cell.GetNeighborCell(d.Pre());
                if (preNeighborCell == null)
                    preNeighborCell = cell;
            
                //下一个方向的相邻格子
                HexCell nextNeighborCell = cell.GetNeighborCell(d.Next());
                if (nextNeighborCell == null)
                    nextNeighborCell = cell;
                
                //v1 v2的混合颜色
                var color1 = cell.CellColor;
                //v3 v4混合颜色为 当前格子和相邻格子颜色相加除2
                var bridgeColor = (cell.CellColor + neighborCell.CellColor) * 0.5f;
                Color[] colorsQuad = { color1, color1, bridgeColor, bridgeColor };
                //添加混合区域quad颜色
                AddQuad(v1,v2,v3,v4,colorsQuad);
                
                //绘制左边的三角形
                var colorFirstConner = (cell.CellColor + preNeighborCell.CellColor + neighborCell.CellColor) / 3f;
                Color[] triColors1 = { color1,colorFirstConner,bridgeColor};
                AddTriangle(v1,center + HexMetrics.GetFirstConner(d),v3,triColors1);
                
                //绘制右边三角形
                var colorSecConner = (cell.CellColor + neighborCell.CellColor + nextNeighborCell.CellColor) / 3f;
                Color[] triColors2 = { color1,bridgeColor,colorSecConner};
                AddTriangle(v2, v4, center + HexMetrics.GetSecondConner(d),triColors2);
                
                #endregion
                
            }
        }

        void TriangulateCellFinal(HexCell cell)
        {
            //生成内部的实体六边形
            for (var direction = HexDirection.NE; direction <= HexDirection.NW ; direction++)
            {
                //构建内部三角形和连接器
                TriangulateAtDirection(cell, direction);
            }
        }

        /// <summary>
        /// 在某个方向上构建六边形Cell和对应混合区域
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="direction"></param>
        void TriangulateAtDirection(HexCell cell, HexDirection direction)
        {
            var center = cell.transform.localPosition;
            var v1 = center + HexMetrics.GetFirstSolidConner(direction);
            var v2 = center + HexMetrics.GetSecondSolidConner(direction);
            Color[] colors = { cell.CellColor, cell.CellColor, cell.CellColor };
            AddTriangle(center, v1, v2, colors);

            //只需要在三个方向上绘制矩阵连接区域 （NE E SE）
           // if(direction <= HexDirection.SE)
                TriangulateConnector(cell, direction, v1, v2);
        }

        /// <summary>
        /// 绘制两个相邻cell之间的矩形连接区域
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="direction"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        void TriangulateConnector(HexCell cell, HexDirection direction, Vector3 v1, Vector3 v2)
        {
            var bridge = HexMetrics.GetBridgeLong(direction);
            var v3 = v1 + bridge;
            var v4 = v2 + bridge;
            
            //没有相邻的格子、就不再绘制连接器了
            HexCell neighborCell = cell.GetNeighborCell(direction);
            if (neighborCell == null)
            {
                // var center = cell.transform.localPosition;
                // v3 = center + HexMetrics.GetFirstConner(direction);
                // v4 = center + HexMetrics.GetSecondConner(direction);
                // var cellColor = cell.CellColor;
                // AddQuad(v1,v2,v3,v4, new []{cellColor,cellColor,cellColor,cellColor});
                
                return;
            }


            Color[] colors = { cell.CellColor,cell.CellColor,neighborCell.CellColor,neighborCell.CellColor };
            AddQuad(v1, v2, v3, v4, colors);
            
            //绘制当前cell 、neighborCell和neighborNextCell共享的三角形
            //下一个方向的相邻格子
            HexCell nextNeighborCell = cell.GetNeighborCell(direction.Next());
            //因为三个单元格共享一个三角形连接，所以我们只需要为两个连接添加它们。所以只要NE和E就可以了
            if (nextNeighborCell != null && (direction == HexDirection.NE || direction == HexDirection.E))
            {
                Vector3 next = v2 + HexMetrics.GetBridgeLong(direction.Next());
                Color[] colorsTri = { cell.CellColor,neighborCell.CellColor,nextNeighborCell.CellColor};
                AddTriangle(v2,v4,next,colorsTri);
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

        
        void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Color[] colors)
        {
            var vertexIndex = mVerticies.Count;
            mVerticies.Add(v1);
            mVerticies.Add(v2);
            mVerticies.Add(v3);
            mVerticies.Add(v4);
            
            //两个三角形
            mTriangles.Add(vertexIndex);
            mTriangles.Add(vertexIndex + 2);
            mTriangles.Add(vertexIndex + 1);
            
            mTriangles.Add(vertexIndex + 1);
            mTriangles.Add(vertexIndex + 2);
            mTriangles.Add(vertexIndex + 3);
            
            mColors.Add(colors[0]);
            mColors.Add(colors[1]);
            mColors.Add(colors[2]);
            mColors.Add(colors[3]);
            
        }
        
    }
}