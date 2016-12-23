

/****    普攻四段    ****/

skill(120004)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);
    //
    //瞬身类技能打断
    addbreaksection(11, 1, 30000);
    //
  };

  section(200)//第一段
  {
    animation("Cike_Hit_04_01")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(1, true, 0.12, 0, 0, 16, 0, 0, 0);
    //
  };
  section(166)//第二段
  {
    animation("Cike_Hit_04_02")
    {
      speed(1);
    };
    //
    //角色移动
    startcurvemove(1, true, 0.1, 0, 0, 16, 0, 0, -30);
    //
    //伤害判定
    areadamage(10, 0, 1.5, 1.5, 2.8, true)
		{
			stateimpact("kDefault", 12000401);
			stateimpact("kLauncher", 12000402);
      //showtip(200, 0, 1, 0);
		};
    //
    //特效
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_PuGong_04_001", 500, "Bone_Root", 1);
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_PuGong_04_002", 500, "Bone_Root", 1);
    //
    //音效
    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_08", false);
    playsound(20, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/guaiwu_shouji_02", true);
    playsound(20, "Hit03", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_Hit_Voice_01", false);
    //
    //定帧效果
    //lockframe(30, "Cike_Hit_01_02", true, 0.1, 100, 1, 100);
    //
    //震屏
    //shakecamera2(1, 400, false, true, vector3(0, 0, 1), vector3(0, 0, 400), vector3(0, 0, 0.1), vector3(0, 0, 100));
    //
    //残影
    //createshadow(1, 50, 1, 200, 600, "3_Cike_02", "Transparent/Cutout/Soft Edge Unlit", 0.8)
    //{
    //   ignorelist(3_Hero_CiKe);
    //};
    //
    //模型消失
    //setenable(0, "Visible", false);
    //setenable(100, "Visible", true);
  };

  section(333)//硬直
  {
    animation("Cike_Hit_04_03")
    {
      speed(1);
    };
    //
    //打断
    addbreaksection(10, 300, 2000);
    addbreaksection(21, 300, 2000);
    addbreaksection(100, 300, 2000);
  };

  section(166)//收招
  {
    animation("Cike_Hit_04_99")
    {
      speed(1);
    };
    //
  };
};
