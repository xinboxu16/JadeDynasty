

/****    双切 四阶   *****/

skill(120243)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
  };

////////////////////////////      111111111      ////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

  section(33)//起手
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

  section(83)//第一段
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

  section(100)//第二段
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

  section(33)//起手
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

  section(83)//第一段
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

  section(100)//第二段
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

  section(100)//硬直
  {
    animation("Cike_Skill02_03_04")
    {
      speed(1);
    };
    //
    //角色移动
    //startcurvemove(1, true, 0.24, 0, 0, 0, 0, -10, 0);
    //
    //打断
    addbreaksection(21, 99, 130);
  };

  /*section(33)//收招
  {
    animation("Cike_Skill02_03_99")
    {
      speed(1);
    };
    //
    //打断
    addbreaksection(10, 1, 1);
  };*/

  section(1000)//下落
  {
    animation("Cike_JumpDown_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(1, true, 1, 0, -10, 0, 0, -20, 0);
    //
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 1000);
    //
    //检测地面高度
    checkonground(1, 1, "JumpDown_02");
    checkonground(1, 0.1, "ToGround");
    //打断
    //addbreaksection(10, 499, 900);
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
  };
};
