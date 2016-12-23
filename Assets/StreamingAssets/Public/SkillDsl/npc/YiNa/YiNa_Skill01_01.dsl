

/****    伊娜小突刺    ****/

skill(400101)
{
  section(1)//初始化
  {
    movechild(0, "5_IP_YiNa_01_w_01", "ef_righthand");//初始化主武器
    movechild(0, "5_IP_YiNa_01_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //瞬身类法术打断
    addbreaksection(11, 1, 30000);
  };

  section(66)//起手
  {
    animation("YiNa_Skill01_01_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(100, true, 0.06, 0, 0, 12, 0, 0, 0);
  };

  section(500)//第一段
  {
    animation("YiNa_Skill01_01_02")
    {
      speed(1);
    };
    //
     //角色移动
    startcurvemove(0, true, 0.1, 0, 0, 50, 0, 0, -100);
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2.4, true)
		{
			stateimpact("kDefault", 40010101);
			stateimpact("kLauncher", 40010102);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_XiaoTuCi_01", 500, "Bone_Root", 1);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);

    //打断
    addbreaksection(1, 200, 2000);
    addbreaksection(10, 200, 2000);
    addbreaksection(100, 200, 2000);
  };

  section(266)//收招
  {
    animation("YiNa_Skill01_01_99")
    {
      speed(1);
    };
  };
};
