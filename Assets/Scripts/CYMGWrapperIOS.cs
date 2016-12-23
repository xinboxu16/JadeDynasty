using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

/**静态类 不能使用 new 关键字创建静态类的实例；
    仅包含静态成员；
    不能被实例化； 
    密封的，不能被继承；
    不能包含实例构造函数，但可以包含静态构造函数:static CYMGWrapperIOS()
    静态构造函数无访问修饰符、无参数，只有一个 static 标志；
    静态构造函数不可被直接调用，当创建类实例或引用任何静态成员之前，静态构造函数被自动执行，并且只执行一次。
 **/
public static class CYMGWrapperIOS
{
    [DllImport("__Internal")]
    private static extern void _InitCYMG(int isDebug);
    [DllImport("__Internal")]
    private static extern void _StartLogin(int isAuto);
    [DllImport("__Internal")]
    private static extern void _OnLoginBillingSuccess(string accountId);
    [DllImport("__Internal")]
    private static extern void _StartLogout();
    [DllImport("__Internal")]
    private static extern void _ShowCallCenter();
    [DllImport("__Internal")]
    private static extern void _MBIOnLogin(string account, string server, string roleName, string roleId, int level, string userId);
    [DllImport("__Internal")]
    private static extern void _GetUniqueIdentifier();

    //初始化畅游平台OpenSDK
    public static void InitCYMG(bool isDebug)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            int flag = 0;
            if (isDebug)
            {
                flag = 1;
            }
            _InitCYMG(flag);
        }
    }

    //账号登录
    public static void StartLogin(bool isAuto)
    {
        if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            int flag = 0;
            if (isAuto)
            {
                flag = 1;
            }
            _StartLogin(flag);
        }
    }

    //账号登录成功，服务器验证通过
    public static void OnLoginBillingSuccess(string accountId)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _OnLoginBillingSuccess(accountId);
        }
    }

    //账号注销
    public static void StartLogout()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _StartLogout();
        }
    }

    //显示客服中心
    public static void ShowCallCenter()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _ShowCallCenter();
        }
    }

    //设备标示符
    public static void GetUniqueIdentifier()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _GetUniqueIdentifier();
        }
    }

    //MBI 玩家角色进入游戏日志
    public static void MBIOnLogin(string account, string server, string roleName, string roleId, int level, string userId)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _MBIOnLogin(account, server, roleName, roleId, level, userId);
        }
    }
}