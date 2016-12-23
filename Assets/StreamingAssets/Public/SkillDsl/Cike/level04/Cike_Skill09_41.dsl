

/****    九刃一段 四阶    ****/

skill(120941)
{
  section(1)//开始  0
  {
    //
    //设置摄像机跟随参数
    setcamerafollowspeed(0, 40, 5, 15, 5, 1);
    //
    //设置霸体
    addimpacttoself(0, 12990001, 500);
  };

  section(1)//初始化  1
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //瞬身类法术打断
    addbreaksection(11, 1, 30000);
  };

  section(33)//起手  2
  {
    animation("Cike_Skill09_01_01")
    {
      speed(1);
    };
    //
		findmovetarget(0, vector3(0, 0, 1), 7, 240, 0.1, 0.9, 0, -3);
    //
    //角色移动
    startcurvemove(100, true, 0.06, 0, 0, 12, 0, 0, 0);
    //
    gotosection(0, 20, 0)
    {
      targetcondition(false);
    };
  };

  section(66)//第一段  3
  {
    animation("Cike_Skill09_01_02")
    {
      speed(1);
    };
    //
     //角色移动
    startcurvemove(0, true, 0.15, 0, 0, 20, 0, 0, -100);
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
    //charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_WuDiZhan_01_DaoGuang_01", 500, "Bone_Root", 1);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_WuDiZhan_01_DaoGuang_01", 3000, vector3(0, 0, 2), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_WuDiZhan_01_ShouJi_01", 3000, vector3(0, 0, 2), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };

  section(133)//硬直  4
  {
    animation("Cike_Skill09_01_03")
    {
      speed(1);
    };
    //
    //模型消失
    //setenable(10, "Visible", false);
    //模型显示
    //setenable(90, "Visible", true);
    //
    //特效
    //sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //角色移动
    startcurvemove(10, true, 0.1, 0, 0, -50, 0, 0, 0);
    //传送
    settransform(0, " ", vector3(0, 0, 0), eular(0, 144, 0), "RelativeSelf", true);
  };

  //////////////////////////         2222222         ////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////////

  section(200)//起手  5
  {
    animation("Cike_Skill09_02_01")
    {
      speed(1);
    };
    //
		findmovetarget(0, vector3(0, 0, 1), 7, 240, 0.1, 0.9, 0, -3);
    //
    //角色移动
    startcurvemove(100, true, 0.06, 0, 0, 12, 0, 0, 0);
    //
    //
    gotosection(0, 20, 0)
    {
      targetcondition(false);
    };
  };

  section(66)//第一段  6
  {
    animation("Cike_Skill09_02_02")
    {
      speed(1);
    };
    //
     //角色移动
    startcurvemove(0, true, 0.15, 0, 0, 20, 0, 0, -100);
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
    //charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_WuDiZhan_02_DaoGuang_01", 500, "Bone_Root", 1);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_WuDiZhan_02_DaoGuang_01", 3000, vector3(0, 0, 2), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_WuDiZhan_02_ShouJi_01", 3000, vector3(0, 0, 2), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };

  section(133)//硬直  7
  {
    animation("Cike_Skill09_02_03")
    {
      speed(1);
    };
    //
    //模型消失
    //setenable(10, "Visible", false);
    //模型显示
    //setenable(90, "Visible", true);
    //
    //特效
    //sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //角色移动
    startcurvemove(10, true, 0.1, 0, 0, -50, 0, 0, 0);
    //传送
    settransform(0, " ", vector3(0, 0, 0), eular(0, 144, 0), "RelativeSelf", true);
  };

  ////////////////////////////////    33333333     //////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////////

  section(33)//起手  8
  {
    animation("Cike_Skill09_03_01")
    {
      speed(1);
    };
    //
		findmovetarget(0, vector3(0, 0, 1), 7, 240, 0.1, 0.9, 0, -3);
    //
    //角色移动
    startcurvemove(100, true, 0.06, 0, 0, 12, 0, 0, 0);
    //
    gotosection(0, 20, 0)
    {
      targetcondition(false);
    };
  };

  section(33)//第一段  9
  {
    animation("Cike_Skill09_03_02")
    {
      speed(1);
    };
    //
     //角色移动
    startcurvemove(0, true, 0.15, 0, 0, 20, 0, 0, -100);
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
    //charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_WuDiZhan_03_Xian_01", 500, "Bone_Root", 1);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_WuDiZhan_03_Xian_01", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_WuDiZhan_03_ShouJi_01", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_WuDiZhan_03_ShouJi_02", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };

  section(233)//硬直  10
  {
    animation("Cike_Skill09_03_03")
    {
      speed(1);
    };
    //
    //模型消失
    //setenable(10, "Visible", false);
    //模型显示
    //setenable(90, "Visible", true);
    //
    //特效
    //sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //角色移动
    startcurvemove(10, true, 0.1, 0, 0, -50, 0, 0, 0);
    //传送
    settransform(0, " ", vector3(0, 0, 0), eular(0, 144, 0), "RelativeSelf", true);
  };

  //////////////////////////////     444444       ///////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////////

  section(100)//起手  11
  {
    animation("Cike_Skill09_04_01")
    {
      speed(1);
    };
    //
		findmovetarget(0, vector3(0, 0, 1), 7, 240, 0.1, 0.9, 0, -3);
    //
    //角色移动
    startcurvemove(100, true, 0.06, 0, 0, 12, 0, 0, 0);
    //
    gotosection(0, 20, 0)
    {
      targetcondition(false);
    };
  };

  section(66)//第一段  12
  {
    animation("Cike_Skill09_04_02")
    {
      speed(1);
    };
    //
     //角色移动
    startcurvemove(0, true, 0.15, 0, 0, 20, 0, 0, -100);
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
    //charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_WuDiZhan_04_DaoGuang_01", 500, "Bone_Root", 1);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_WuDiZhan_04_DaoGuang_01", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_WuDiZhan_04_ShouJi_01", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };

  section(33)//硬直  13
  {
    animation("Cike_Skill09_04_03")
    {
      speed(1);
    };
    //
    //模型消失
    //setenable(10, "Visible", false);
    //模型显示
    //setenable(90, "Visible", true);
    //
    //特效
    //sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //角色移动
    startcurvemove(10, true, 0.1, 0, 0, -50, 0, 0, 0);
    //传送
    settransform(0, " ", vector3(0, 0, 0), eular(0, 144, 0), "RelativeSelf", true);
  };


  //////////////////////////////     5555555       ///////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////////

  section(33)//起手  14
  {
    animation("Cike_Skill09_05_01")
    {
      speed(1);
    };
    //
		findmovetarget(0, vector3(0, 0, 1), 7, 240, 0.1, 0.9, 0, -3);
    //
    //角色移动
    startcurvemove(100, true, 0.06, 0, 0, 12, 0, 0, 0);
    //
    gotosection(0, 20, 0)
    {
      targetcondition(false);
    };
  };

  section(33)//第一段  15
  {
    animation("Cike_Skill09_05_02")
    {
      speed(1);
    };
    //
     //角色移动
    startcurvemove(0, true, 0.15, 0, 0, 20, 0, 0, -100);
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
    //charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_WuDiZhan_05_Xian_02", 500, "Bone_Root", 1);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_WuDiZhan_05_Xian_02", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_WuDiZhan_05_ShouJi_01", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_WuDiZhan_05_ShouJi_02", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };

  section(33)//硬直  16
  {
    animation("Cike_Skill09_05_03")
    {
      speed(1);
    };
    //
    //模型消失
    //setenable(10, "Visible", false);
    //模型显示
    //setenable(90, "Visible", true);
    //
    //特效
    //sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //角色移动
    startcurvemove(10, true, 0.1, 0, 0, -50, 0, 0, 0);
    //传送
    settransform(0, " ", vector3(0, 0, 0), eular(0, 144, 0), "RelativeSelf", true);
  };

  //////////////////////////////     666666       ///////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////////

  section(33)//起手  17
  {
    animation("Cike_Skill09_06_01")
    {
      speed(1);
    };
    //
		findmovetarget(0, vector3(0, 0, 1), 7, 240, 0.1, 0.9, 0, -3);
    //
    //角色移动
    startcurvemove(100, true, 0.06, 0, 0, 12, 0, 0, 0);
    //
    gotosection(0, 20, 0)
    {
      targetcondition(false);
    };
  };

  section(100)//第一段  18
  {
    animation("Cike_Skill09_06_02")
    {
      speed(1);
    };
    //
     //角色移动
    startcurvemove(0, true, 0.15, 0, 0, 20, 0, 0, -100);
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
    //charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_WuDiZhan_06_Xian_01", 500, "Bone_Root", 1);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_WuDiZhan_06_Xian_01", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_WuDiZhan_06_ShouJi_01", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_WuDiZhan_06_ShouJi_02", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };

  section(166)//硬直  19
  {
    animation("Cike_Skill09_06_03")
    {
      speed(1);
    };
    //
    //模型消失
    //setenable(10, "Visible", false);
    //模型显示
    //setenable(90, "Visible", true);
    //
    //特效
    //sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //角色移动
    startcurvemove(10, true, 0.1, 0, 0, -50, 0, 0, 0);
    //传送
    settransform(0, " ", vector3(0, 0, 0), eular(0, 144, 0), "RelativeSelf", true);
  };

    //////////////////////////////     7777777       ///////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////////

  section(1)//起手  20
  {
    animation("Cike_Skill09_07_01")
    {
      speed(1);
    };
  };

  section(200)//第一段  21
  {
    //
    //重置摄像机
    resetcamerafollowspeed(0);
    //
    animation("Cike_Skill09_07_02")
    {
      speed(1);
    };
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
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_WuDiZhan_07_LuoDi_01", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };

  section(300)//硬直  22
  {
    animation("Cike_Skill09_07_99")
    {
      speed(1);
    };
  };


  //////////////////////////////////////////////////////////
  //////////////////////////////////////////////////////////
  //////////////////////////////////////////////////////////


  section(1)//结束  23
  {
  };

  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    //模型显示
    setenable(0, "Visible", true);
    //
    //重置摄像机
    resetcamerafollowspeed(0);
  };

  onstop() //技能正常结束时会运行该段逻辑
  {
    //模型显示
    setenable(0, "Visible", true);
    //
    //重置摄像机
    resetcamerafollowspeed(0);
  };
};




