

/****    普攻二段    ****/

skill(120002)
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

  section(133)//第一段
  {
    animation("Cike_Hit_02_01")
    {
      speed(2);
    };
    //
    //角色移动
    startcurvemove(0, true, 0.15, 0, 0, 16, 0, 0, -100);
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.2, 2.4, true)
		{
			stateimpact("kDefault", 12000201);
			stateimpact("kLauncher", 12000204);
			stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_PuGong_02_001", 500, "Bone_Root", 1);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_03", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_02", true);
  };

  section(233)//第二段
  {
    animation("Cike_Hit_02_02")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(100, true, 0.06, 0, 0, 15, 0, 0, 0);
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.2, 2.4, true)
		{
			stateimpact("kDefault", 12000202);
			stateimpact("kLauncher", 12000205);
			stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_PuGong_02_002", 500, "Bone_Root", 1);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_04", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_03", true);
    //
    //定帧效果
    //lockframe(30, "Cike_Hit_01_02", true, 0.1, 100, 1, 100);
    //
    //震屏
    //shakecamera2(1, 400, false, true, vector3(0, 0, 1), vector3(0, 0, 400), vector3(0, 0, 0.1), vector3(0, 0, 100));
  };

  section(166)//第三段
  {
    animation("Cike_Hit_02_03")
    {
      speed(1);
    };
    //
     //角色移动
    startcurvemove(1, true, 0.1, 0, 0, 20, 0, 0, 0);
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2.4, true)
		{
			stateimpact("kDefault", 12000203);
			stateimpact("kLauncher", 12000206);
			stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0;)
		};
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_PuGong_02_003", 2000, "Bone_Root", 1);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_05", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_04", true);
    //
    //定帧效果
    //lockframe(30, "Cike_Hit_01_02", true, 0.1, 100, 1, 100);
    //
    //震屏
    //shakecamera2(1, 400, false, true, vector3(0, 0, 1), vector3(0, 0, 400), vector3(0, 0, 0.1), vector3(0, 0, 100));
    //
    //打断
    //addbreaksection(10, 1, 233);
  };

  section(466)//硬直
  {
    animation("Cike_Hit_02_04")
    {
      speed(1);
    };
    //
    //打断
    addbreaksection(1, 99, 2000);
    addbreaksection(10, 100, 2000);
    addbreaksection(21, 100, 2000);
    addbreaksection(100, 100, 2000);
  };

  section(333)//收招
  {
    animation("Cike_Hit_02_99")
    {
      speed(1);
    };
    //
  };
};
