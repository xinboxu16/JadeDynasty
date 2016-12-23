

/****    瞬身一段 四阶    ****/

skill(120431)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movecontrol(true);
  };

  section(233)//第一段
  {
    animation("Cike_Skill04_01_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(30, true, 0.22, 0, 0, 25, 0, 0, -40);
    //
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 400);
    //
    //伤害判定
    areadamage(30, 0, 0, 0, 2, false)
		{
			stateimpact("kDefault", 12040101);
      //showtip(200, 0, 1, 0);
		};
    areadamage(80, 0, 0, 0, 2, false)
		{
			stateimpact("kDefault", 12040101);
      //showtip(200, 0, 1, 0);
		};
    //特效
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 0), 10, eular(0, 0, 0), vector3(1, 1, 1), true);
    //sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_001", 3000, vector3(0, 0, 0), 80, eular(0, 0, 0), vector3(1, 1, 1), true);
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_001", 3000, "Bone_Root", 120);
    //
    //音效
    playsound(10, "Sound01", "Sound/Cike/CikeSkillSound01", 2000, "Sound/Cike/Cike_Skill04_ShunShen_01", false);
    playsound(10, "Sound02", "Sound/Cike/CikeSkillSound01", 2000, "Sound/Cike/Cike_Skill04_Voice_01", false);
    //
    //模型消失
    setenable(50, "Visible", false);
    //模型显示
    setenable(200, "Visible", true);
    //
  };

  section(66)//收招
  {
    animation("Cike_Skill04_01_99")
    {
      speed(1);
    };
    //
    //打断
    addbreaksection(1, 1, 1000);
    addbreaksection(1, 10, 1000);
    addbreaksection(1, 11, 1000);
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
