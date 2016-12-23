

/****    上挑    ****/

skill(120221)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movecontrol(true);
    //
    //瞬身类技能打断
    addbreaksection(11, 1, 30000);
    //
  };

  section(400)//起手
  {
    animation("Cike_Skill02_01_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(100, true, 0.06, 0, 0, 15, 0, 0, 0);
  };

  section(66)//第一段
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
    areadamage(30, 0, 1.5, 1.5, 3.5, true)
		{
			stateimpact("kDefault", 12020101);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_ShangTiao_01", 3000, vector3(0, 0, 1), 10, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_ShangTiao_02", 3000, vector3(0, 0, 2.5), 10, eular(0, 0, 0), vector3(1, 1, 1), true);
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

  section(233)//硬直
  {
    animation("Cike_Skill02_01_03")
    {
      speed(1);
    };
    //
    //打断
    addbreaksection(1, 120, 333);
    addbreaksection(10, 220, 333);
    addbreaksection(21, 120, 333);
  };

  section(33)//收招
  {
    animation("Cike_Skill02_01_99")
    {
      speed(1);
    };
  };
};
