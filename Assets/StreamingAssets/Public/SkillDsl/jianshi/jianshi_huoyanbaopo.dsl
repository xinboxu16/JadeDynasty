skill(110701)
{
  section(700)
  {
	//全局参数
    addbreaksection(10, 600, 800);
    addbreaksection(30, 0, 344);
    addbreaksection(30, 489, 800);
    movecontrol(true);
    animation("jianshi_pugong_01");

    movechild(0, "1_JianShi_w_02", "ef_backweapon03");
    movechild(0, "1_JianShi_w_03", "ef_backweapon02");

    movechild(66, "1_JianShi_w_01", "ef_rightweapon01");

    startcurvemove(300, true, 0.05, 0, 0, 0, 0, 0, 160, 0.05, 0, 0, 8, 0, 0, 320);
	setanimspeed(300, "jianshi_pugong_01", 1.5);
	
	setanimspeed(367, "jianshi_pugong_01", 3);
    charactereffect("Hero_FX/1_jianshi/1_hero_jianshi_pugong_01", 200, "Bone010", 367);
    
	areadamage(389, 0, 1.5, 1.5, 3, false) 
	{
      stateimpact("kDefault", 100101);
    };
	//sound("Sound/JianShi/JS_PuGong_0301",389);

    lockframe(411, "jianshi_pugong_01", true, 0, 100, 1, 150);
	shakecamera(411, true, 120, 60, 100, 0.03);

	setanimspeed(467, "jianshi_pugong_01", 1);
  };

  section(100)
  {
	movechild(0, "1_JianShi_w_01", "ef_backweapon01");
    movechild(0, "1_JianShi_w_02", "ef_leftweapon02");
    movechild(0, "1_JianShi_w_03", "ef_leftweapon01");
  };
};
