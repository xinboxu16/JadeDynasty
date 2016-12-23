

/****    伊娜普攻三段    ****/

skill(400003)
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

  section(22)//起手
  {
    animation("YiNa_Hit_03_01")
    {
      speed(1.5);
    };
    //
    //角色移动
    startcurvemove(100, true, 0.06, 0, 0, 15, 0, 0, 0);
  };

  section(200)//第一段
  {
    animation("YiNa_Hit_03_02")
    {
      speed(1);
    };
     //角色移动
     //startcurvemove(0, true, 0.15, 0, 0, 20, 0, 0, -100);
    //
    //伤害判定
    areadamage(0, 0, 1.5, 1.1, 2.2, true)
		{
			stateimpact("kDefault", 40000105);
			stateimpact("kLauncher", 40000106);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
    areadamage(150, 0, 1.5, 1.1, 2.2, true)
		{
			stateimpact("kDefault", 40000107);
			stateimpact("kLauncher", 40000108);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_PuGong_03_01", 500, "Bone_Root", 1);
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_PuGong_03_02", 500, "Bone_Root", 150);
    //
    //音效
    playsound(0, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_06", false);
    playsound(150, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_06", false);
    playsound(0, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
    playsound(150, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };

  section(177)//硬直
  {
    animation("YiNa_Hit_03_03")
    {
      speed(1.5);
    };
    //
    //角色移动
    //startcurvemove(66, true, 0.12, 0, 0, 20, 0, 0, 0);
  };
};
