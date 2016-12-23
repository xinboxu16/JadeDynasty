

/****    伊娜大突刺    ****/

skill(400201)
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

  section(466)//起手
  {
    animation("YiNa_Skill02_01_01")
    {
      speed(1);
    };
    //
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_DaTuCi_XuLi_01", 500, "Bone_Root", 1);
  };
  section(266)//第一段
  {
    animation("YiNa_Skill02_01_02")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(1, true, 0.1, 0, 0, 100, 0, 0, -30);
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2.8, true)
		{
			stateimpact("kDefault", 40020101);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_DaTuCi_02", 1500, "Bone_Root", 1);
    sceneeffect("Monster_FX/YiNa/6_Mon_YiNa_DaTuCi_Huo", 3000, vector3(0, 0, 1), 10, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_08", false);
    playsound(20, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_02", true);
    playsound(20, "Hit03", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_Voice_01", false);
  };

  section(333)//收招
  {
    animation("YiNa_Skill02_01_99")
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
