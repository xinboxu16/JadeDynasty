

/****    影刃 四阶    ****/

skill(121143)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
  };

  section(200)//起手
  {
    animation("Cike_Skill01_02_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(30, true, 0.2, 0, 0, 15, 0, 0, -40);
    //
    //自身增加霸体buff
    //addimpacttoself(1, 12990001, 500);
    //
    //模型消失
    setenable(30, "Visible", false);
    //模型显示
    setenable(120, "Visible", true);
    //
    //特效
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 0), 10, eular(0, 0, 0), vector3(1, 1, 1), true);
  };

  section(166)//第一段
  {
    animation("Cike_Skill01_02_02")
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
			stateimpact("kDefault", 12010301);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_TuZhan_03", 2000, "Bone_Root", 10);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_LuoDiYan_01", 3000, vector3(-0.6, 0, 1.5), 10, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill01_TuZhan_05", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill01_Voice_02", false);
    //
    //定帧效果
    //lockframe(30, "Cike_Hit_01_02", true, 0.1, 100, 1, 100);
    //
    //震屏
    //shakecamera2(1, 400, false, true, vector3(0, 0, 1), vector3(0, 0, 400), vector3(0, 0, 0.1), vector3(0, 0, 100));
  };

  section(266)//硬直
  {
    animation("Cike_Skill01_02_03")
    {
      speed(1);
    };
    //
    //打断
    addbreaksection(1, 266, 3000);
    addbreaksection(10, 266, 3000);
    addbreaksection(21, 266, 3000);
    addbreaksection(100, 266, 3000);
  };

  section(500)//收招
  {
    animation("Cike_Skill01_02_99")
    {
      speed(1);
    };
  };

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
