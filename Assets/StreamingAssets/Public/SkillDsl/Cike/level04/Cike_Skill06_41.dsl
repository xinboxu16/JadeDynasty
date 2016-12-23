

/****    影袭一段 四阶    ****/

skill(120641)
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
    enablechangedir(0, 2000);
		//findmovetarget(0, vector3(0, 0, 1), 2.5, 60, 0.1, 0.9, 0, -0.8);
  };

  section(66)//起手
  {
    animation("Cike_Skill06_01_01")
    {
      speed(1);
    };
  };

  section(50)//第一段
  {
    animation("Cike_Skill06_01_02")
    {
      speed(2);
    };
    //
    //
    enablechangedir(0, 2000);
    //
    //召唤NPC
    summonnpc(0, 101, "Hero/3_Cike/3_Cike_02", 125101, vector3(0, 0, 0));
    //
    //伤害判定
    /*
    areadamage(10, 0, 1.5, 1.5, 2.4, true)
		{
			stateimpact("kDefault", 12000101);
			stateimpact("kLauncher", 12000103);
			stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    */
    //
    //特效
    //charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_PuGong_01_001", 500, "Bone_Root", 1);
    //sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    //playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_01", false);
  };

  section(200)//硬直
  {
    animation("Cike_Skill06_01_03")
    {
      speed(1);
    };
    //打断
    addbreaksection(1, 100, 2000);
    addbreaksection(10, 100, 2000);
    addbreaksection(21, 100, 2000);
    addbreaksection(100, 100, 2000);
  };
};
