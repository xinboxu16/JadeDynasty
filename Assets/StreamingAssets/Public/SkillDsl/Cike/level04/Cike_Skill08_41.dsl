

/****    影子模仿一段 四阶    ****/

skill(120841)
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
    //
    //为自己增加变更技能的impact
		//addimpacttoself(1, 12080101, 5000);
  };

  section(100)//第一段
  {
    animation("Cike_Skill08_01_01")
    {
      speed(1);
    };
    //
    //特效
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_YingZiMoFang_HeiYan_01", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_YingZiMoFang_HeiYanFaShe_01", 3000, "Bone_Root", 0);
    //
    //召唤NPC
    summonnpc(0, 102, "Hero/3_Cike/3_Cike_03", 125201, vector3(0, 0, 0));
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_01", false);
    //
    //打断
    addbreaksection(1, 300, 2000);
    addbreaksection(10, 10, 2000);
    addbreaksection(21, 10, 2000);
    addbreaksection(100, 100, 2000);
  };

  section(533)//收招
  {
    animation("Cike_Skill08_01_99")
    {
      speed(1);
    };
  };
};
