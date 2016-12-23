

/****    双切2 四阶   *****/

skill(121243)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
  };

  section(133)//起手
  {
    animation("Cike_Test01_01")
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

  section(166)//第一段
  {
    animation("Cike_Test01_02")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(0, true, 0.15, 0, 5, 0, 0, -10, 0);
    //
    //伤害判定
    areadamage(10, 0, 1, 1.5, 3, true)
		{
			stateimpact("kLauncher", 12020303);
      showtip(200, 0, 0, 1);
		};
    areadamage(110, 0, -2, 1.5, 3, true)
		{
			stateimpact("kDefault", 12020304);
      showtip(200, 0, 1, 1);
		};
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_PuGong_01_001", 500, "Bone_Root", 10);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill02_ShangTiao_03", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
    //
    //定帧效果
    //lockframe(30, "Cike_Hit_01_02", true, 0.1, 100, 1, 100);
  };

  section(233)//硬直
  {
    animation("Cike_Test01_03")
    {
      speed(1);
    };
    //
    //角色移动
    //startcurvemove(1, true, 0.24, 0, 0, 0, 0, -10, 0);
    //
    //打断
    addbreaksection(21, 99, 130);
  };

  section(1000)//下落
  {
    animation("Cike_JumpDown_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(1, true, 1, 0, -10, 0, 0, -20, 0);
    //
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 1000);
    //
    //检测地面高度
    checkonground(1, 1, "JumpDown_02");
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
