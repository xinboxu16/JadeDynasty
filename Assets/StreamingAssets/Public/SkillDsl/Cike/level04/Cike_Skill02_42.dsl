

/****    飞踢 四阶    ****/

skill(120242)
{
  section(1)//初始化 0
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
  };

  section(266)//起手 1
  {
    animation("Cike_Skill02_02_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(100, true, 0.06, 0, 0, 15, 0, 0, 0);
    //
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 500);
  };

  section(66)//第一段 2
  {
    animation("Cike_Skill02_02_02")
    {
      speed(1);
    };
    //
     //角色移动
    startcurvemove(0, true, 0.15, 0, 30, 20, 0, -10, 0);
    //
    //伤害判定
    areadamage(30, 0, 1.5, 1.5, 3, true)
		{
			stateimpact("kLauncher", 12020201);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    //charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_FeiTi_01 ", 3000, "ef_rightfoot", 10);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_FeiTi_01", 3000, vector3(0, 0, 1), 10, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill02_ShangTiao_02", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill02_Voice_01", false);
    //
    //定帧效果
    //lockframe(30, "Cike_Hit_01_02", true, 0.1, 100, 1, 100);
    //
    //震屏
    //shakecamera2(1, 400, false, true, vector3(0, 0, 1), vector3(0, 0, 400), vector3(0, 0, 0.1), vector3(0, 0, 100));
  };

  section(233)//硬直 3
  {
    animation("Cike_Skill02_02_03")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(0, true, 0.24, 0, 0, 0, 0, -10, 0);
  };

////////////////////////////      111111111      ////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

  section(33)//起手 4
  {
    animation("Cike_Skill02_03_01")
    {
      speed(2);
    };
    //
    //角色移动
    startcurvemove(100, true, 0.06, 0, 0, 15, 0, 0, 0);
    //
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 500);
  };

  section(83)//第一段 5
  {
    animation("Cike_Skill02_03_02")
    {
      speed(2);
    };
    //
     //角色移动
    startcurvemove(0, true, 0.15, 0, 5, 0, 0, -10, 0);
    //
    //伤害判定
    areadamage(30, 0, 1.5, 1.5, 3, true)
		{
			stateimpact("kLauncher", 12020301);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_PuGong_01_001", 500, "Bone_Root", 10);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill02_ShangTiao_03", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
    //
    //定帧效果
    //lockframe(30, "Cike_Hit_01_02", true, 0.1, 100, 1, 100);
  };

  section(100)//第二段 6
  {
    animation("Cike_Skill02_03_03")
    {
      speed(2);
    };
    //
     //角色移动
    startcurvemove(1, true, 0.15, 0, 5, 0, 0, -10, 0);
    //
    //伤害判定
    areadamage(30, 0, 1.5, 1.5, 3, true)
		{
			stateimpact("kLauncher", 12020302);
      //showtip(200, 0, 1, 0);
		};
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_PuGong_01_002", 500, "Bone_Root", 10);
    //
    //音效
    playsound(10, "Hit03", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill02_ShangTiao_04", false);
    playsound(10, "Hit04", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_02", true);
    //
    //定帧效果
    //lockframe(30, "Cike_Hit_01_02", true, 0.1, 100, 1, 100);
  };

////////////////////////////      2222222222      ////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

  section(33)//起手 7
  {
    animation("Cike_Skill02_03_01")
    {
      speed(2);
    };
    //
    //角色移动
    startcurvemove(100, true, 0.06, 0, 0, 15, 0, 0, 0);
    //
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 500);
  };

  section(83)//第一段 8
  {
    animation("Cike_Skill02_03_02")
    {
      speed(2);
    };
    //
     //角色移动
    startcurvemove(0, true, 0.15, 0, 5, 0, 0, -10, 0);
    //
    //伤害判定
    areadamage(30, 0, 1.5, 1.5, 3, true)
		{
			stateimpact("kLauncher", 12020301);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_PuGong_01_001", 500, "Bone_Root", 10);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill02_ShangTiao_03", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
    //
    //定帧效果
    //lockframe(30, "Cike_Hit_01_02", true, 0.1, 100, 1, 100);
  };

  section(100)//第二段 9
  {
    animation("Cike_Skill02_03_03")
    {
      speed(2);
    };
    //
     //角色移动
    startcurvemove(1, true, 0.15, 0, 5, 0, 0, -10, 0);
    //
    //伤害判定
    areadamage(30, 0, 1.5, 1.5, 3, true)
		{
			stateimpact("kLauncher", 12020302);
      //showtip(200, 0, 1, 0);
		};
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_PuGong_01_002", 500, "Bone_Root", 10);
    //
    //音效
    playsound(10, "Hit03", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill02_ShangTiao_04", false);
    playsound(10, "Hit04", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_02", true);
    //
    //定帧效果
    //lockframe(30, "Cike_Hit_01_02", true, 0.1, 100, 1, 100);
  };

////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////


  section(100)//硬直 10
  {
    animation("Cike_Skill02_03_04")
    {
      speed(0.2);
    };
    //
    //角色移动
    startcurvemove(1, true, 0.5, 0, 2, 0, 0, -10, 0);
  };

  section(1)//初始化 11
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
  };

  section(600)//起手 12
  {
		enablechangedir(300, 600);

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
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_ShenTiXuanZhuan_02", 2000, "Bone_Root", 10);
    //
    //音效
    playsound(10, "Sound01", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill02_ShangTiao_05", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill02_Voice_02", false);
    //
  };

  section(233)//第一段 13
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
    //检测地面高度
    checkonground(1, 0.1, "ToGround");
  };

  section(577) //14
  {
    animation("Cike_Skill02_04_03")
    {
      speed(1.5);
    };
    //
    //角色移动
    startcurvemove(0, true, 0.2, 0, 0, -12, 0, -30, 35);
    startcurvemove(300, true, 0.2, 0, 0, -15, 0, -30, 35);
    //
    //特效
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_LuoDiYan_02", 3000, vector3(0, 0, 0), 200, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_LuoDiYan_01", 3000, vector3(0, 0, 0), 400, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(400, "Sound03", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill02_ShangTiao_07", false);

    addbreaksection(10, 570, 1000);
    addbreaksection(11, 570, 1000);
    addbreaksection(21, 570, 1000);
    addbreaksection(100, 570, 1000);
  };

  onmessage("JumpDown_02")
  {
    animation("Cike_JumpDown_02")
    {
      speed(1);
    };
  };

  onmessage("ToGround")
  {
    stopcursection();
    gotosection(50, 14, 1);
  };
};
