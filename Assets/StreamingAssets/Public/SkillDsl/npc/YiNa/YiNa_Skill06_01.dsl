

/****    伊娜防御    ****/

skill(400601)
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
    //自身增加无敌霸体buff
    addimpacttoself(0, 40000003, 1000);
  };

  section(100)//起手
  {
    animation("YiNa_Skill06_01_01")
    {
      speed(1);
    };
  };

  section(733)//第一段
  {
    animation("YiNa_Skill06_01_02")
    {
      speed(1);
    };
    //
    //特效
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_FangYu_01", 500, "Bone_Root", 1);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };

  section(166)//硬直
  {
    animation("YiNa_Skill06_01_99")
    {
      speed(1);
    };
  };
};
