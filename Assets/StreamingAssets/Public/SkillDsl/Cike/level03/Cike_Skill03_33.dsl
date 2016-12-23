

/****    升龙刃 三阶    ****/

skill(120333)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movecontrol(true);
  };

  section(200)//起手
  {
    animation("Cike_Skill03_03_01")
    {
      speed(1);
    };
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_ShengLongRen_01_002", 3000, "Bone_Root", 100);
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_ShengLongRen_01_003", 900, "Bone_Root", 100);
    //
    //音效
    playsound(10, "Sound01", "Sound/Cike/CikeSkillSound01", 2000, "Sound/Cike/Cike_Skill03_FengRenZhan_02", false);
    playsound(10, "Sound02", "Sound/Cike/CikeSkillSound01", 2000, "Sound/Cike/Cike_Skill03_Voice_02", false);
    //
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 1000);
  };

  section(833)//第一段
  {
    animation("Cike_Skill03_03_02")
    {
      speed(1);
    };
    //
     //角色移动
    startcurvemove(0, true, 0.05, 0, 70, 0, 0, -300, 0);
    startcurvemove(50, true, 0.35, 0, 20, 0, 0, -50, 0);
    startcurvemove(400, true, 0.2, 0, 0, 0, 0, -20, 0);
    //
    //伤害判定
    areadamage(30, 0, 0, 0, 3, true)
		{
			stateimpact("kDefault", 12030301);
      //showtip(200, 0, 1, 0);
		};
    areadamage(130, 0, 0, 0, 3, true)
		{
			stateimpact("kDefault", 12030301);
      //showtip(200, 0, 1, 0);
		};
    areadamage(230, 0, 0, 0, 3, true)
		{
			stateimpact("kDefault", 12030301);
      //showtip(200, 0, 1, 0);
		};
    areadamage(330, 0, 0, 0, 3, true)
		{
			stateimpact("kDefault", 12030301);
      //showtip(200, 0, 1, 0);
		};
    areadamage(430, 0, 0, 0, 3, true)
		{
			stateimpact("kDefault", 12030301);
      //showtip(200, 0, 1, 0);
		};
    //
    //定帧效果
    //lockframe(30, "Cike_Hit_01_02", true, 0.1, 100, 1, 100);
    //
    //震屏
    //shakecamera2(1, 400, false, true, vector3(0, 0, 1), vector3(0, 0, 400), vector3(0, 0, 0.1), vector3(0, 0, 100));
    //
  };

  section(1000)//下落
  {
    animation("Cike_JumpDown_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(1, true, 1, 0, -20, 0, 0, -20, 0);
    //
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 1000);
    //
    //检测地面高度
    checkonground(1, 2, "JumpDown_02");
    checkonground(1, 0.1, "ToGround");
    //
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
