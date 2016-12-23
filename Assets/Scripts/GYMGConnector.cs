using UnityEngine;
using System.Collections;
using DashFire;
public class GYMGConnector : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //切换场景保存gameobject
        DontDestroyOnLoad(this.gameObject);
        CYMGWrapperIOS.InitCYMG(true);
	}

    //获取设备唯一标识符
    void OnGetUniqueIdentifier(string msg)
    {
        Debug.LogError("GetUniqueIdentifier Success. msg:" + msg);
        LogicSystem.PublishLogicEvent("ge_device_info", "lobby", msg);
    }
    //账号登录CYMG成功的回调处理方法
    void OnAccountLoginCYMGSuccess(string msg)
    {
        Debug.LogError("Account login CYMG Success. msg:" + msg);
        LogicSystem.EventChannelForGfx.Publish("ge_ui_connect_hint", "ui", true, true);
        LogicSystem.PublishLogicEvent("ge_account_login", "lobby", msg);
    }
    //请求商品列表的回调处理方法
    void OnRequestGoodsList(string msg)
    {
        Debug.LogError("Request GoodsList. msg:" + msg);
        LogicSystem.PublishLogicEvent("ge_goods_list", "lobby", msg);
    }
    //充值支付的回调处理方法
    void OnPaymentResult(string msg)
    {
        Debug.LogError("Payment Finish. msg:" + msg);
        //LogicSystem.PublishLogicEvent("ge_goods_list", "lobby", msg);
    }
}
