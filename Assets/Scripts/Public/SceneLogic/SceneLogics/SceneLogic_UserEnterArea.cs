using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    public class SceneLogic_UserEnterArea : AbstractSceneLogic
    {
        public override void Execute(SceneLogicInfo info, long deltaTime)
        {
            if (null == info || info.IsLogicFinished) return;
            info.Time += deltaTime;
            if (info.Time > 100)
            {
                /*
                 * 一个场景有多个UserEnterAreaLogicInfo对象 通过m_IsTriggered确定是否要执行哪个多边形区域
                 */
                UserEnterAreaLogicInfo data = info.LogicDatas.GetData<UserEnterAreaLogicInfo>();
                if (null == data)
                {
                    data = new UserEnterAreaLogicInfo();
                    info.LogicDatas.AddData<UserEnterAreaLogicInfo>(data);
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
                        data.m_TriggerType = (UserEnterAreaLogicInfo.TiggerTypeEnum)int.Parse(sc.m_Params[1]);
                    }
                }
                info.Time = 0;
                //执行逻辑
                if (null != data && !data.m_IsTriggered)
                {
                    if (data.m_TriggerType == UserEnterAreaLogicInfo.TiggerTypeEnum.All)
                    {
                        int ct = 0;
                        info.SpatialSystem.VisitObjectInPolygon(data.m_Area, (float distSqr, DashFireSpatial.ISpaceObject obj) =>
                        {
                            if (obj.GetObjType() == DashFireSpatial.SpatialObjType.kUser)
                            {
                                ++ct;
                            }
                        });

                        if (ct == info.UserManager.Users.Count)
                        {
                            SceneLogicSendStoryMessage(info, "alluserenterarea:" + info.ConfigId, ct);
                            data.m_IsTriggered = true;
                        }
                    }
                    else
                    {
                        //判断是否在多边形内
                        int id = 0;
                        info.SpatialSystem.VisitObjectInPolygon(data.m_Area, (float distSqr, DashFireSpatial.ISpaceObject obj) =>
                        {
                            if (obj.GetObjType() == DashFireSpatial.SpatialObjType.kUser)
                            {
                                id = (int)obj.GetID();
                                return false;
                            }
                            return true;
                        });
                        if (id > 0)
                        {
                            SceneLogicSendStoryMessage(info, "anyuserenterarea:" + info.ConfigId, id);//SceneLogicView_General->OnSceneLogicSendStoryMessage->ClientStorySystem:SendMessage
                            data.m_IsTriggered = true;
                        }
                    }
                }
            }
        }
    }
}
