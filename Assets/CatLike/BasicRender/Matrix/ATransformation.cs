using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RenderDemo
{
    //变换组件抽象类
    public abstract class ATransformation : MonoBehaviour
    {
        /// <summary>
        /// 把一个点point通过某种变换操作,得到一个新的点并返回
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public abstract Vector3 Apply(Vector3 point);
    }
}

