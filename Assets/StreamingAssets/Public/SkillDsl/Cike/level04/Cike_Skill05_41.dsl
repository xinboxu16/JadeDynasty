

/****    瞬身斩 前斩 四阶    ****/

skill(120541)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //目标选择
		findmovetarget(0, vector3(0, 0, 1), 10, 60, 0.1, 0.9, 0, -3);
  };

  section(200)//起手
  {
    animation("Cike_Skill05_01_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(10, true, 0.1, 0, 0, 20, 0, 0, 0);
    //
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 500);
    //
    //特效
    //sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //模型消失
    setenable(10, "Visible", false);
    //模型显示
    setenable(150, "Visible", true);
    //
  };

  section(133)//第一段
  {
    animation("Cike_Skill05_01_02")
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
			stateimpact("kDefault", 12050101);
			stateimpact("kLauncher", 12050102);
			stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    //charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_ShunShenZhan_QianZhan_01", 500, "Bone_Root", 1);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_ShunShenZhan_QianZhan_01", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_ShunShenZhan_QianZhan_02", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill05_ShunShenZhan_01", false);
    playsound(10, "Hit01", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
    //playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Call_01", false);
    //
    //定帧效果
    //lockframe(30, "Cike_Hit_01_02", true, 0.1, 100, 1, 100);
    //
    //震屏
    //shakecamera2(1, 400, false, true, vector3(0, 0, 1), vector3(0, 0, 400), vector3(0, 0, 0.1), vector3(0, 0, 100));
  };

  section(100)//硬直
  {
    animation("Cike_Skill05_01_03")
    {
      speed(1);
    };
    //
    //打断
    addbreaksection(10, 100, 2000);
    addbreaksection(11, 100, 2000);
    addbreaksection(21, 100, 2000);
    addbreaksection(100, 100, 2000);
  };

  section(266)//收招
  {
    animation("Cike_Skill05_01_99")
    {
      speed(1);
    };
    //打断
    addbreaksection(1, 100, 2000);
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
