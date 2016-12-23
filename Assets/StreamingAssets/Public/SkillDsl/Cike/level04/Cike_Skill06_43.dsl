

/****    影袭二段 四阶    ****/

skill(120642)
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

  section(166)//起手
  {
    animation("Cike_Skill06_02_01")
    {
      speed(1);
    };
  };

  section(167)//第一段
  {
    animation("Cike_Skill06_02_02")
    {
      speed(2);
    };
    //
    //召唤NPC
    summonnpc(10, 101, "Hero/3_Cike/3_Cike_02", 125102, vector3(0, 0, 0));
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
    //charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_PuGong_01_001", 500, "Bone_Root", 1);
    //
    //音效
    //playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_01", false);
  };

  section(66)//第二段
  {
    animation("Cike_Skill06_02_03")
    {
      speed(1);
    };
    //
    //召唤NPC
    summonnpc(10, 101, "Hero/3_Cike/3_Cike_02", 125103, vector3(0, 0, 0));
    //
    //伤害判定
    /*
    areadamage(1, 0, 1.5, 0.8, 2.2, true)
		{
			stateimpact("kDefault", 12000102);
			stateimpact("kLauncher", 12000104);
			stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    */
    //特效
    //charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_PuGong_01_002", 500, "Bone_Root", 1);
    //sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    //playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_02", false);
  };

  section(66)//硬直
  {
    animation("Cike_Skill06_02_04")
    {
      speed(1);
    };
    //
    //打断
    addbreaksection(1, 66, 2000);
    addbreaksection(10, 66, 2000);
    addbreaksection(21, 66, 2000);
    addbreaksection(100, 66, 2000);
  };
  /*
  section(433)//收招
  {
    animation("Cike_Skill06_01_99")
    {
      speed(1);
    };
    */
    //
  };
};
