using UnityEngine;

namespace ObjectManagerDemo
{
    public class MyShape : PersistableObject
    {
 
        private int mShapeId = -1;

        public int ShapeId
        {
            get => mShapeId;
            set
            {
                if (mShapeId == -1 && value != -1)
                {
                    mShapeId = value;
                }
                else
                {
                    Debug.LogError("Not Allowed to Change ShapeId");
                }
            }
        }



        private int mMaterialId;

        public int MaterialId => mMaterialId;

        private Color mColor;

        public Color Color => mColor;

        public void SetMaterial(Material mat, int matId)
        {
            this.mMaterialId = matId;
            GetComponent<MeshRenderer>().material = mat;
        }

        
        /// <summary>
        /// 使用共享的mpb
        /// </summary>
        private static MaterialPropertyBlock s_mpb;
        static int colorPropertyId = Shader.PropertyToID("_Color");

        public void SetColor(Color color)
        {
            mColor = color;
            //GetComponent<MeshRenderer>().material.color = color;
            
            //使用MaterialPropertyBlock 设置颜色，避免生成新的materail对象

            if (s_mpb == null)
            {
                s_mpb = new MaterialPropertyBlock();
            }
            s_mpb.SetColor(colorPropertyId,color);
            GetComponent<MeshRenderer>().SetPropertyBlock(s_mpb);

        }


        public override void Save(GameDataWriter writer)
        {
            writer.WriteInt(this.ShapeId);
            writer.WriteInt(this.MaterialId);
            writer.WriteColor(this.Color);
            base.Save(writer);
        }


        public override void Load(GameDataReader reader)
        {
            base.Load(reader);
        }
    }
}