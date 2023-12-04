using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;


[RequireComponent(typeof(MeshFilter) ,typeof(MeshRenderer))]
public class SimpleCube : MonoBehaviour
{
    public int XSize = 1; 
    public int YSize = 1;
    public int ZSize = 1;
    
    private MeshFilter mMeshFilter;
    
    /// <summary>
    /// cube顶点数组
    /// </summary>
    private Vector3[] mVertices;

    private int mButtomVetexStartIndex;
    private int mTopVetexStartIndex;
    private int mTotalVetexNum;
    
    /// <summary>
    /// cube的顶点索引
    /// </summary>
    private int[] mTriangles;
    
    private void Awake()
    {
        mMeshFilter = GetComponent<MeshFilter>();
        var cubeMesh = new Mesh();
        mMeshFilter.mesh = cubeMesh;

        //StartCoroutine(FillVertices());
        CreateVertices();
        
        CreateTriangles();
    
        mMeshFilter.mesh.vertices = mVertices;
        mMeshFilter.mesh.triangles = mTriangles;
        
    }
    
    IEnumerator FillVertices()
    {
        var waitForSecs = new WaitForSeconds(0.2f);
     
        //计算所需的顶点个数(无重复顶点)
        int cornerVertexNum = 8;
        int edgeVertexNum = 4 * (XSize - 1) + 4 * (YSize - 1) + 4 * (ZSize - 1);
        int innerFaceVertexNum = 2 * (XSize - 1) * (YSize - 1) + 2 * (XSize - 1) * (ZSize - 1) +
                                 2 * (ZSize - 1) * (YSize - 1);

        mVertices = new Vector3[cornerVertexNum + edgeVertexNum + innerFaceVertexNum];

        
        
        int vIndex = 0;  //顶点的索引
        
        //逆时针 从下到上填充每一层（XZ平面上的）的顶点
        for (int y = 0; y < YSize; y++)
        {
            for (int x = 0; x <= XSize; x++)
            {
                mVertices[vIndex++] = new Vector3(x, y, 0);
                yield return waitForSecs;
            }
            for (int z = 1; z <= ZSize; z++)
            {
                mVertices[vIndex++] = new Vector3(XSize, y, z);
                yield return waitForSecs;
            }
        
            for (int x = XSize -1; x >= 0; x--)
            {
                mVertices[vIndex++] = new Vector3(x, y, ZSize);
                yield return waitForSecs;
            }
        
            for (int z = ZSize -1; z >= 0; z--)
            {
                mVertices[vIndex++] = new Vector3(0, y, z);
                yield return waitForSecs;
            }
        } 
        
        //填充立方体底面的顶点 Bottom
        for (int z = 1; z <= ZSize -1 ; z++)
        {
            for (int x = 1; x <= XSize -1 ; x++)
            {
                mVertices[vIndex++] = new Vector3(x, 0, z);
                yield return waitForSecs;
            }
        }
        
        //填充立方体上面的顶点 Top
        for (int z = 1; z <= ZSize -1 ; z++)
        {
            for (int x = 1; x <= XSize -1 ; x++)
            {
                mVertices[vIndex++] = new Vector3(x, YSize, z);
                yield return waitForSecs;
            }
        }

        yield return null;
    }
    
    
    /// <summary>
    /// 创建顶点
    /// </summary>
    void CreateVertices()
    {
     
        //计算所需的顶点个数(无重复顶点)
        int cornerVertexNum = 8;
        int edgeVertexNum = 4 * (XSize - 1) + 4 * (YSize - 1) + 4 * (ZSize - 1);
        int innerFaceVertexNum = 2 * (XSize - 1) * (YSize - 1) + 2 * (XSize - 1) * (ZSize - 1) +
                                 2 * (ZSize - 1) * (YSize - 1);

        mVertices = new Vector3[cornerVertexNum + edgeVertexNum + innerFaceVertexNum];

        
        int vIndex = 0;  //顶点的索引
        
        //逆时针 从下到上填充每一层（XZ平面上的）的顶点
        for (int y = 0; y <= YSize; y++)
        {
            for (int x = 0; x <= XSize; x++)
            {
                mVertices[vIndex++] = new Vector3(x, y, 0);
            }
            for (int z = 1; z <= ZSize; z++)
            {
                mVertices[vIndex++] = new Vector3(XSize, y, z);
            }
        
            for (int x = XSize -1; x >= 0; x--)
            {
                mVertices[vIndex++] = new Vector3(x, y, ZSize);
            }
        
            for (int z = ZSize -1; z >= 1; z--)
            {
                mVertices[vIndex++] = new Vector3(0, y, z);
            }
        }

        mButtomVetexStartIndex = vIndex;
        //填充立方体底面的顶点 Bottom
        for (int z = 1; z <= ZSize -1 ; z++)
        {
            for (int x = 1; x <= XSize -1 ; x++)
            {
                mVertices[vIndex++] = new Vector3(x, 0, z);
            }
        }
        
        mTopVetexStartIndex = vIndex;
        //填充立方体上面的顶点 Top
        for (int z = 1; z <= ZSize -1 ; z++)
        {
            for (int x = 1; x <= XSize -1 ; x++)
            {
                mVertices[vIndex++] = new Vector3(x, YSize, z);
            }
        }
        
        mTotalVetexNum = vIndex;
    }

