

/****    普攻一段    ****/

skill(120001)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //瞬身类法术打断
    addbreaksection(11, 1, 30000);
    //
		//findmovetarget(0, vector3(0, 0, 1), 2.5, 60, 0.1, 0.9, 0, -0.8);
  };

  section(133)//起手
  {
    animation("Cike_Hit_01_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(100, true, 0.06, 0, 0, 12, 0, 0, 0);
  };

  section(100)//第一段
  {
    animation("Cike_Hit_01_02")
    {
      speed(1.33);
    };
    //
     //角色移动
    startcurvemove(0, true, 0.15, 0, 0, 20, 0, 0, -100);
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2.4, true)
		{
			stateimpact("kDefault", 12000101);
			stateimpact("kLauncher", 12000103);
			stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_PuGong_01_001", 500, "Bone_Root", 1);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_01", false);
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

  section(100)//第二段
  {
    animation("Cike_Hit_01_03")
    {
      speed(1);
    };
    //
     //角色移动
    startcurvemove(10, true, 0.03, 0, 0, 10, 0, 0, 0);
    //
    //伤害判定
    areadamage(1, 0, 1.5, 0.8, 2.2, true)
		{
			stateimpact("kDefault", 12000102);
			stateimpact("kLauncher", 12000104);
			stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_PuGong_01_002", 500, "Bone_Root", 1);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_02", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_02", true);
    //定帧效果
    //lockframe(30, "Cike_Hit_01_02", true, 0.1, 100, 1, 100);
    //
    //震屏
    //shakecamera2(1, 400, false, true, vector3(0, 0, 1), vector3(0, 0, 400), vector3(0, 0, 0.1), vector3(0, 0, 100));
    //
    //打断
    //addbreaksection(10, 900, 933);
  };

  section(333)//硬直
  {
    animation("Cike_Hit_01_04")
    {
      speed(1);
    };
    //
    //打断
    addbreaksection(1, 119, 2000);
    addbreaksection(10, 120, 2000);
    addbreaksection(21, 120, 2000);
    addbreaksection(100, 120, 2000);
  };

  section(433)//收招
  {
    animation("Cike_Hit_01_99")
    {
      speed(1);
    };
    //
  };
};
