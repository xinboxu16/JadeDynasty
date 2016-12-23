

/****    影子模仿影子      ****/

skill(125201)
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
    setlifetime(0, 10000);
  };

  section(520)//第一段
  {
    //角色移动
    startcurvemove(0, true, 0.5, 0, 0, 10, 0, 0, 0);
    //
    //模型消失
    setenable(0, "Visible", false);
    //模型显示
    setenable(500, "Visible", true);
    //
    //模仿主角动作
    simulatemove(500);
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_YingZiMoFang_YiDuan_01", 2000, "Bone_Root", 500);
    //sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_shadow01_01", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_FaZhen_02", 3000, vector3(0, 0, 0), 500, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
    //playsound(290, "Hit03", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill04_ShunShen_01", false);
    //
    //打断
    addbreaksection(1, 510, 2000);
    addbreaksection(10, 510, 2000);
    addbreaksection(21, 510, 2000);
    addbreaksection(100, 510, 2000);
  };

  section(600)//第二段
  {
    animation("Cike_Skill08_02_01")
    {
      speed(1);
    };
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