    /// <summary>
    /// 创建三角形索引
    /// </summary>
    void CreateTriangles()
    {
        //计算所需的索引个数 (一共6个面，每个面的索引个数 = 每个面的quad个数 * 6 )
        int quadCount = (XSize * YSize * 2 + XSize * ZSize * 2 + YSize * ZSize * 2);
        mTriangles = new int[quadCount * 6];
        int curTriangleIndex = 0;
        //一圈的顶底个数
        int ringVertNum = (XSize + 1) *2 +  (ZSize + 1) * 2 - 4;
        //一圈的quad数量
        int ringQuadNum = (XSize + ZSize) * 2;
        
        //第一个quad的三角形
        //curTriangleIndex = SetQuadTriangle(mTriangles, curTriangleIndex, 0, 1, ringVertNum,ringVertNum + 1);
        
        //从下到上依次设置一圈三角形
        int v = 0;
        for (int y = 0; y < YSize; y++)
        {
            int q = 0;
            for (q = 0; q < ringQuadNum - 1; q++)
            {
                curTriangleIndex = SetQuadTriangle(mTriangles, curTriangleIndex, v + q,  v + q + 1, v + ringVertNum + q, v + ringVertNum + q + 1);
            }
            //一圈的最后一个quad的v10 v11要跟第一个quad的v00,v01 重合
            curTriangleIndex = SetQuadTriangle(mTriangles, curTriangleIndex, v + q, v , v + ringVertNum + q,v + ringVertNum);
            
            //处理玩一圈之后顶点index 加 ringVertNum
            v += ringVertNum;
        }
        
        Debug.Log("vIndex: " + v);
        
        //设置立方体上面和下面的三角形
        //1 底面
        curTriangleIndex = CreateBottomTriangles(curTriangleIndex,mButtomVetexStartIndex,ringVertNum);

        
        //2 上面
       curTriangleIndex = CreateTopTriangles(curTriangleIndex, mTopVetexStartIndex, ringVertNum);
    }

