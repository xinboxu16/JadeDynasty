

/****    伊娜飞镖2    ****/

skill(400502)
{
  section(1)//初始化
  {
    movechild(0, "5_IP_YiNa_01_w_01", "ef_righthand");//初始化主武器
    movechild(0, "5_IP_YiNa_01_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //瞬身类法术打断
    addbreaksection(11, 1, 30000);
    //
		//findmovetarget(0, vector3(0, 0, 1), 2.5, 60, 0.1, 0.9, 0, -0.8);
  };

  section(133)//起手
  {
    animation("YiNa_Skill05_01_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(100, true, 0.06, 0, 0, 12, 0, 0, 0);
  };

  section(166)//第一段
  {
    animation("YiNa_Skill05_01_02")
    {
      speed(1);
    };
    //
    //召唤飞镖
    summonnpc(0, 101, "Monster_FX/YiNa/6_Mon_YiNa_FeiBiao_01", 400511, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/YiNa/6_Mon_YiNa_FeiBiao_01", 400512, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/YiNa/6_Mon_YiNa_FeiBiao_01", 400513, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/YiNa/6_Mon_YiNa_FeiBiao_01", 400514, vector3(0, 0, 0));
    summonnpc(0, 101, "Monster_FX/YiNa/6_Mon_YiNa_FeiBiao_01", 400515, vector3(0, 0, 0));
    //特效
    //charactereffect("Monster_FX/YiNa/6_Mon_YiNa_PuGong_01", 500, "Bone_Root", 1);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };

  section(100)//硬直
  {
    animation("YiNa_Skill05_01_99")
    {
      speed(1);
    };
  };
};
