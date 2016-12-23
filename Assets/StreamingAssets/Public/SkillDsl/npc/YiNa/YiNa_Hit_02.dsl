

/****    伊娜普攻二段    ****/

skill(400002)
{
  section(1)//初始化
  {
    movechild(0, "5_IP_YiNa_01_w_01", "ef_righthand");//初始化主武器
    movechild(0, "5_IP_YiNa_01_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //瞬身类技能打断
    addbreaksection(11, 1, 30000);
    //
  };

  section(88)//起手
  {
    animation("YiNa_Hit_02_01")
    {
      speed(1.5);
    };
  };

  section(44)//第一段
  {
    animation("YiNa_Hit_02_02")
    {
      speed(1.5);
    };
    //
    //角色移动
    startcurvemove(0, true, 0.15, 0, 0, 16, 0, 0, -100);
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.2, 2.4, true)
		{
			stateimpact("kDefault", 40000103);
			stateimpact("kLauncher", 40000104);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_PuGong_02", 500, "Bone_Root", 1);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_03", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_02", true);
  };

  section(110)//硬直
  {
    animation("YiNa_Hit_02_03")
    {
      speed(1.5);
    };
  };
};
