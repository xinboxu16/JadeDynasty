using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public enum SceneSubTypeEnum
    {
        TYPE_UNKNOWN = -1,
        TYPE_STORY = 0,
        TYPE_ELITE = 1,
        TYPE_EXPEDITION = 2,
    }
    public enum SceneTypeEnum
    {
        TYPE_UNKNOWN = -1,
        TYPE_PURE_CLIENT_SCENE = 0,
        TYPE_PVE = 1,
        TYPE_MULTI_PVE = 2,
        TYPE_PVP = 3,
        TYPE_SERVER_SELECT = 4,
        TYPE_NUM = 5,
    }

    //未实现
    public class SceneLogicView_General
    {
        public SceneLogicView_General()
        {
            //AbstractSceneLogic.OnSceneLogicSendStoryMessage += this.OnSceneLogicSendStoryMessage;
        }

        //public void OnSceneLogicSendStoryMessage(SceneLogicInfo info, string msgId, object[] args)
        //{
        //    if (WorldSystem.Instance.IsPveScene() || WorldSystem.Instance.IsPureClientScene())
        //    {
        //        ClientStorySystem.Instance.SendMessage(msgId, args);
        //    }
        //}
    }

    internal sealed class SceneLogicViewManager
    {
        private ArrayList m_Views = new ArrayList();

        private SceneLogicViewManager() { }

        public void Init()
        {
            //添加各个view实例
            m_Views.Add(new SceneLogicView_General());
        }

        public static SceneLogicViewManager Instance
        {
            get { return s_Instance; }
        }
        private static SceneLogicViewManager s_Instance = new SceneLogicViewManager();
    }
}