    /// <summary>
    /// 创建底面的三角形索引
    /// </summary>
    /// <param name="curTriangleIndex">当前三角形index</param>
    /// <param name="buttomStartVertex">底面开始的顶点index</param>
    /// <param name="ringVertNum">XZ平面一圈的顶点个数</param>
    /// <returns></returns>
    int CreateBottomTriangles(int curTriangleIndex,int buttomStartVertex, int ringVertNum)
    {
         //第一排
        var v00 = 0;
        var v01 = 0;
        var v11 = buttomStartVertex;
        var v10 = 0;
        curTriangleIndex = SetQuadTriangle(mTriangles, curTriangleIndex, v00, v00 + 1,ringVertNum - 1,v11);
        for (int x = 1; x < XSize - 1; x++)
        {
            curTriangleIndex = SetQuadTriangle(mTriangles, curTriangleIndex, v00 + x,  v00 + x + 1,v11 + x - 1,v11 + x);
        }
        curTriangleIndex = SetQuadTriangle(mTriangles, curTriangleIndex,v00 + XSize - 1 ,  v00 + XSize, v11 + XSize - 2, XSize + 1);
        //中间

        for (int z = 1; z < ZSize - 1; z++)
        {
            //中间第一个
            v00 = ringVertNum - z;
            v01 = v00 - 1;
            v10 = buttomStartVertex + (z-1) * (XSize - 1);
            v11 = v10 + (XSize - 1);
            curTriangleIndex = SetQuadTriangle(mTriangles, curTriangleIndex, v00, v10 ,v01,v11);

            for (int x = 1; x < XSize - 1; x++)
            {
                v00 = buttomStartVertex + (z - 1) * (XSize - 1) + (x-1);
                v01 = v00 + (XSize - 1);
                v10 = v00 + 1;
                v11 = v01 + 1;
                curTriangleIndex = SetQuadTriangle(mTriangles, curTriangleIndex, v00, v10 ,v01,v11);
            }
            
            //中间最后一个
            v00 = buttomStartVertex + z * (ZSize - 1) - 1;
            v01 = v00 + (ZSize - 1);
            v10 = XSize + z;
            v11 = v10 + 1;
            curTriangleIndex = SetQuadTriangle(mTriangles, curTriangleIndex, v00, v10 ,v01,v11);
            
        }
        
        
        //最后一排
        v00 = ringVertNum - (ZSize - 1);
        v01 =  v00 - 1;
        v11 = v00 - 2;
        v10 = buttomStartVertex + (XSize - 1) * (ZSize - 2);
        
        var vStart = buttomStartVertex;
        curTriangleIndex = SetQuadTriangle(mTriangles, curTriangleIndex, v00, v10 ,v01,v11);
        
        //最后一排中间
        for (int x = 1; x < XSize - 1; x++)
        {
            v00 = buttomStartVertex + (XSize - 1) * (ZSize - 2) + x - 1;
            v10 = v00 + 1;
            v01 = ringVertNum - ZSize - x;
            v11 = ringVertNum - ZSize - (x+1);
            curTriangleIndex = SetQuadTriangle(mTriangles, curTriangleIndex, v00,  v10, v01, v11);
            
        }
        
        v00  =  buttomStartVertex + (XSize - 1) * (ZSize - 1) - 1;
        v10 = XSize + ZSize - 1;
        v01 = v10 + 2;
        v11 = v10 + 1;
        curTriangleIndex = SetQuadTriangle(mTriangles, curTriangleIndex,v00 ,  v10, v01,v11);

        return curTriangleIndex;
    }
    
