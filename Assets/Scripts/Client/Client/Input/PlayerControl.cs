using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        private PlayerMovement pm_;
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
    class PlayerMovement
    {

    }
}
