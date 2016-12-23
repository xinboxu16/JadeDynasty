

/****    风刃杀二段    ****/

skill(120312)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movecontrol(true);
  };

  section(33)//起手
  {
    animation("Cike_Skill03_02_01")
    {
      speed(1);
    };
    //
    //特效
    //charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_FengRenShaErDuan_01", 500, "Bone_Root", 1);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_FengRenShaErDuan_01_001", 3000, vector3(0, 0, 0), 10, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //角色移动
    //startcurvemove(100, true, 0.06, 0, 0, 15, 0, 0, 0);
    //
    //自身增加霸体buff
    addimpacttoself(1, 12990001, 500);
  };

  section(66)//第一段
  {
    animation("Cike_Skill03_02_02")
    {
      speed(1);
    };
    //
     //角色移动
    //startcurvemove(0, true, 2, 0, 0, 20, 0, 0, 0);
    //
    //伤害判定
    areadamage(30, 0, 0, 0, 3.5, true)
		{
			stateimpact("kDefault", 12030201);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    //charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_FengRenShaErDuan_01", 500, "Bone_Root", 1);
    //sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_FengRenShaErDuan_01", 3000, vector3(0, 0, 0), 1, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //定帧效果
    //lockframe(30, "Cike_Hit_01_02", true, 0.1, 100, 1, 100);
    //
    //震屏
    //shakecamera2(1, 400, false, true, vector3(0, 0, 1), vector3(0, 0, 400), vector3(0, 0, 0.1), vector3(0, 0, 100));
    //
    //打断
    //addbreaksection(10, 900, 933);
  };

  section(733)//硬直
  {
    animation("Cike_Skill03_02_03")
    {
      speed(1);
    };
    //
    //
    //角色移动
    startcurvemove(0, true, 0.6, 0, 25, 0, 0, -100, 0);
    //
    //打断
    addbreaksection(1, 500, 3000);
    addbreaksection(10, 500, 3000);
    addbreaksection(21, 500, 3000);
  };

  section(1)//收招
  {
    animation("Cike_Skill03_02_99")
    {
      speed(1);
    };
    //
    //打断
    //addbreaksection(10, 499, 900);
  };
};
