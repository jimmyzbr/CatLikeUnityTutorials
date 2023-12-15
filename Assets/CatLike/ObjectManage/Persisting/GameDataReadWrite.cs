using System.IO;
using UnityEngine;

namespace ObjectManagerDemo
{
    public class GameDataReader
    {
        private BinaryReader mReader;
        public GameDataReader(BinaryReader reader)
        {
            mReader = reader;
        }

        public int ReadInt()
        {
            return mReader.ReadInt32();
        }
        
        public float ReadFloat()
        {
            return mReader.ReadSingle();
        }

        public Vector3 ReadVec3()
        {
            Vector3 v3;
            v3.x = mReader.ReadSingle();
            v3.y = mReader.ReadSingle();
            v3.z = mReader.ReadSingle();
            return v3;
        }

        public Quaternion ReadQuat()
        {
            Quaternion q;
            q.x = mReader.ReadSingle();
            q.y = mReader.ReadSingle();
            q.z = mReader.ReadSingle();
            q.w = mReader.ReadSingle();

            return q;
        }

        public Color ReadColor()
        {
            Color col;
            col.r = mReader.ReadByte() / 255.0f;
            col.g = mReader.ReadByte()  / 255.0f;
            col.b = mReader.ReadByte() / 255.0f;
            col.a = mReader.ReadByte() / 255.0f;

            return col;
        }
    }
    
    
    public class GameDataWriter
    {
        private BinaryWriter mWriter;
        public GameDataWriter(BinaryWriter writer,int saveVersion)
        {
            mWriter = writer;
            WriteInt(-saveVersion);
        }

        public void WriteInt(int v)
        {
             mWriter.Write(v);
        }

        public void WriteVec3(Vector3 v3)
        {
            mWriter.Write(v3.x);
            mWriter.Write(v3.y);
            mWriter.Write(v3.z);
        }

        public void WriteQuat(Quaternion q)
        {
            mWriter.Write(q.x);
            mWriter.Write(q.y);
            mWriter.Write(q.z);
            mWriter.Write(q.w);
        }

        public void WriteColor(Color col)
        {
            mWriter.Write((byte)(col.r * 255.0f));
            mWriter.Write((byte)(col.g * 255.0f));
            mWriter.Write((byte)(col.b * 255.0f));
            mWriter.Write((byte)(col.a * 255.0f));
        }
    }
}