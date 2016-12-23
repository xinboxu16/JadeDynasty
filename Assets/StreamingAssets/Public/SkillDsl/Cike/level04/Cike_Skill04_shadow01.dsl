

/****    瞬身术影子攻击    ****/

skill(125301)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //设定方向为施法者方向
    settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
		//findmovetarget(0, vector3(0, 0, 1), 2.5, 60, 0.1, 0.9, 0, -0.8);
  };

  section(500)//延迟
  {
    animation("Cike_Stand")
    {
      speed(1);
    };
  };

  section(200)//起手
  {
    animation("Cike_Skill06_shadow01_01")
    {
      speed(1);
    };
  };

  section(366)//第一段
  {
    animation("Cike_Skill06_shadow01_02")
    {
      speed(1);
    };
    //
    //伤害判定
    areadamage(10, 0, 1.5, 0, 2.2, true)
		{
			stateimpact("kDefault", 12060101);
			stateimpact("kLauncher", 12060102);
			stateimpact("kKnockDown", 12990000);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_ShadowHit_02", 500, "Bone_Root", 1);
    //sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 1.5), 320, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Skill06_YingXi_01", false);
    playsound(10, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_01", true);
  };

  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    //模型消失
    setenable(0, "Visible", true);
    destroyself(0);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
  };

  onstop() //技能正常结束时会运行该段逻辑
  {
    //模型消失
    setenable(0, "Visible", true);
    destroyself(0);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 0), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
  };
};
