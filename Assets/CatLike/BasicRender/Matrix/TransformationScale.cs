using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RenderDemo
{
    //缩放变换组件
    public class TransformationScale : ATransformation
    {
        public Vector3 scale = Vector3.one;
        public override Vector3 Apply(Vector3 point)
        {
            point.x *= scale.x;
            point.y *= scale.y;
            point.z *= scale.z;

            return point;
        }
        
    }
}

