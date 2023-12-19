using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RenderDemo
{
    //缩放变换组件
    public class TransformationScale : ATransformation
    {
        public Vector3 scale = Vector3.one;

        /// <summary>
        /// 缩放变换矩阵
        /// </summary>
        public override Matrix4x4 Matrix
        {
            get
            {
                Matrix4x4 matrix = new Matrix4x4();
                matrix.SetRow(0,new Vector4(scale.x,0,0,0));
                matrix.SetRow(1,new Vector4(0,scale.y,0,0));
                matrix.SetRow(2,new Vector4(0,0,scale.z,0));
                matrix.SetRow(3,new Vector4(0,0,0,1));

                return matrix;
            }
        }

        public override Vector3 Apply(Vector3 point)
        {
            // point.x *= scale.x;
            // point.y *= scale.y;
            // point.z *= scale.z;

            point = Matrix.MultiplyPoint(point);
            return point;
        }
        
    }
}

