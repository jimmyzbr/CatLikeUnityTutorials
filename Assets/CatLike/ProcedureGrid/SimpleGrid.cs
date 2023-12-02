using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class SimpleGrid : MonoBehaviour
{
    /// <summary>
    /// 格子宽度
    /// </summary>
    public int GridXSize = 2;
    /// <summary>
    /// 格子高度
    /// </summary>
    public int GridYSize = 2;

    /// <summary>
    /// 顶点
    /// </summary>
    private Vector3[] mVertices;
    
    /// <summary>
    /// 索引
    /// </summary>
    private int[] mTriangles;
    
    private void Awake()
    {
        StartCoroutine(GenerateGrid());
    }

    IEnumerator GenerateGrid()
    {
        //所需顶点位置信息
        var vertextCount = (GridXSize + 1) * (GridYSize + 1);
        
        mVertices = new Vector3[vertextCount];
        //每个顶点的UV坐标
        Vector2[] uvCoords = new Vector2[vertextCount];
        //每个顶点的切线坐标
        Vector4[] tangents = new Vector4[vertextCount];
        
        //每一个quad需要6个索引（2个三角形状）
        mTriangles = new int[GridXSize * GridYSize * 6];

        //设置顶点信息
        for (int y = 0; y < GridYSize + 1; y++)
        {
            for (int x = 0; x < GridXSize + 1; x++)
            {
                var vertexIndex = x + y * (GridXSize + 1);
                
                mVertices[vertexIndex] = new Vector3(x, y, 0);
                uvCoords[vertexIndex] = new Vector2((float)x / GridXSize, (float)y / GridYSize);
                
                //As we have a flat surface, all tangents simply point in the same direction, which is to the right.
                tangents[vertexIndex] = new Vector4(1, 0, 0, -1);
            }
        }
        
        //设置索引信息
        //为每一个quad设置顶点索引数组
        var triangleIndex = 0;
        for (int y = 0; y < GridYSize; y++)
        {
            for (int x = 0; x < GridXSize; x++)
            {
                mTriangles[triangleIndex] = x + y * (GridXSize + 1) ;
                mTriangles[triangleIndex + 1] = x + (y +1) * (GridXSize + 1);
                mTriangles[triangleIndex +2] =  (x + 1) + y * (GridXSize + 1) ;
                mTriangles[triangleIndex +3] =  (x + 1) + y * (GridXSize + 1) ;
                mTriangles[triangleIndex +4] =  x + (y +1) * (GridXSize + 1);
                mTriangles[triangleIndex +5] =  (x + 1) + (y +1) * (GridXSize + 1);

                triangleIndex += 6;
            }
        }
       
        

        var mesh = new Mesh();
        mesh.vertices = mVertices;
        mesh.triangles = mTriangles;
        //自动计算法线
        mesh.RecalculateNormals();
        //设置mesh uv坐标
        mesh.uv = uvCoords;
        mesh.tangents = tangents;
        
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        yield return null;
    }
    
    private void OnDrawGizmos()
    {
        if(mVertices == null)
            return;
        
        Gizmos.color = Color.blue;
        foreach (var v in mVertices)
        {
            Gizmos.DrawSphere(v,0.05f);
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
