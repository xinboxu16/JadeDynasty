using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public sealed class ControlSystemOperation
    {
        private static ControlSystemHelper s_Helper = new ControlSystemHelper();

        public static void Reset()
        {
            s_Helper.System.Reset();
        }

        private sealed class ControlSystemHelper
        {
            private ControlSystem m_ControlSystem = new ControlSystem();
            private ObjectPool<FaceDirController> m_FaceControllerPool = new ObjectPool<FaceDirController>();

            internal ControlSystem System
            {
                get { return m_ControlSystem; }
            }

            internal ObjectPool<FaceDirController> FaceControllerPool
            {
                get { return m_FaceControllerPool; }
            }
        }

        public static void Tick()
        {
            s_Helper.System.Tick();
        }

        public static void AdjustCharacterFaceDir(int id, float faceDir)
        {
            const float c_PI = (float)Math.PI;
            const float c_2PI = (float)Math.PI * 2;
            CharacterInfo info = WorldSystem.Instance.GetCharacterById(id);
            if (null != info)
            {
                //这样就可以旋转 但是ControllerIdCalculator是什么作用
                //info.GetMovementStateInfo().SetFaceDir(faceDir);
                //当前面向的弧度
                float curFaceDir = info.GetMovementStateInfo().GetFaceDir();
                float deltaDir = ((faceDir + c_2PI) - curFaceDir) % c_2PI;
                if (deltaDir > c_PI)
                {
                    deltaDir = c_2PI - deltaDir;
                }
                if (deltaDir > 0.1f)
                {
                    int ctrlId = ControllerIdCalculator.Calc(ControllerType.FaceDir, id);
                    FaceDirController ctrl = s_Helper.FaceControllerPool.Alloc();
                    if (null != ctrl)
                    {
                        ctrl.Init(ctrlId, id, faceDir);
                        s_Helper.System.AddController(ctrl);
                    }
                }
                else
                {
                    info.GetMovementStateInfo().SetFaceDir(faceDir);
                }
            }
        }
    }
}
