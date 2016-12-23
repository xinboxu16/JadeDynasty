

/****    突斩一段 三阶    ****/

skill(120131)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movecontrol(true);
  };

  section(133)//起手
  {
    animation("Cike_Skill01_01_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(100, true, 0.06, 0, 0, 15, 0, 0, 0);
    //
    //模型消失
    setenable(30, "Visible", false);
    //模型显示
    setenable(120, "Visible", true);
    //
    //特效
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 0), 10, eular(0, 0, 0), vector3(1, 1, 1), true);
  };

  section(300)//第一段
  {
    animation("Cike_Skill01_01_02")
    {
      speed(1);
    };
    //
     //角色移动
    startcurvemove(0, true, 0.15, 0, 0, 20, 0, 0, -100);
    //
    //伤害判定
    areadamage(30, 0, 1.5, 1.5, 3, true)
		{
			stateimpact("kDefault", 12010101);
			stateimpact("kKnockDown", 12990000);
			stateimpact("kLauncher", 12010103);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_TuZhan_01", 500, "Bone_Root", 10);
    //charactereffect("Hero_FX/5_cike/5_hero_cike_pugong_0011", 500, "Bone_Root", 1);
    //
    //模型消失
    setenable(150, "Visible", false);
    //模型显示
    setenable(270, "Visible", true);
    //
    //特效
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 0), 270, eular(0, 0, 0), vector3(1, 1, 1), true);
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

  section(66)//第二段
  {
    animation("Cike_Skill01_01_03")
    {
      speed(1);
    };
    //
     //角色移动
    startcurvemove(1, true, 0.03, 0, 0, 10, 0, 0, 0);
    //
    //伤害判定
    areadamage(30, 0, 1.5, 1.5, 3, true)
		{
			stateimpact("kDefault", 12010102);
			stateimpact("kKnockDown", 12990000);
			stateimpact("kLauncher", 12010104);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_TuZhan_02", 500, "Bone_Root", 10);
    //charactereffect("Hero_FX/5_cike/5_hero_cike_pugong_0012", 500, "Bone_Root", 1);
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
    animation("Cike_Skill01_01_04")
    {
      speed(1);
    };
    //
    //打断
    addbreaksection(1, 233, 3000);
    addbreaksection(10, 233, 3000);
    addbreaksection(21, 233, 3000);
  };

  /*section(1)//收招
  {
    animation("Cike_Skill01_01_99")
    {
      speed(1);
    };
    //
    //打断
    //addbreaksection(10, 499, 900);
  };*/

  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    //模型显示
    setenable(0, "Visible", true);
  };

  onstop() //技能正常结束时会运行该段逻辑
  {
    //模型显示
    setenable(0, "Visible", true);
  };
};