    /// <summary>
    /// 创建顶部的三角形索引
    /// </summary>
    /// <param name="curTriangleIndex">当前三角形index</param>
    /// <param name="buttomStartVertex">底面开始的顶点index</param>
    /// <param name="ringVertNum">XZ平面一圈的顶点个数</param>
    /// <returns></returns>
    int CreateTopTriangles(int curTriangleIndex,int topStartVertex, int ringVertNum)
    {
        var topRingStartVertextIndex = mButtomVetexStartIndex - ringVertNum;
        var topRingEndVertextIndex = mButtomVetexStartIndex - 1;
        
         //第一排
        var v00 = topRingStartVertextIndex;
        var v01 = topRingEndVertextIndex;
        var v11 = topStartVertex;
        var v10 = v00 + 1;
        curTriangleIndex = SetQuadTriangle(mTriangles, curTriangleIndex, v00, v10,v01,v11);
        for (int x = 1; x < XSize - 1; x++)
        {
            curTriangleIndex = SetQuadTriangle(mTriangles, curTriangleIndex, v00 + x,  v00 + x + 1,v11 + x - 1,v11 + x);
        }
        curTriangleIndex = SetQuadTriangle(mTriangles, curTriangleIndex,v00 + XSize - 1 ,  v00 + XSize, v11 + XSize - 2, v00 + XSize + 1);
        
        
        //中间
        
        for (int z = 1; z < ZSize - 1; z++)
        {
            //中间第一个
            v00 = (topRingEndVertextIndex + 1) - z;
            v01 = v00 - 1;
            v10 = topStartVertex + (z-1) * (XSize - 1);
            v11 = v10 + (XSize - 1);
            curTriangleIndex = SetQuadTriangle(mTriangles, curTriangleIndex, v00, v10 ,v01,v11);

            for (int x = 1; x < XSize - 1; x++)
            {
                v00 = topStartVertex + (z - 1) * (XSize - 1) + (x-1);
                v01 = v00 + (XSize - 1);
                v10 = v00 + 1;
                v11 = v01 + 1;
                curTriangleIndex = SetQuadTriangle(mTriangles, curTriangleIndex, v00, v10 ,v01,v11);
            }
            
            //中间最后一个
            v00 = topStartVertex + z * (ZSize - 1) - 1;
            v01 = v00 + (ZSize - 1);
            v10 = topRingStartVertextIndex +  XSize + z;
            v11 = v10 + 1;
            curTriangleIndex = SetQuadTriangle(mTriangles, curTriangleIndex, v00, v10 ,v01,v11);
            
        }
        
    
        
        //最后一排
        v00 = topRingEndVertextIndex - (ZSize - 2);
        v01 =  v00 - 1;
        v11 = v00 - 2;
        v10 = topStartVertex + (XSize - 1) * (ZSize - 2);
        
        curTriangleIndex = SetQuadTriangle(mTriangles, curTriangleIndex, v00, v10 ,v01,v11);
        
        //最后一排中间
        for (int x = 1; x < XSize - 1; x++)
        {
            v00 = topStartVertex + (XSize - 1) * (ZSize - 2) + x - 1;
            v10 = v00 + 1;
            v01 = (topRingEndVertextIndex + 1) - ZSize - x;
            v11 = (topRingEndVertextIndex + 1) - ZSize - (x+1);
            curTriangleIndex = SetQuadTriangle(mTriangles, curTriangleIndex, v00,  v10, v01, v11);
            
        }

        v00 = topStartVertex + (XSize - 1) * (ZSize - 1) - 1;
        v10 = topRingStartVertextIndex + XSize + ZSize - 1;
        v01 = v10 + 2;
        v11 = v10 + 1;
        curTriangleIndex = SetQuadTriangle(mTriangles, curTriangleIndex,v00 ,  v10, v01,v11);

        return curTriangleIndex;
    }
    
    /// <summary>
    /// 设置一个quad的三角形索引
    ///     v01------------v11
    ///       |            |
    ///       |            |
    ///       |            |
    ///     v00------------v10
    /// </summary>
    /// <param name="triangles">三角形索引数组</param>
    /// <param name="curTriangleIndex">当前三角形索引数组的Index</param>
    /// <param name="V00">左下角顶点index</param>
    /// <param name="V01">右下角顶点index</param>
    /// <param name="V10">左上角顶点index</param>
    /// <param name="V11">左上角顶点index</param>
    /// <returns>返回当前索引数组的Index</returns>
    static int SetQuadTriangle(int[] triangles, int curTriangleIndex, int V00, int V10,int V01,int V11)
    {
        triangles[curTriangleIndex] = V00;
        triangles[curTriangleIndex + 1] = V01;
        triangles[curTriangleIndex + 2] = V10;
        triangles[curTriangleIndex + 3] = V10;
        triangles[curTriangleIndex + 4] = V01;
        triangles[curTriangleIndex + 5] = V11;
        
        return curTriangleIndex + 6;
    }
    
    

    private void OnDrawGizmos()
    {
        if (mVertices == null)
            return;

        Gizmos.color = Color.blue;
        for (int i = 0; i < mVertices.Length; i++)
        {
            Gizmos.DrawSphere(mVertices[i],0.1f);
        }
    }
}
