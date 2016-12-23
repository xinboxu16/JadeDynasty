

/****    回旋破 四阶    ****/

skill(120341)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
  };

  section(133)//起手
  {
    animation("Cike_Skill03_01_01")
    {
      speed(1);
    };
    //
    //聚怪效果
    areadamage(10, 0, 0, 0, 6, true)
		{
			stateimpact("kDefault", 12030202);
      //showtip(200, 0, 0, 1);
		};
    //
    //特效
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_JuGuai", 3000, vector3(0, 0, 0), 10, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_FengRenShaErDuan_01_001", 3000, vector3(0, 0, 0), 10, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 500);
  };

  section(66)//第一段
  {
    animation("Cike_Skill03_01_02")
    {
      speed(1);
    };
    //
    //伤害判定
    areadamage(60, 0, 0, 0, 3, true)
		{
			stateimpact("kDefault", 12030201);
      //showtip(200, 0, 1, 0);
		};
    //
    //音效
    playsound(10, "Sound01", "Sound/Cike/CikeSkillSound01", 2000, "Sound/Cike/Cike_Skill03_FengRenZhan_01", false);
    playsound(10, "Sound02", "Sound/Cike/CikeSkillSound01", 2000, "Sound/Cike/Cike_Skill03_Voice_01", false);
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

  section(733)//硬直
  {
    animation("Cike_Skill03_01_03")
    {
      speed(1);
    };
    //
    //
    //角色移动
    startcurvemove(0, true, 0.35, 0, 25, 0, 0, -100, 0);
    startcurvemove(350, true, 0.15, 0, -50, 0, 0, 0, 0);
    //
    //特效
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_LuoDiYan_01", 3000, vector3(0, 0, 0), 400, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //打断
    addbreaksection(1, 650, 3000);
    addbreaksection(10, 600, 3000);
    addbreaksection(21, 600, 3000;);
    addbreaksection(100, 650, 3000;);
  };

  section(1)//收招
  {
    animation("Cike_Skill03_01_99")
    {
      speed(1);
    };
  };
};
