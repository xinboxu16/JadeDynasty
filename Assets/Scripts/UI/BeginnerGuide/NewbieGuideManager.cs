using DashFire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewbieGuideManager : MonoBehaviour
{
    private object eo = null;
    private NewbieGuideManager ngm = null;
    private Transform uiRootTF = null;

    void Start()
    {
        eo = LogicSystem.EventChannelForGfx.Subscribe("ge_ui_unsubscribe", "ui", UnSubscribe);
    }

    private void UnSubscribe()
    {
        try
        {
            if (eo != null)
            {
                DashFire.LogicSystem.EventChannelForGfx.Unsubscribe(eo);
            }
            //GuidEnd();引导结束 未实现
            if (ngm != null)
            {
                NGUITools.DestroyImmediate(ngm);
            }
        }
        catch (System.Exception ex)
        {
            DashFire.LogicSystem.LogicLog("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    public void SetMySelf(NewbieGuideManager myself, Transform tf)
    {
        ngm = myself;
        uiRootTF = tf;
    }
}
