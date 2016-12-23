using StorySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public sealed class ClientStorySystem
    {
        public void Init()
        {
            //注册剧情命令
            //StoryCommandManager.Instance.RegisterCommandFactory("startstory", new StoryCommandFactoryHelper<Story.Commands.StartStoryCommand>());
        }

        #region Sington
        public static ClientStorySystem Instance
        {
            get
            {
                return s_Instance;
            }
        }
        private static ClientStorySystem s_Instance = new ClientStorySystem();
        #endregion
    }
}
