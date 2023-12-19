using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RenderDemo
{
    //位置变换组件
    public class TransformationPos : ATransformation
    {
        public Vector3 position = Vector3.zero;

        /// <summary>
        /// 平移变换矩阵
        /// </summary>
        public override Matrix4x4 Matrix
        {
            get
            {
                Matrix4x4 matrix = new Matrix4x4();
                matrix.SetRow(0,new Vector4(1,0,0,position.x));
                matrix.SetRow(1,new Vector4(0,1,0,position.y));
                matrix.SetRow(2,new Vector4(0,0,1,position.z));
                matrix.SetRow(3,new Vector4(0,0,0,1));
                return matrix;
            }
        }

        public override Vector3 Apply(Vector3 point)
        {
            //return point + position;
            //使用变换矩阵去变换点point的位置
            return Matrix.MultiplyPoint(point);
        }
        
    }
}

