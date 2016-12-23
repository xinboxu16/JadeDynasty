

/****    瞬狱影杀阵影子木桩     ****/

skill(125401)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //设定方向为施法者方向
    settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
    //
    //设定生命时间
    setlifetime(0, 6500);
  };

  section(5000)//第一段
  {
    animation("Cike_Skill07_shadow01_01")
    {
      speed(4);
    };
    move2targetpos(0, 10) {
      ownerlasttouchpos();
    };
    //
    //特效
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_EX_FaZhen_01", 3000, vector3(0, 0, 0), 40, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_EX_02", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
    //playsound(290, "Hit03", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill04_ShunShen_01", false);
    //
  };

  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    //模型显示
    setenable(0, "Visible", true);
  };

  onstop() //技能正常结束时会运行该段逻辑
  {
    //模型显示
    setenable(0, "Visible", true);
  };
};
