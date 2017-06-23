using DashFire;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DashFireSpatial
{
    public class MapPatchParser
    {
        private class ObstacleInfo
        {
            public byte m_OldValue;
            public byte m_UpdatedValue;

            public ObstacleInfo(byte oldValue, byte updatedValue)
            {
                m_OldValue = oldValue;
                m_UpdatedValue = updatedValue;
            }
        }

        private const int c_RecordSize = sizeof(short) + sizeof(short) + sizeof(byte) * 2;

        private SortedDictionary<int, ObstacleInfo> m_PatchDatas = new SortedDictionary<int, ObstacleInfo>();

        public void Load(string filename)
        {
             if (!FileReaderProxy.Exists(filename))
             {
                 return;
             }
             try
             {
                 using (MemoryStream ms = FileReaderProxy.ReadFileAsMemoryStream(filename))
                 {
                     using (BinaryReader br = new BinaryReader(ms))
                     {
                         while(ms.Position <= ms.Length - c_RecordSize)
                         {
                             short row = br.ReadInt16();
                             short col = br.ReadInt16();
                             byte obstacle = br.ReadByte();
                             byte oldObstacle = br.ReadByte();

                             Update(row, col, obstacle, oldObstacle);
                         }
                         br.Close();
                     }
                     ms.Close();
                 }
             }catch(Exception ex)
             {
                 LogSystem.Error("{0}\n{1}", ex.Message, ex.StackTrace);
             }
        }

        public void Update(int row, int col, byte obstacle, byte oldObstacle)
        {
            if(row >= 10000 || col >= 10000)
            {
                throw new Exception("Can't support huge map (row>=10000 || col>=10000)");
            }
            int key = EncodeKey(row, col);
            if (m_PatchDatas.ContainsKey(key))
            {
                m_PatchDatas[key].m_OldValue = oldObstacle;
                m_PatchDatas[key].m_UpdatedValue = obstacle;
            }
            else
            {
                m_PatchDatas.Add(key, new ObstacleInfo(oldObstacle, obstacle));
            }
        }

        public void VisitPatches(MyAction<int, int, byte> visitor)
        {
            foreach(KeyValuePair<int, ObstacleInfo> pair in m_PatchDatas)
            {
                byte obstacle = pair.Value.m_UpdatedValue;
                byte oldObstacle = pair.Value.m_OldValue;
                if(obstacle != oldObstacle)
                {
                    int key = pair.Key;
                    int row, col;
                    DecodeKey(key, out row, out col);
                    visitor(row, col, obstacle);
                }
            }
        }

        private int EncodeKey(int row, int col)
        {
            return row * 10000 + col;
        }

        private void DecodeKey(int key, out int row, out int col)
        {
            row = key / 10000;
            col = key % 10000;
        }
    }
}
