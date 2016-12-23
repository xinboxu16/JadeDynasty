

/****    飞踢    ****/

skill(120232)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movecontrol(true);
    //
    //瞬身类技能打断
    //addbreaksection(11, 1, 30000);
    //
  };

  section(266)//起手
  {
    animation("Cike_Skill02_02_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(100, true, 0.06, 0, 0, 15, 0, 0, 0);
    //
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 500);
  };

  section(66)//第一段
  {
    animation("Cike_Skill02_02_02")
    {
      speed(1);
    };
    //
     //角色移动
    startcurvemove(0, true, 0.15, 0, 30, 30, 0, -10, 0);
    //
    //伤害判定
    areadamage(30, 0, 1.5, 1.5, 4, true)
		{
			stateimpact("kLauncher", 12020201);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_FeiTi_01", 3000, "ef_rightfoot", 10);
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
    animation("Cike_Skill02_02_03")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(0, true, 0.24, 0, 0, 0, 0, -10, 0);
    //
    //
    //打断
    addbreaksection(21, 232, 263);
  };

  /*section(433)//收招
  {
    animation("Cike_Skill02_02_99")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(0, true, 0.4, 0, 0, 0, 0, -10, 0);
    //
    //打断
    addbreaksection(10, 1, 433);
  };*/

  section(1000)//下落
  {
    animation("Cike_JumpDown_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(1, true, 1, 0, -5, 5, 0, -20, 0);
    //
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 1000);
    //
    //检测地面高度
    checkonground(1, 2, "JumpDown_02");
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
