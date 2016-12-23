

/****    瞬身斩 后斩 四阶    ****/

skill(120542)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
  };

  section(156)//起手
  {
    animation("Cike_Skill05_02_01")
    {
      speed(1.5);
    };
    //
    //角色移动
    startcurvemove(10, true, 0.156, 0, 0, 20, 0, 0, 0);
    //
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 500);
    //
    //特效
    //sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill05_ShunShenZhan_02", false);
    //模型消失
    setenable(90, "Visible", false);
    //模型显示
    //setenable(140, "Visible", true);
    //
  };

   section(90)//起手二段
  {
    animation("Cike_Skill05_02_02")
    {
      speed(1.5);
    };
    //
    //特效
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiXian_01", 3000, vector3(0, 0, 0), 60, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //角色移动
    startcurvemove(10, true, 0.15, 0, 0, -20, 0, 0, 0);
    //
    //传送
    settransform(0, " ", vector3(0, 0, 0), eular(0, 180, 0), "RelativeSelf", true);
    //
    //模型消失
    //setenable(60, "Visible", false);
    //模型显示
    setenable(80, "Visible", true);
    //
  };

  section(133)//第一段
  {
    animation("Cike_Skill05_02_03")
    {
      speed(1);
    };
    //
    //传送
    //settransform(0, " ", vector3(0, 0, 0), eular(0, 180, 0), "RelativeSelf", true);
    //
     //角色移动
    //startcurvemove(0, true, 0.15, 0, 0, 20, 0, 0, -100);
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2.4, true)
		{
			stateimpact("kDefault", 12050201);
			stateimpact("kLauncher", 12050202);
			stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    //charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_ShunShenZhan_HouZhan_01", 500, "Bone_Root", 1);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_ShunShenZhan_HouZhan_01", 3000, vector3(0, 0, 0), 30, eular(0, 0, 0), vector3(1, 1, 1), true);
    //charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_ShunShenZhan_HouZhan_02", 500, "Bone_Root", 1);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_ShunShenZhan_HouZhan_02", 3000, vector3(0, 0, 0), 30, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill05_ShunShenZhan_01", false);
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_02", true);
    //playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Call_01", false);
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

  section(333)//硬直
  {
    animation("Cike_Skill05_02_04")
    {
      speed(1);
    };
    //
    //打断
    addbreaksection(1, 333, 2000);
    addbreaksection(10, 100, 2000);
    addbreaksection(11, 10, 2000);
    addbreaksection(21, 100, 2000);
    addbreaksection(100, 120, 2000);
  };

  section(366)//收招
  {
    animation("Cike_Skill05_02_99")
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
