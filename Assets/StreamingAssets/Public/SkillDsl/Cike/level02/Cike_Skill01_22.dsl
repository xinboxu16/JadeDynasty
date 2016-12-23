

/****    突斩二段 二阶    ****/

skill(120122)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movecontrol(true);
  };

  section(66)//起手
  {
    animation("Cike_Skill01_02_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(100, true, 0.06, 0, 0, 15, 0, 0, 0);
    //
    //自身增加霸体buff
    //addimpacttoself(1, 12990001, 700);
  };

  section(566)//第一段
  {
    animation("Cike_Skill01_02_02")
    {
      speed(1);
    };
    //
     //角色移动
    startcurvemove(0, true, 0.566, 0, 10, 10, 0, -40, 0);
    //startcurvemove(0, true, 0.5, 0, 0, 10, 0, 0, 0);
    //
    //伤害判定
    areadamage(30, 0, 1.5, 1.5, 2.5, true)
		{
			stateimpact("kDefault", 12010201);
			stateimpact("kKnockDown", 12990000);
			stateimpact("kLauncher", 12010202);
      //showtip(200, 0, 1, 0);
		};
    areadamage(130, 0, 1.5, 1.5, 2.5, true)
		{
			stateimpact("kDefault", 12010201);
			stateimpact("kKnockDown", 12990000);
			stateimpact("kLauncher", 12010202);
      //showtip(200, 0, 1, 0);
		};
    areadamage(230, 0, 1.5, 1.5, 2.5, true)
		{
			stateimpact("kDefault", 12010201);
			stateimpact("kKnockDown", 12990000);
			stateimpact("kLauncher", 12010202);
      //showtip(200, 0, 1, 0);
		};
    areadamage(330, 0, 1.5, 1.5, 2.5, true)
		{
			stateimpact("kDefault", 12010201);
			stateimpact("kKnockDown", 12990000);
			stateimpact("kLauncher", 12010202);
      //showtip(200, 0, 1, 0);
		};
    areadamage(430, 0, 1.5, 1.5, 2.5, true)
		{
			stateimpact("kDefault", 12010201);
			stateimpact("kKnockDown", 12990000);
			stateimpact("kLauncher", 12010202);
      //showtip(200, 0, 1, 0);
		};
    areadamage(530, 0, 1.5, 1.5, 2.5, true)
		{
			stateimpact("kDefault", 12010203);
			stateimpact("kKnockDown", 12990000);
			stateimpact("kLauncher", 12010204);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_TuZhanYiDuan_01", 500, "Bone_Root", 10);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_LuoDiYan_02", 3000, vector3(0, 0, 0), 500, eular(0, 0, 0), vector3(1, 1, 1), true);
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

  section(66)//硬直
  {
    animation("Cike_Skill01_02_03")
    {
      speed(1);
    };
    //
    //打断
    addbreaksection(1, 66, 3000);
    addbreaksection(10, 66, 3000);
    addbreaksection(21, 66, 3000);
  };

  /*section(1)//收招
  {
    animation("Cike_Skill01_02_99")
    {
      speed(1);
    };
    //
    //打断
    //addbreaksection(10, 499, 900);
  };*/
};
