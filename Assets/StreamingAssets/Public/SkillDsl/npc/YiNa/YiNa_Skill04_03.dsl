

/****    伊娜右冲    ****/

skill(400403)
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

  section(333)//第一段
  {
    animation("YiNa_Skill04_03_01")
    {
      speed(1);
    };
    //
     //角色移动
    startcurvemove(0, true, 0.3, 40, 0, 0, -100, 0, 0);
    //
    //特效
    charactereffect("Monster_FX/YiNa/6_Mon_YiNa_ShanShen_03", 500, "Bone_Root", 1);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };

  section(100)//硬直
  {
    animation("YiNa_Skill04_03_99")
    {
      speed(1);
    };
  };
};
