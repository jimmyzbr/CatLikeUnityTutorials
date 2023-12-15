using System.Collections.Generic;
using UnityEngine;

namespace ObjectManagerDemo
{
    /// <summary>
    /// 形状工厂类
    /// </summary>
    [CreateAssetMenu(fileName = "MyShapeFactory", menuName = "New MyShapeFactory", order = 0)]
    public class MyShapeFactory : ScriptableObject
    {
        [SerializeField]
        List<MyShape> shapes;

        [SerializeField]
        private List<Material> materials;

        public MyShape GetShape(int shapeId,Color col,int matId = 0)
        {
            Debug.Assert(shapeId < shapes.Count);
            var shape =  GameObject.Instantiate(shapes[shapeId]);
            shape.ShapeId = shapeId;
           
            var mat = materials[matId];
            shape.SetMaterial(mat,matId);
            
            shape.SetColor(col);
            
            return shape;
        }

        public MyShape GetRandom()
        {
            var shapeId = Random.Range(0, shapes.Count);
            var matId = Random.Range(0, materials.Count);
            var color = Random.ColorHSV(0, 1, 0.5f, 1, 0.25f, 1, 1, 1);
            return GetShape(shapeId,color,matId);
        }

    }
}