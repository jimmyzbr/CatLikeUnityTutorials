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
            mMeshCollider.sharedMesh = mMesh;
        }

        /// <summary>
        /// 三角形化
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
                var center = cell.transform.localPosition;
                for (int j = 0; j < 6; j++)
                {
                    AddTriangle(center, center + HexMetrics.corners[j], center + HexMetrics.corners[j+1],cell.CellColor);
                }
            }

            mMesh.vertices = mVerticies.ToArray();
            mMesh.triangles = mTriangles.ToArray();
            mMesh.colors = mColors.ToArray();
            
            mMesh.RecalculateNormals();
        }
        

        void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3,Color color)
        {
            var index = mVerticies.Count;
            mVerticies.Add(v1);
            mVerticies.Add(v2);
            mVerticies.Add(v3);
            
            mColors.Add(color);
            mColors.Add(color);
            mColors.Add(color);
            
            mTriangles.Add(index);
            mTriangles.Add(index + 1);
            mTriangles.Add(index + 2);
        }
    }
}