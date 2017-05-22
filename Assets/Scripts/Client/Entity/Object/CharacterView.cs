using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class CharacterView
    {
        private int m_Actor = 0;
        private Dictionary<string, uint> effect_map_ = new Dictionary<string, uint>();

        protected void DestroyActor()
        {
            GfxSystem.DestroyGameObject(m_Actor);
            Release();
        }

        private void Release()
        {
            List<string> keyList = effect_map_.Keys.ToList();
            if (keyList != null && keyList.Count > 0)
            {
                foreach (string model in keyList)
                {
                    //DetachActor(model);
                }
            }
            CurWeaponList.Clear();
        }

        private List<string> m_CurWeaponName = new List<string>();

        public List<string> CurWeaponList
        {
            get
            {
                return m_CurWeaponName;
            }
        }

        private SharedGameObjectInfo m_ObjectInfo = new SharedGameObjectInfo();

        public SharedGameObjectInfo ObjectInfo
        {
            get { return m_ObjectInfo; }
        }

        public int Actor
        {
            get { return m_Actor; }
        }
    }
}
