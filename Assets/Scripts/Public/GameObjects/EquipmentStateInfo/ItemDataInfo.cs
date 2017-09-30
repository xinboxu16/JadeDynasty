using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class ItemDataInfo
    {
        private int m_ItemId = 0;
        private int m_ItemNum = 0;
        private int m_Level = 1;
        private int m_RandomProperty = 0;
        private bool m_IsUnlock = true;
        private ItemConfig m_ItemConfig = null;

        public int ItemId
        {
            get
            {
                if (0 == m_ItemId)
                {
                    if (null != m_ItemConfig)
                        m_ItemId = m_ItemConfig.m_ItemId;
                }
                return m_ItemId;
            }
            set { m_ItemId = value; }
        }

        public ItemConfig ItemConfig
        {
            get { return m_ItemConfig; }
            set { m_ItemConfig = value; }
        }
        public int Level
        {
            get { return m_Level; }
            set { m_Level = value; }
        }
        public int RandomProperty
        {
            get { return m_RandomProperty; }
            set { m_RandomProperty = value; }
        }

        public bool IsUnlock
        {
            get { return m_IsUnlock; }
            set { m_IsUnlock = value; }
        }

        public int ItemNum
        {
            get { return m_ItemNum; }
            set { m_ItemNum = value; }
        }

        //修改属性
        #region
        public float GetAddHpMax(float refVal, int refLevel)
        {
            float ret = 0;
            if (null != m_ItemConfig)
            {
                ret += m_ItemConfig.m_AttrData.GetAddHpMax(refVal, refLevel, m_Level);
                if (m_RandomProperty > 0)
                {
                    AppendAttributeConfig data = AppendAttributeConfigProvider.Instance.GetDataById(m_RandomProperty);
                    if (null != data)
                    {
                        ret += data.GetAddHpMax(refVal, refLevel);
                    }
                }
            }
            return ret;
        }
        public float GetAddEpMax(float refVal, int refLevel)
        {
            float ret = 0;
            if (null != m_ItemConfig)
            {
                ret += m_ItemConfig.m_AttrData.GetAddEpMax(refVal, refLevel, m_Level);
                if (m_RandomProperty > 0)
                {
                    AppendAttributeConfig data = AppendAttributeConfigProvider.Instance.GetDataById(m_RandomProperty);
                    if (null != data)
                    {
                        ret += data.GetAddEpMax(refVal, refLevel);
                    }
                }
            }
            return ret;
        }
        public float GetAddAd(float refVal, int refLevel)
        {
            float ret = 0;
            if (null != m_ItemConfig)
            {
                ret += m_ItemConfig.m_AttrData.GetAddAd(refVal, refLevel, m_Level);
                if (m_RandomProperty > 0)
                {
                    AppendAttributeConfig data = AppendAttributeConfigProvider.Instance.GetDataById(m_RandomProperty);
                    if (null != data)
                    {
                        ret += data.GetAddAd(refVal, refLevel);
                    }
                }
            }
            return ret;
        }
        public float GetAddADp(float refVal, int refLevel)
        {
            float ret = 0;
            if (null != m_ItemConfig)
            {
                ret += m_ItemConfig.m_AttrData.GetAddADp(refVal, refLevel, m_Level);
                if (m_RandomProperty > 0)
                {
                    AppendAttributeConfig data = AppendAttributeConfigProvider.Instance.GetDataById(m_RandomProperty);
                    if (null != data)
                    {
                        ret += data.GetAddADp(refVal, refLevel);
                    }
                }
            }
            return ret;
        }
        public float GetAddMDp(float refVal, int refLevel)
        {
            float ret = 0;
            if (null != m_ItemConfig)
            {
                ret += m_ItemConfig.m_AttrData.GetAddMDp(refVal, refLevel, m_Level);
                if (m_RandomProperty > 0)
                {
                    AppendAttributeConfig data = AppendAttributeConfigProvider.Instance.GetDataById(m_RandomProperty);
                    if (null != data)
                    {
                        ret += data.GetAddMDp(refVal, refLevel);
                    }
                }
            }
            return ret;
        }
        public float GetAddCri(float refVal, int refLevel)
        {
            float ret = 0;
            if (null != m_ItemConfig)
            {
                ret += m_ItemConfig.m_AttrData.GetAddCri(refVal, refLevel);
                if (m_RandomProperty > 0)
                {
                    AppendAttributeConfig data = AppendAttributeConfigProvider.Instance.GetDataById(m_RandomProperty);
                    if (null != data)
                    {
                        ret += data.GetAddCri(refVal, refLevel);
                    }
                }
            }
            return ret;
        }
        public float GetAddPow(float refVal, int refLevel)
        {
            float ret = 0;
            if (null != m_ItemConfig)
            {
                ret += m_ItemConfig.m_AttrData.GetAddPow(refVal, refLevel);
                if (m_RandomProperty > 0)
                {
                    AppendAttributeConfig data = AppendAttributeConfigProvider.Instance.GetDataById(m_RandomProperty);
                    if (null != data)
                    {
                        ret += data.GetAddPow(refVal, refLevel);
                    }
                }
            }
            return ret;
        }
        public float GetAddBackHitPow(float refVal, int refLevel)
        {
            float ret = 0;
            if (null != m_ItemConfig)
            {
                ret += m_ItemConfig.m_AttrData.GetAddBackHitPow(refVal, refLevel);
                if (m_RandomProperty > 0)
                {
                    AppendAttributeConfig data = AppendAttributeConfigProvider.Instance.GetDataById(m_RandomProperty);
                    if (null != data)
                    {
                        ret += data.GetAddBackHitPow(refVal, refLevel);
                    }
                }
            }
            return ret;
        }
        public float GetAddCrackPow(float refVal, int refLevel)
        {
            float ret = 0;
            if (null != m_ItemConfig)
            {
                ret += m_ItemConfig.m_AttrData.GetAddCrackPow(refVal, refLevel);
                if (m_RandomProperty > 0)
                {
                    AppendAttributeConfig data = AppendAttributeConfigProvider.Instance.GetDataById(m_RandomProperty);
                    if (null != data)
                    {
                        ret += data.GetAddCrackPow(refVal, refLevel);
                    }
                }
            }
            return ret;
        }
        public float GetAddHpRecover(float refVal, int refLevel)
        {
            float ret = 0;
            if (null != m_ItemConfig)
            {
                ret += m_ItemConfig.m_AttrData.GetAddHpRecover(refVal, refLevel);
                if (m_RandomProperty > 0)
                {
                    AppendAttributeConfig data = AppendAttributeConfigProvider.Instance.GetDataById(m_RandomProperty);
                    if (null != data)
                    {
                        ret += data.GetAddHpRecover(refVal, refLevel);
                    }
                }
            }
            return ret;
        }
        public float GetAddEpRecover(float refVal, int refLevel)
        {
            float ret = 0;
            if (null != m_ItemConfig)
            {
                ret += m_ItemConfig.m_AttrData.GetAddEpRecover(refVal, refLevel);
                if (m_RandomProperty > 0)
                {
                    AppendAttributeConfig data = AppendAttributeConfigProvider.Instance.GetDataById(m_RandomProperty);
                    if (null != data)
                    {
                        ret += data.GetAddEpRecover(refVal, refLevel);
                    }
                }
            }
            return ret;
        }

        public float GetAddSpd(float refVal, int refLevel)
        {
            float ret = 0;
            if (null != m_ItemConfig)
            {
                ret += m_ItemConfig.m_AttrData.GetAddSpd(refVal, refLevel);
                if (m_RandomProperty > 0)
                {
                    AppendAttributeConfig data = AppendAttributeConfigProvider.Instance.GetDataById(m_RandomProperty);
                    if (null != data)
                    {
                        ret += data.GetAddSpd(refVal, refLevel);
                    }
                }
            }
            return ret;
        }
        public float GetAddFireDam(float refVal, int refLevel)
        {
            float ret = 0;
            if (null != m_ItemConfig)
            {
                ret += m_ItemConfig.m_AttrData.GetAddFireDam(refVal, refLevel);
                if (m_RandomProperty > 0)
                {
                    AppendAttributeConfig data = AppendAttributeConfigProvider.Instance.GetDataById(m_RandomProperty);
                    if (null != data)
                    {
                        ret += data.GetAddFireDam(refVal, refLevel);
                    }
                }
            }
            return ret;
        }
        public float GetAddFireErd(float refVal, int refLevel)
        {
            float ret = 0;
            if (null != m_ItemConfig)
            {
                ret += m_ItemConfig.m_AttrData.GetAddFireErd(refVal, refLevel);
                if (m_RandomProperty > 0)
                {
                    AppendAttributeConfig data = AppendAttributeConfigProvider.Instance.GetDataById(m_RandomProperty);
                    if (null != data)
                    {
                        ret += data.GetAddFireErd(refVal, refLevel);
                    }
                }
            }
            return ret;
        }
        public float GetAddIceDam(float refVal, int refLevel)
        {
            float ret = 0;
            if (null != m_ItemConfig)
            {
                ret += m_ItemConfig.m_AttrData.GetAddIceDam(refVal, refLevel);
                if (m_RandomProperty > 0)
                {
                    AppendAttributeConfig data = AppendAttributeConfigProvider.Instance.GetDataById(m_RandomProperty);
                    if (null != data)
                    {
                        ret += data.GetAddIceDam(refVal, refLevel);
                    }
                }
            }
            return ret;
        }
        public float GetAddIceErd(float refVal, int refLevel)
        {
            float ret = 0;
            if (null != m_ItemConfig)
            {
                ret += m_ItemConfig.m_AttrData.GetAddIceErd(refVal, refLevel);
                if (m_RandomProperty > 0)
                {
                    AppendAttributeConfig data = AppendAttributeConfigProvider.Instance.GetDataById(m_RandomProperty);
                    if (null != data)
                    {
                        ret += data.GetAddIceErd(refVal, refLevel);
                    }
                }
            }
            return ret;
        }
        public float GetAddPoisonDam(float refVal, int refLevel)
        {
            float ret = 0;
            if (null != m_ItemConfig)
            {
                ret += m_ItemConfig.m_AttrData.GetAddPoisonDam(refVal, refLevel);
                if (m_RandomProperty > 0)
                {
                    AppendAttributeConfig data = AppendAttributeConfigProvider.Instance.GetDataById(m_RandomProperty);
                    if (null != data)
                    {
                        ret += data.GetAddPoisonDam(refVal, refLevel);
                    }
                }
            }
            return ret;
        }
        public float GetAddPoisonErd(float refVal, int refLevel)
        {
            float ret = 0;
            if (null != m_ItemConfig)
            {
                ret += m_ItemConfig.m_AttrData.GetAddPoisonErd(refVal, refLevel);
                if (m_RandomProperty > 0)
                {
                    AppendAttributeConfig data = AppendAttributeConfigProvider.Instance.GetDataById(m_RandomProperty);
                    if (null != data)
                    {
                        ret += data.GetAddPoisonErd(refVal, refLevel);
                    }
                }
            }
            return ret;
        }
        public float GetAddWeight(float refVal, int refLevel)
        {
            float ret = 0;
            if (null != m_ItemConfig)
            {
                ret += m_ItemConfig.m_AttrData.GetAddWeight(refVal, refLevel);
                if (m_RandomProperty > 0)
                {
                    AppendAttributeConfig data = AppendAttributeConfigProvider.Instance.GetDataById(m_RandomProperty);
                    if (null != data)
                    {
                        ret += data.GetAddWeight(refVal, refLevel);
                    }
                }
            }
            return ret;
        }
        public float GetAddRps(float refVal, int refLevel)
        {
            float ret = 0;
            if (null != m_ItemConfig)
            {
                ret += m_ItemConfig.m_AttrData.GetAddRps(refVal, refLevel);
                if (m_RandomProperty > 0)
                {
                    AppendAttributeConfig data = AppendAttributeConfigProvider.Instance.GetDataById(m_RandomProperty);
                    if (null != data)
                    {
                        ret += data.GetAddRps(refVal, refLevel);
                    }
                }
            }
            return ret;
        }
        public float GetAddAttackRange(float refVal, int refLevel)
        {
            float ret = 0;
            if (null != m_ItemConfig)
            {
                ret += m_ItemConfig.m_AttrData.GetAddAttackRange(refVal, refLevel);
                if (m_RandomProperty > 0)
                {
                    AppendAttributeConfig data = AppendAttributeConfigProvider.Instance.GetDataById(m_RandomProperty);
                    if (null != data)
                    {
                        ret += data.GetAddAttackRange(refVal, refLevel);
                    }
                }
            }
            return ret;
        }
        #endregion
    }
}
