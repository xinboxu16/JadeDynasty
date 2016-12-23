

/****    流星 四阶    ****/

skill(120244)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
  };

  section(600)//起手
  {
    animation("Cike_Skill02_04_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(100, true, 0.6, 0, 8, -10, 0, -10, 10);
    //
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 1500);
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_ShenTiXuanZhuan_01", 2000, "Bone_Root", 10);
    //
    //音效
    playsound(10, "Sound01", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill02_ShangTiao_05", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill02_Voice_02", false);
    //
  };

  section(133)//第一段
  {
    animation("Cike_Skill02_04_02")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(0, true, 0.1, 0, -70, 60, 0, 0, 0);
    //
    //伤害
    areadamage(100, 0, 0, 0, 4, false)
    {
      stateimpact("kDefault", 12020401);
      //showtip(200, 0, 1, 0);
    };
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_TiaoXiaShunShen_01", 3000, "Bone_Root", 10);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_BiaoLianHua_03_001", 3000, vector3(0, 0, 0), 100, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(10, "Sound02", "Sound/Cike/CikeSkillSound01", 2000, "Sound/Cike/Cike_Skill02_ShangTiao_06", false);
    //
    //定帧效果
    //lockframe(30, "Cike_Hit_01_02", true, 0.1, 100, 1, 100);
    //
    //震屏
    //shakecamera2(1, 400, false, true, vector3(0, 0, 1), vector3(0, 0, 400), vector3(0, 0, 0.1), vector3(0, 0, 100));
  };

  section(866)//硬直
  {
    animation("Cike_Skill02_04_03")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(0, true, 0.3, 0, 0, -16, 0, -30, 35);
    startcurvemove(300, true, 0.3, 0, 0, -18, 0, -30, 35);
    //
    //特效
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_LuoDiYan_02", 3000, vector3(0, 0, 0), 300, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_LuoDiYan_01", 3000, vector3(0, 0, 0), 600, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(600, "Sound03", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill02_ShangTiao_07", false);
  };

  section(333)//收招
  {
    animation("Cike_Skill02_04_99")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(0, true, 0.3, 0, 0, 0, 0, -30, 0);
    //
    //打断
    addbreaksection(1, 1, 133);
    addbreaksection(10, 1, 133);
    addbreaksection(21, 1, 133);
    addbreaksection(100, 1, 133);
  };
};
