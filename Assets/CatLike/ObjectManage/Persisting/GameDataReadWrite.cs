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
    }
    
    
    public class GameDataWriter
    {
        private BinaryWriter mWriter;
        public GameDataWriter(BinaryWriter writer)
        {
            mWriter = writer;
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
    }
}