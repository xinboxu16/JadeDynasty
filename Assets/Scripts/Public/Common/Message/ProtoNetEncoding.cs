using System;
using System.IO;
using System.Collections.Generic;

namespace DashFire.Network
{
  public sealed class ProtoNetEncoding
  {
    public byte[] Encode(object msg)
    {
      m_DataStream.SetLength(0);
      m_Serializer.Serialize(m_DataStream, msg);
      byte[] ret = new byte[m_DataStream.Length];
      m_DataStream.Position = 0;
      m_DataStream.Read(ret, 0, ret.Length);

      //LogSystem.Info("encode message:id {0} len({1})[{2}]", id, ret.Length - 2, jsonData.GetType().Name);
      return ret;
    }
    public object Decode(Type t, byte[] msgbuf)
    {
      m_DataStream.SetLength(0);
      m_DataStream.Write(msgbuf, 0, msgbuf.Length);
      m_DataStream.Position = 0;
      try {
        object msg = m_Serializer.Deserialize(m_DataStream, null, t);
        if (msg == null) {
          LogSystem.Debug("decode message error:can't find {0} len({1}) !!!", t.Name, msgbuf.Length);
          return null;
        }
        //LogSystem.Info("decode message:id {0} len({1})[{2}]", id, msgbuf.Length - 2, jsonData.GetType().Name);
        return msg;
      } catch (Exception ex) {
        LogSystem.Error("decode message error:{0} len({1}) {2}\n{3}\nData:\n{4}", t.Name, msgbuf.Length, ex.Message, ex.StackTrace, Helper.BinToHex(msgbuf));
        throw ex;
      }
    }

    private ProtobufSerializer m_Serializer = new ProtobufSerializer();
    private MemoryStream m_DataStream = new MemoryStream(4096);
  }
}
