using DashFire.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DashFire
{
    /*
    * TODO: 
    * 1. 由於多數操作依賴playerself的存在, 而playerself可能未創建或者已經刪除
    *    所以是否可以考慮外部輸入還是設計成可以與某個obj綁定?
    * 2. 組合鍵, 設定一組鍵的狀態同時彙報, 例如wasd的狀態希望能夠在一個囘調函數中得到通知
    *    (不適用主動查詢的情況下), 使用主動查詢就必須存在一個Tick函數
    *    
    */
    public class PlayerControl
    {
        private long m_LastTickTime = 0;
        private long m_LastTickIntervalMs = 0;
        private bool m_IsJoystickControl = false;
        private float m_lastMoveDir = -1f;
        private float m_lastDir = -1f;
        private bool m_LastTickIsMoving = false;
        private bool m_LastTickIsSkillMoving = false;
        private int m_lastSelectObjId = -1;

        private float c_2PI = (float)Math.PI * 2;
        private const float c_PiDiv8 = (float)Math.PI / 8;//22.5度
        private const float c_PiDiv16 = (float)Math.PI / 16;//11度

        private PlayerMovement pm_;

        private Vector2 old_mouse_pos_;
        private Vector2 mouse_pos_;

        public bool EnableMoveInput { get; set; }
        public bool EnableRotateInput { get; set; }
        public bool EnableSkillInput { get; set; }

        public PlayerControl()
        {
            pm_ = new PlayerMovement();
            EnableMoveInput = true;
            EnableRotateInput = true;
            EnableSkillInput = true;
        }

        public void Reset()
        {
            EnableMoveInput = true;
            EnableRotateInput = true;
            EnableSkillInput = true;
        }

        public void Init()
        {
            GfxSystem.ListenKeyPressState(
              Keyboard.Code.Z,
              Keyboard.Code.X,
              Keyboard.Code.Space,
              Keyboard.Code.Period,
              Keyboard.Code.W,
              Keyboard.Code.S,
              Keyboard.Code.A,
              Keyboard.Code.D,
              Keyboard.Code.P,
              Keyboard.Code.M,
              Keyboard.Code.F1,
              Keyboard.Code.B,
              Keyboard.Code.F2,
              Keyboard.Code.F3,
              Keyboard.Code.F4,
              Keyboard.Code.F6);
            GfxSystem.ListenKeyboardEvent(Keyboard.Code.F6, this.FillRage);
            GfxSystem.ListenKeyboardEvent(Keyboard.Code.F4, this.FillHp);
            GfxSystem.ListenKeyboardEvent(Keyboard.Code.F3, this.KillAll);
            GfxSystem.ListenKeyboardEvent(Keyboard.Code.F2, this.ToolPool);
            GfxSystem.ListenKeyboardEvent(Keyboard.Code.F1, this.DebugLog);
            GfxSystem.ListenKeyboardEvent(Keyboard.Code.B, this.BuyStamina);
            GfxSystem.ListenKeyboardEvent(Keyboard.Code.P, this.SwitchHero);
            GfxSystem.ListenKeyboardEvent(Keyboard.Code.Z, this.SwitchDebug);
            GfxSystem.ListenKeyboardEvent(Keyboard.Code.X, this.SwitchObserver);
            GfxSystem.ListenKeyboardEvent(Keyboard.Code.Space, this.InteractObject);
            GfxSystem.ListenKeyboardEvent(Keyboard.Code.Period, this.PrintPosition);
            GfxSystem.ListenTouchEvent(TouchEvent.Cesture, this.TouchHandle);
            GfxSystem.ListenKeyboardEvent(Keyboard.Code.M, this.OnPlaySkill);
        }

        public void Tick()
        {
            long now = TimeUtility.GetServerMilliseconds();
            m_LastTickIntervalMs = now - m_LastTickTime;
            m_LastTickTime = now;

            //观战
            if(WorldSystem.Instance.IsObserver && !WorldSystem.Instance.IsFollowObserver)
            {
                //TODO:未实现
                return;
            }

            UserInfo playerSelf = WorldSystem.Instance.GetPlayerSelf();//当前角色
            if (null == playerSelf)
                return;

            // if move input is disable
            // MotionStatus is MoveStop, and MotionChanged is reflect the change accordingly
            if(EnableMoveInput)
            {
                if(!IsKeyboardControl())
                {
                    CheckJoystickControl();
                }
            }

            if(!m_IsJoystickControl)
            {
                pm_.Update(EnableMoveInput);
            }

            MovementStateInfo msi = playerSelf.GetMovementStateInfo();

            Vector3 pos = msi.GetPosition3D();
            //LogSystem.Debug("PlayerControl Pos : {0}, Dir : {1}", pos.ToString(), playerSelf.GetMovementStateInfo().GetFaceDir());

            bool reface = false;
            if(m_LastTickIsSkillMoving && !msi.IsSkillMoving)
            {
                reface = true;
            }

            //操作同步机制改为发给服务器同时本地就开始执行（服务器转发给其它客户端，校验失败则同时发回原客户端进行位置调整）
            Vector3 mousePos = new Vector3(GfxSystem.GetMouseX(), GfxSystem.GetMouseY(), GfxSystem.GetMouseZ());
            if(pm_.MotionStatus == PlayerMovement.Motion.Moving || pm_.JoyStickMotionStatus == PlayerMovement.Motion.Moving)
            {
                if (pm_.MotionChanged || pm_.JoyStickMotionChanged || !m_LastTickIsMoving)
                {
                    playerSelf.SkillController.AddBreakSkillTask();//中断技能
                    //float moveDir = RoundMoveDir(pm_.MoveDir);//Round圆形的
                    float moveDir = pm_.MoveDir;

                    //GfxSystem.GfxLog("PlayerControl.Tick MoveDir:{0} RoundMoveDir:{1}", pm_.MoveDir, moveDir);

                    if(!m_LastTickIsMoving || !Geometry.IsSameFloat(moveDir, m_lastMoveDir))
                    {
                        msi.SetMoveDir(moveDir);
                        msi.IsMoving = true;
                        msi.TargetPosition = Vector3.zero;

                        if(WorldSystem.Instance.IsPvpScene() || WorldSystem.Instance.IsMultiPveScene())
                        {
                            NetworkSystem.Instance.SyncPlayerMoveStart(moveDir);//通知服务器移动
                        }
                    }

                    if (EnableRotateInput)
                    {
                        if(reface || !m_LastTickIsMoving || !Geometry.IsSameFloat(pm_.MoveDir, m_lastDir))
                        {
                            msi.SetFaceDir(pm_.MoveDir);
                            msi.SetWantFaceDir(pm_.MoveDir);

                            if (WorldSystem.Instance.IsPvpScene() || WorldSystem.Instance.IsMultiPveScene())
                            {
                                NetworkSystem.Instance.SyncFaceDirection(pm_.MoveDir);
                            }
                        }
                    }

                    m_lastDir = pm_.MoveDir;
                    m_lastMoveDir = moveDir;
                }
                m_LastTickIsMoving = true;
            }
            else
            {
                //停止移动
                if (m_LastTickIsMoving)
                {
                    playerSelf.SkillController.CancelBreakSkillTask();
                    playerSelf.GetMovementStateInfo().IsMoving = false;

                    if (WorldSystem.Instance.IsPvpScene() || WorldSystem.Instance.IsMultiPveScene())
                    {
                        NetworkSystem.Instance.SyncPlayerMoveStop();
                    }

                    if (EnableRotateInput)
                    {
                        if(reface)
                        {
                            msi.SetFaceDir(m_lastDir);
                            msi.SetWantFaceDir(m_lastDir);

                            if (WorldSystem.Instance.IsPvpScene() || WorldSystem.Instance.IsMultiPveScene())
                            {
                                NetworkSystem.Instance.SyncFaceDirection(m_lastDir);
                            }
                        }
                    }
                }
                m_LastTickIsMoving = false;
            }
            m_LastTickIsSkillMoving = msi.IsSkillMoving;

            old_mouse_pos_ = mouse_pos_;
            mouse_pos_.x = GfxSystem.GetMouseX();
            mouse_pos_.y = GfxSystem.GetMouseY();

            UserAiStateInfo aiInfo = playerSelf.GetAiStateInfo();
            if(null != aiInfo && (int)AiStateId.Idle == aiInfo.CurState)
            {
                m_lastSelectObjId = -1;
            }
        }

        private float RoundMoveDir(float dir)
        {
            int intDir = (int)(dir / c_PiDiv16);
            intDir = (intDir + 1) % 32;
            intDir /= 2;
            //if(dir > 0)
            //{
            //    Debug.Log("RoundMoveDir" + dir + " PiDiv " + (intDir * c_PiDiv8));
            //}
            return intDir * c_PiDiv8;
        }

        private bool IsKeyboardControl()
        {
            bool ret = false;
            if (GfxSystem.IsKeyPressed(Keyboard.Code.W)
                || GfxSystem.IsKeyPressed(Keyboard.Code.A)
                || GfxSystem.IsKeyPressed(Keyboard.Code.S)
                || GfxSystem.IsKeyPressed(Keyboard.Code.D))
            {
                ret = true;
            }
            return ret;
        }

        private void CheckJoystickControl()
        {
            UserInfo playerself = WorldSystem.Instance.GetPlayerSelf();
            if (null == playerself)
                return;
            float dir = GfxSystem.GetJoystickDir();
            if(dir < 0)
            {
                dir += c_2PI;
            }
            Vector3 targetPos = new Vector3(GfxSystem.GetJoystickTargetPosX(), GfxSystem.GetJoystickTargetPosY(), GfxSystem.GetJoystickTargetPosZ());
            UpdateMoveState(playerself, targetPos, dir);
            //Debug.Log("CheckJoystickControl_UpdateMoveState:" + dir);
        }

        private void UpdateMoveState(UserInfo playerSelf, Vector3 targetPos, float towards)
        {
            CharacterView view = EntityManager.Instance.GetUserViewById(playerSelf.GetId());
            if(null != view && view.ObjectInfo.IsGfxMoveControl && Geometry.IsSamePoint(Vector3.zero, targetPos))
            {
                LogSystem.Debug("UpdateMoveState IsGfxMoveControl : {0} , targetpos : {1}", view.ObjectInfo.IsGfxMoveControl, targetPos.ToString());
                return;
            }
            //Debug.Log("UpdateMoveState" + targetPos);
            PlayerMovement.Motion m = Geometry.IsSamePoint(Vector3.zero, targetPos) ? PlayerMovement.Motion.Stop : PlayerMovement.Motion.Moving;
            pm_.JoyStickMotionChanged = pm_.JoyStickMotionStatus != m || !Geometry.IsSameFloat(m_lastDir, towards);
            pm_.JoyStickMotionStatus = m;
            pm_.MoveDir = towards;
            if(Geometry.IsSamePoint(Vector3.zero, targetPos))
            {
                pm_.JoyStickMotionStatus = PlayerMovement.Motion.Stop;
                m_IsJoystickControl = false;
            }
            else
            {
                m_IsJoystickControl = true;
            }
        }

        //填充 怒气值
        private void FillRage(int key_code, int what)
        {

        }

        //填充 血值
        private void FillHp(int key_code, int what)
        {

        }

        private void KillAll(int key_code, int what)
        {
 
        }

        private void ToolPool(int key_code, int what)
        {

        }

        private void DebugLog(int key_code, int what)
        {
            
        }

        //购买体力
        private void BuyStamina(int key_code, int what)
        {

        }

        private void SwitchHero(int key_code, int what)
        {

        }

        private void SwitchDebug(int key_code, int what)
        {

        }

        private void SwitchObserver(int key_code, int what)
        {

        }

        //相互作用
        private void InteractObject(int key_code, int what)
        {

        }

        private void PrintPosition(int key_code, int what)
        {

        }

        private void TouchHandle(int what, GestureArgs e)
        {

        }

        private void OnPlaySkill(int key_code, int what)
        {

        }

        #region Sington
        private static PlayerControl inst_ = new PlayerControl();
        public static PlayerControl Instance 
        { 
            get { return inst_; }
        }
        #endregion
    }

    //未实现
    public class PlayerMovement
    {
        public enum Motion
        {
            Moving,
            Stop,
        }

        public enum KeyHit
        {
            None = 0,
            Up = 1,
            Down = 2,
            Left = 4,
            Right = 8,
            Other = 16,
        }

        private enum KeyIndex
        {
            W = 0,
            A,
            S,
            D
        }

        public KeyHit last_key_hit_;
        private static readonly Keyboard.Code[] s_Normal = new Keyboard.Code[] { Keyboard.Code.W, Keyboard.Code.A, Keyboard.Code.S, Keyboard.Code.D };

        public float MoveDir { get; set; }
        public Motion MotionStatus { get; set; }
        public bool MotionChanged { get; set; }
        public Motion JoyStickMotionStatus { get; set; }
        public bool JoyStickMotionChanged { get; set; }

        public PlayerMovement()
        {
            MoveDir = 0;
            MotionStatus = Motion.Stop;
            MotionChanged = false;
            JoyStickMotionStatus = Motion.Stop;
            JoyStickMotionChanged = false;
            last_key_hit_ = KeyHit.None;
        }

        public void Update(bool moveEnable)
        {
            UserInfo playerSelf = WorldSystem.Instance.GetPlayerSelf();
            if(null == playerSelf || playerSelf.IsDead())
            {
                return;
            }
            KeyHit kh = KeyHit.None;
            if(moveEnable)
            {
                //TODO:不懂
                if(DashFireSpatial.SpatialObjType.kNPC == playerSelf.GetRealControlledObject().SpaceObject.GetObjType())
                {
                    NpcInfo npcInfo = playerSelf.GetRealControlledObject().CastNpcInfo();
                    if (null != npcInfo)
                    {
                        if(!npcInfo.CanMove)
                        {
                            return;
                        }
                    }
                }
                if (GfxSystem.IsKeyPressed(GetKeyCode(KeyIndex.W)))
                    kh |= KeyHit.Up;
                if (GfxSystem.IsKeyPressed(GetKeyCode(KeyIndex.A)))
                    kh |= KeyHit.Left;
                if (GfxSystem.IsKeyPressed(GetKeyCode(KeyIndex.S)))
                    kh |= KeyHit.Down;
                if (GfxSystem.IsKeyPressed(GetKeyCode(KeyIndex.D)))
                    kh |= KeyHit.Right;
            }

            Motion m = kh == KeyHit.None ? Motion.Stop : Motion.Moving;
            MotionChanged = MotionStatus != m || last_key_hit_ != kh;

            if(MotionChanged)
            {
                GfxSystem.GfxLog("MotionChanged:{0}!={1} || {2}!={3}", MotionStatus, m, last_key_hit_, kh);
            }

            last_key_hit_ = kh;
            MotionStatus = m;
            MoveDir = CalcMoveDir(kh);
            if (MoveDir < 0)
            {
                MotionStatus = Motion.Stop;
            }
            if (MotionChanged)
            {
                GfxSystem.GfxLog("InputMoveDir:{0} Pos:{1}", MoveDir, playerSelf.GetMovementStateInfo().GetPosition3D().ToString());
            }
        }

        /**
          * @brief 计算移动方向
          *           0       
          *          /|\
          *           |
          *3π/2 -----*-----> π/2
          *           |
          *           |
          *           |
          *           π
          */
        private static readonly float[] s_MoveDirs = new float[] { -1,  0, (float)Math.PI, -1, 3*(float)Math.PI/2, 7*(float)Math.PI/4, 5*(float)Math.PI/4, 
      //                    UDL          R          UR         DR           UDR        LR  ULR  LRD      UDLR
                            3*(float)Math.PI/2, (float)Math.PI/2, (float)Math.PI/4, 3*(float)Math.PI/4, (float)Math.PI/2, -1, 0,   (float)Math.PI, -1 };

        private float CalcMoveDir(KeyHit kh)
        {
            return s_MoveDirs[(int)kh];
        }

        private Keyboard.Code GetKeyCode(KeyIndex index)
        {
            Keyboard.Code ret = Keyboard.Code.W;
            if (index >= KeyIndex.W && index <= KeyIndex.D)
            {
                Keyboard.Code[] list = s_Normal;
                ret = list[(int)index];
            }
            return ret;
        }
    }
}
