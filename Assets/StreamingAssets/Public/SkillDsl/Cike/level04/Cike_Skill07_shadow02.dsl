

/****    瞬狱影杀阵影子攻击1    ****/

skill(125402)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //设定方向为施法者方向
    //settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
		//findmovetarget(0, vector3(0, 0, 1), 2.5, 60, 0.1, 0.9, 0, -0.8);

    setlifetime(0, 1000);
  };

  section(500)//起手
  {
    animation("Cike_Skill06_shadow01_01")
    {
      speed(1);
    };

    crosssummonmove(0, true, 0, 10, 30);
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_Cike_TuoWei_01", 3000, "ef_body", 0);
    //sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_ShadowXian_01", 3000, vector3(0, 0, 5), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
  };

  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    //模型消失
    setenable(0, "Visible", true);
    //destroyself(0);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 1.5), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
  };

  onstop() //技能正常结束时会运行该段逻辑
  {
    //模型消失
    setenable(0, "Visible", true);
    //destroyself(0);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 1.5), 0, eular(0, 0, 0), vector3(1, 1, 1), true);
  };
};
