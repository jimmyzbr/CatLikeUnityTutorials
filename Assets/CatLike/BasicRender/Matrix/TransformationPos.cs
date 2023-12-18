using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RenderDemo
{
    //位置变换组件
    public class TransformationPos : ATransformation
    {
        public Vector3 position = Vector3.zero;
        
        public override Vector3 Apply(Vector3 point)
        {
            return point + position;
        }
        
    }
}

