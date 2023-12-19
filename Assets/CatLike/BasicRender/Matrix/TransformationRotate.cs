using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RenderDemo
{
    //旋转变换组件
    public class TransformationRotate : ATransformation
    {
        public Vector3 rotation = Vector3.zero;

        public override Matrix4x4 Matrix
        {
            get
            {
                float radX = rotation.x * Mathf.Deg2Rad;
                float radY = rotation.y * Mathf.Deg2Rad;
                float radZ = rotation.z * Mathf.Deg2Rad;
                float sinX = Mathf.Sin(radX);
                float cosX = Mathf.Cos(radX);
                float sinY = Mathf.Sin(radY);
                float cosY = Mathf.Cos(radY);
                float sinZ = Mathf.Sin(radZ);
                float cosZ = Mathf.Cos(radZ);
			
                Matrix4x4 matrix = new Matrix4x4();
                matrix.SetColumn(0, new Vector4(
                    cosY * cosZ,
                    cosX * sinZ + sinX * sinY * cosZ,
                    sinX * sinZ - cosX * sinY * cosZ,
                    0f
                ));
                matrix.SetColumn(1, new Vector4(
                    -cosY * sinZ,
                    cosX * cosZ - sinX * sinY * sinZ,
                    sinX * cosZ + cosX * sinY * sinZ,
                    0f
                ));
                matrix.SetColumn(2, new Vector4(
                    sinY,
                    -sinX * cosY,
                    cosX * cosY,
                    0f
                ));
                matrix.SetColumn(3, new Vector4(0f, 0f, 0f, 1f));
                return matrix;
            }
        }

        public override Vector3 Apply(Vector3 point)
        {
            // point = UnitySolution(point);
            // return point;
            
            //使用旋转矩阵进行变换
            return Matrix.MultiplyPoint(point);
            
            
            float radX = rotation.x * Mathf.Deg2Rad;
            float radY = rotation.y * Mathf.Deg2Rad;
            float radZ = rotation.z * Mathf.Deg2Rad;
            float sinX = Mathf.Sin(radX);
            float cosX = Mathf.Cos(radX);
            float sinY = Mathf.Sin(radY);
            float cosY = Mathf.Cos(radY);
            float sinZ = Mathf.Sin(radZ);
            float cosZ = Mathf.Cos(radZ);

            //旋转矩阵 rotMatrix
            //  |xAxis |
            //  |yAxis |
            //  |zAxis |
            Vector3 xAxis = new Vector3(
                cosY * cosZ,
                cosX * sinZ + sinX * sinY * cosZ,
                sinX * sinZ - cosX * sinY * cosZ
            );
            Vector3 yAxis = new Vector3(
                -cosY * sinZ,
                cosX * cosZ - sinX * sinY * sinZ,
                sinX * cosZ + cosX * sinY * sinZ
            );
            Vector3 zAxis = new Vector3(
                sinY,
                -sinX * cosY,
                cosX * cosY
            );
    
            //通过旋转矩阵乘法来变换point点   rotMatrix * point
            point = xAxis * point.x + yAxis * point.y + zAxis * point.z;
            
            return point;
        }
    
        
        /// <summary>
        /// 使用四元素的实现
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        Vector3 UnitySolution(Vector3 point)
        {
            Quaternion rot = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
            point = rot * point;
            return point;
        }
        
    }
}

