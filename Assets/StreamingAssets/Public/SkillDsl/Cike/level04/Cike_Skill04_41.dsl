

/****    瞬身一段 四阶    ****/

skill(120441)
{
  section(1)//初始化 0
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //自身增加霸体buff
    addimpacttoself(0, 12990001, 500);
  };

  section(50)//判断 1
  {
    gotosection(0, 2, 50)
    {
      statecondition(true, "kStiffness");
    };

     gotosection(50, 4, 1);
  };

  section(50)//判断 2
  {
    addimpacttoself(0, 12990004, 500);
    addimpacttoself(1, 12990001, 1000);
    gotosection(0, 3, 50)
    {
      statecondition(true, "kStand");
    };
  };

  section(500)//受身分支 3
  {
    //
    //召唤NPC
    summonnpc(0, 101, "Hero/3_Cike/3_Cike_02", 125301, vector3(0, 0, 0));
    //
    movecontrol(true);
    animation("Cike_Skill04_03_01")
    {
      speed(1);
    };
    //
    //转向
    //settransform(0, " ", vector3(0, 0, 0), eular(0, 180, 0), "RelativeSelf", true);
    //
    //角色移动
    startcurvemove(10, true, 0.12, 0, 0, -25, 0, 0, 0);
    startcurvemove(120, true, 0.30, 0, 0, -15, 0, 0, 40);
    //
    //自身增加霸体buff
    //addimpacttoself(1, 12990001, 400);
    //
    //特效
    //sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 0), 10, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_001", 3000, vector3(0, 0, 0), 80, eular(0, 0, 0), vector3(1, 1, 1), true);
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_001", 3000, "Bone_Root", 10);
    //
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_ShunShen_01", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_LuoDiYan_03", 3000, vector3(0, 0, 0), 120, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_LuoDiYan_03", 3000, vector3(0, 0, 0), 220, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_LuoDiYan_03", 3000, vector3(0, 0, 0), 320, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(10, "Sound01", "Sound/Cike/CikeSkillSound01", 2000, "Sound/Cike/Cike_Skill04_ShunShen_01", false);
    playsound(10, "Sound02", "Sound/Cike/CikeSkillSound01", 2000, "Sound/Cike/Cike_Skill04_Voice_01", false);
    //
    //模型消失
    setenable(0, "Visible", false);
    //模型显示
    setenable(120, "Visible", true);
    //
    //跳转
    gotosection(500, 6, 1);
    //
    //跳转
    addbreaksection(10, 450, 1000);
    addbreaksection(11, 450, 1000);
    addbreaksection(21, 450, 1000);
    addbreaksection(100, 450, 1000);
  };

  section(233)//正常分支 4
  {

    movecontrol(true);
    animation("Cike_Skill04_01_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(0, true, 0.22, 0, 0, 30, 0, 0, -40);
    //
    //自身增加霸体buff
//    addimpacttoself(1, 12990001, 400);
    //
    //伤害判定
    //areadamage(30, 0, 0, 0, 2, false)
		//{
		//	stateimpact("kDefault", 12040101);
      //showtip(200, 0, 1, 0);
		//};
    //areadamage(80, 0, 0, 0, 2, false)
		//{
		//	stateimpact("kDefault", 12040101);
      //showtip(200, 0, 1, 0);
		//};
    //特效
    //sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 0), 10, eular(0, 0, 0), vector3(1, 1, 1), true);
    //sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_001", 3000, vector3(0, 0, 0), 80, eular(0, 0, 0), vector3(1, 1, 1), true);
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_001", 3000, "Bone_Root", 120);
    //
    //音效
    playsound(10, "Sound01", "Sound/Cike/CikeSkillSound01", 2000, "Sound/Cike/Cike_Skill04_ShunShen_01", false);
    playsound(10, "Sound02", "Sound/Cike/CikeSkillSound01", 2000, "Sound/Cike/Cike_Skill04_Voice_01", false);
    //
    //模型消失
    setenable(50, "Visible", false);
    //模型显示
    setenable(180, "Visible", true);
    //
    //打断
    addbreaksection(1, 200, 1000);
    addbreaksection(10, 200, 1000);
    addbreaksection(11, 200, 1000);
    addbreaksection(21, 200, 1000);
    addbreaksection(100, 200, 1000);
    //
  };

  section(66)//正常收招 5
  {
    animation("Cike_Skill04_01_99")
    {
      speed(1);
    };
    //
    //跳转
    gotosection(65, 7, 1);
  };

  section(733)//受身收招 6
  {
    animation("Cike_Skill04_03_99")
    {
      speed(1);
    };
    //
    //打断
    addbreaksection(1, 1, 1000);
  };

  section(1)//结束 7
  {
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
