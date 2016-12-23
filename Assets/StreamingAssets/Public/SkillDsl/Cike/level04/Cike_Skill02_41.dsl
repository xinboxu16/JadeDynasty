

/****    挑空 四阶    ****/

skill(120241)
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

  section(266)//起手
  {
    animation("Cike_Skill02_01_01")
    {
      speed(1);
    };
    //
    //自身增加霸体buff
    //addimpacttoself(0, 12990001, 500);
  };

  section(166)//第一段
  {
    animation("Cike_Skill02_01_02")
    {
      speed(1);
    };
    //
     //角色移动
    startcurvemove(0, true, 0.15, 0, 0, 20, 0, 0, -100);
    //
    //伤害判定
    areadamage(20, 0, 0.5, 1.5, 2.2, true)
		{
			stateimpact("kDefault", 12020101);
      //showtip(200, 0, 1, 0);
		};
    areadamage(90, 0, 0.5, 2.0, 2.2, true)
		{
			stateimpact("kDefault", 12020102);
      //showtip(200, 0, 0, 1);
		};
    areadamage(160, 0, 0.5, 2.5, 2.2, true)
		{
			stateimpact("kDefault", 12020103);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_ShangTiao_01", 3000, vector3(0, 0, 1), 10, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_ShangTiao_02_003", 3000, vector3(0, 0, 1.8), 10, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill02_ShangTiao_01", false);
    //
    //定帧效果
    //lockframe(30, "Cike_Hit_01_02", true, 0.1, 100, 1, 100);
    //
    //震屏
    //shakecamera2(1, 400, false, true, vector3(0, 0, 1), vector3(0, 0, 400), vector3(0, 0, 0.1), vector3(0, 0, 100));
  };

  section(100)//硬直
  {
    animation("Cike_Skill02_01_03")
    {
      speed(1);
    };
    //
    //打断
    addbreaksection(1, 20, 333);
    addbreaksection(10, 20, 333);
    addbreaksection(21, 20, 333);
    addbreaksection(100, 20, 333);
  };

  section(533)//收招
  {
    animation("Cike_Skill02_01_99")
    {
      speed(1);
    };
  };
};
