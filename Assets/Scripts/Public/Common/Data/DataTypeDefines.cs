using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 * @file DataTypeDefines.cs
 * @brief 数据枚举定义
 *
 * @author lixiaojiang
 * @version 1.0.0
 * @date 2012-12-16
 */

namespace DashFire
{
    //-----------------------------------------------------------
    // 单场景数据相关枚举类型
    //-----------------------------------------------------------

    /**
     * @brief 单场景数据类型
     */
    public enum DataMap_Type
    {
        DT_None,
        DT_Unit,
        DT_SceneLogic,
        DT_All,
    }

    /**
     * @brief 技能配置数据类型
     */
    public enum SkillConfigType
    {
        SCT_SKILL,
        SCT_IMPACT,
        SCT_EFFECT,
        SCT_SOUND,
    }

    /**
     * @brief 角色动作类型
     *
     */
    public enum Animation_Type
    {
        // 无动作
        AT_None = -1,

        // 前跑
        AT_RunForward = 1,

        // 开火
        AT_Attack = 5,

        // 投掷
        AT_HitHigh = 8,

        // 受伤
        AT_Hurt0 = 9,
        AT_Hurt1 = 10,
        AT_Hurt2 = 11,

        AT_SlowMove = 12,
        AT_FastMove = 13,

        // 休闲
        AT_IdelStand = 14,
        AT_Idle0 = 15,
        AT_Idle1 = 16,
        AT_Idle2 = 17,

        // 死亡
        AT_Dead = 18,
        AT_Born = 19,
        AT_Taunt = 20,

        AT_RunForwardShoot,
        AT_RunBackwardShoot,
        AT_RunLeftShoot,
        AT_RunRightShoot,
        AT_RunForwardLeftShoot,
        AT_RunForwardRightShoot,
        AT_RunBackwardLeftShoot,
        AT_RunBackwardRightShoot,

        AT_PostDead,

        // 影技能 影闪
        AT_DashSlow = 51,
        AT_GetUp1 = 55,
        AT_GetUp2 = 56,

        AT_Stand = 60,
        AT_SLEEP = 63,
        AT_FlyUp = 64,
        AT_FlyDown = 65,
        AT_FlyHurt = 66,
        AT_FlyDownGround = 67,
        AT_OnGround = 68,
        AT_Grab = 69,

        AT_CombatStand = 70,
        AT_CombatRun = 71,

        AT_Celebrate = 99,
        AT_Depressed = 100,
        AT_SkillSection1 = 101,
        AT_SkillSection2 = 102,
        AT_SkillSection3 = 103,
        AT_SkillSection4 = 104,
        AT_SkillSection5 = 105,
        AT_SkillSection6 = 106,
        AT_SkillSection7 = 107,
        AT_SkillSection8 = 108,
        AT_SkillSection9 = 109,
        AT_SkillSection10 = 110,
        AT_SkillSection11 = 111,
        AT_SkillSection12 = 112,
        AT_SkillSection13 = 113,
        AT_SkillSection14 = 114,
        AT_SkillSection15 = 115,
        AT_SkillSection16 = 116,
        AT_SkillSection17 = 117,
        AT_SkillSection18 = 118,
        AT_SkillSection19 = 119,
        AT_SkillSection20 = 120,
    }
}
