

/****    伊娜旋风    ****/

skill(400301)
{
  section(1)//初始化
  {
    movechild(0, "5_IP_YiNa_01_w_01", "ef_righthand");//初始化主武器
    movechild(0, "5_IP_YiNa_01_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //瞬身类技能打断
    addbreaksection(11, 1, 30000);
  };

  section(133)//起手
  {
    animation("YiNa_Skill03_01_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(100, true, 0.06, 0, 0, 15, 0, 0, 0);
  };

  section(300)//第一段
  {
    animation("YiNa_Skill03_01_02")
    {
      speed(1);
    };
     //角色移动
     //startcurvemove(0, true, 0.15, 0, 0, 20, 0, 0, -100);
    //
    //伤害判定
    areadamage(0, 0, 1.5, 1.1, 2.2, true)
		{
			stateimpact("kDefault", 40030101);
			stateimpact("kKnockDown", 40000001);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_XuanFeng_01", 500, "Bone_Root", 1);
    //
    //音效
    playsound(0, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_06", false);
    playsound(0, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };

  section(166)//收招
  {
    animation("YiNa_Skill03_01_99")
    {
      speed(1);
    };
    //
    //打断
    addbreaksection(1, 1, 2000);
    addbreaksection(10, 1, 2000);
    addbreaksection(100, 1, 2000);
  };
};
