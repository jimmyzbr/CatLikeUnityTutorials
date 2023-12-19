using UnityEngine;

namespace RenderDemo
{
    /// <summary>
    /// 摄像机变换
    /// </summary>
    public class CameraTransformation : ATransformation
    {
        public override Matrix4x4 Matrix
        {
            get
            {
                //单位矩阵
                
                // Matrix4x4 matrix = new Matrix4x4();
                // matrix.SetRow(0, new Vector4(1f, 0f, 0f, 0f));
                // matrix.SetRow(1, new Vector4(0f, 1f, 0f, 0f));
                // matrix.SetRow(2, new Vector4(0f, 0f, 1f, 0f));
                // matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
                // return matrix;
                
                //正交投影矩阵
                Matrix4x4 matrix = new Matrix4x4();
                matrix.SetRow(0, new Vector4(1f, 0f, 0f, 0f));
                matrix.SetRow(1, new Vector4(0f, 1f, 0f, 0f));
                matrix.SetRow(2, new Vector4(0f, 0f, 0f, 0f));
                matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
                return matrix;
                
            }
        }

        public override Vector3 Apply(Vector3 point)
        {
            return Matrix.MultiplyPoint(point);
        }
    }
}