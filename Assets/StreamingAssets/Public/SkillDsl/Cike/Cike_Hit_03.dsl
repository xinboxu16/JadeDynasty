

/****    普攻三段    ****/

skill(120003)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //瞬身类技能打断
    addbreaksection(11, 1, 30000);
    //
  };

  section(66)//起手
  {
    animation("Cike_Hit_03_01")
    {
      speed(1);
    };
    //
    //角色移动
    //startcurvemove(100, true, 0.06, 0, 0, 15, 0, 0, 0);
  };

  section(133)//第一段
  {
    animation("Cike_Hit_03_02")
    {
      speed(1);
    };
     //角色移动
    //startcurvemove(0, true, 0.15, 0, 0, 20, 0, 0, -100);
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.1, 2.2, true)
		{
			stateimpact("kDefault", 12000301);
			stateimpact("kLauncher", 12000304);
			stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_PuGong_03_001", 500, "Bone_Root", 1);
    //
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_06", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
    //
    //定帧效果
    //lockframe(30, "Cike_Hit_01_02", true, 0.1, 100, 1, 100);
    //
    //震屏
    //shakecamera2(1, 400, false, true, vector3(0, 0, 1), vector3(0, 0, 400), vector3(0, 0, 0.1), vector3(0, 0, 100));
    //
    //打断
    //addbreaksection(10, 900, 933);
  };

  section(133)//第二段
  {
    animation("Cike_Hit_03_03")
    {
      speed(1);
    };
    //
    //角色移动
    //startcurvemove(1, true, 0.06, 0, 0, 15, 0, 0, 0);
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.1, 2.2, true)
		{
			stateimpact("kDefault", 12000302);
			stateimpact("kLauncher", 12000305);
			stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_PuGong_03_002", 500, "Bone_Root", 1);
    //
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_07", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_02", true);
    //
    //定帧效果
    //lockframe(30, "Cike_Hit_01_02", true, 0.1, 100, 1, 100);
    //
    //震屏
    //shakecamera2(1, 400, false, true, vector3(0, 0, 1), vector3(0, 0, 400), vector3(0, 0, 0.1), vector3(0, 0, 100));
    //
    //打断
    //addbreaksection(10, 900, 933);
  };

  section(100)//第三段
  {
    animation("Cike_Hit_03_04")
    {
      speed(1);
    };
     //角色移动
    //startcurvemove(1, true, 0.06, 0, 0, 15, 0, 0, 0);
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.1, 2.2, true)
		{
			stateimpact("kDefault", 12000303);
			stateimpact("kLauncher", 12000306);
			stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_PuGong_03_003", 500, "Bone_Root", 1);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_06", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
    //
    //定帧效果
    //lockframe(30, "Cike_Hit_01_02", true, 0.1, 100, 1, 100);
    //
    //震屏
    //shakecamera2(1, 400, false, true, vector3(0, 0, 1), vector3(0, 0, 400), vector3(0, 0, 0.1), vector3(0, 0, 100));
    //
    //打断
    //addbreaksection(10, 900, 933);
  };

  section(133)//硬直
  {
    animation("Cike_Hit_03_05")
    {
      speed(1);
    };
    //
    //角色移动
    //startcurvemove(66, true, 0.12, 0, 0, 20, 0, 0, 0);
    //
    //打断
    addbreaksection(1, 119, 2000);
    addbreaksection(10, 120, 2000);
    addbreaksection(21, 120, 2000);
    addbreaksection(100, 120, 2000);
  };

  section(266)//收招
  {
    animation("Cike_Hit_03_99")
    {
      speed(1);
    };
    //
  };
};
