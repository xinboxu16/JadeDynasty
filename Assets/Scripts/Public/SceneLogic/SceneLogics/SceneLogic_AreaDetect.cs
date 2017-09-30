using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public class SceneLogic_AreaDetect : AbstractSceneLogic
    {
        public override void Execute(SceneLogicInfo info, long deltaTime)
        {
            if (null == info || info.IsLogicFinished) return;
            info.Time += deltaTime;
            if (info.Time > 100)
            {
                AreaDetectLogicInfo data = info.LogicDatas.GetData<AreaDetectLogicInfo>();
                if (null == data)
                {
                    data = new AreaDetectLogicInfo();
                    info.LogicDatas.AddData<AreaDetectLogicInfo>(data);

                    SceneLogicConfig sc = info.SceneLogicConfig;
                    if (null != sc)
                    {
                        List<float> pts = Converter.ConvertNumericList<float>(sc.m_Params[0]);
                        data.m_Area = new Vector3[pts.Count / 2];
                        for (int ix = 0; ix < pts.Count - 1; ix += 2)
                        {
                            data.m_Area[ix / 2].x = pts[ix];
                            data.m_Area[ix / 2].z = pts[ix + 1];
                        }
                        data.m_TriggerType = (AreaDetectLogicInfo.TiggerTypeEnum)int.Parse(sc.m_Params[1]);
                        data.m_Timeout = long.Parse(sc.m_Params[2]);
                    }
                }
                data.m_CurTime += info.Time;
                info.Time = 0;
                //执行逻辑
                if (null != data && !data.m_IsTriggered && data.m_CurTime >= data.m_Timeout)
                {
                    data.m_IsTriggered = true;
                    if (data.m_TriggerType == AreaDetectLogicInfo.TiggerTypeEnum.All)
                    {
                        ArrayList list = new ArrayList();
                        info.SpatialSystem.VisitObjectInPolygon(data.m_Area, (float distSqr, DashFireSpatial.ISpaceObject obj) =>
                        {
                            list.Add((int)obj.GetID());
                        });
                        OnSceneLogicSendStoryMessage(info, "areadetect:" + info.ConfigId, list.ToArray());
                    }
                    else if (data.m_TriggerType == AreaDetectLogicInfo.TiggerTypeEnum.Npc)
                    {
                        ArrayList list = new ArrayList();
                        info.SpatialSystem.VisitObjectInPolygon(data.m_Area, (float distSqr, DashFireSpatial.ISpaceObject obj) =>
                        {
                            if (obj.GetObjType() == DashFireSpatial.SpatialObjType.kNPC)
                            {
                                list.Add((int)obj.GetID());
                            }
                        });
                        OnSceneLogicSendStoryMessage(info, "areadetect:" + info.ConfigId, list.ToArray());
                    }
                    else if (data.m_TriggerType == AreaDetectLogicInfo.TiggerTypeEnum.User)
                    {
                        ArrayList list = new ArrayList();
                        info.SpatialSystem.VisitObjectInPolygon(data.m_Area, (float distSqr, DashFireSpatial.ISpaceObject obj) =>
                        {
                            if (obj.GetObjType() == DashFireSpatial.SpatialObjType.kUser)
                            {
                                list.Add((int)obj.GetID());
                            }
                        });
                        OnSceneLogicSendStoryMessage(info, "areadetect:" + info.ConfigId, list.ToArray());
                    }
                }
            }
        }
    }
}
