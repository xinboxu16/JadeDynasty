skill(1)
{
  section(700)
  {
	//全局参数
    addbreaksection(10, 600, 800);
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
	shakecamera(411, true, 120, 40, 100, 0.03);

	setanimspeed(467, "jianshi_pugong_01", 1);
  };

  section(100)
  {
	movechild(0, "1_JianShi_w_01", "ef_backweapon01");
    movechild(0, "1_JianShi_w_02", "ef_leftweapon02");
    movechild(0, "1_JianShi_w_03", "ef_leftweapon01");
  };
};

skill(2)
{
  section(473)
  {
	//全局参数
    addbreaksection(10, 373, 523);
    movecontrol(true);
    animation("jianshi_pugong_02");

    movechild(0, "1_JianShi_w_02", "ef_backweapon03");
    movechild(0, "1_JianShi_w_03", "ef_backweapon02");
    movechild(0, "1_JianShi_w_01", "ef_rightweapon01");

    startcurvemove(100, true,, 0.05, 0, 0, 0, 0, 0, 160, 0.05, 0, 0, 8, 0, 0, 320);
	setanimspeed(100, "jianshi_pugong_02", 1.5);
	
    charactereffect("Hero_FX/1_jianshi/1_hero_jianshi_pugong_02", 200, "Bone010", 122);

	setanimspeed(167, "jianshi_pugong_02", 3);

	//playsound("Sound/JianShi/JS_PuGong_0302",178);
    areadamage(178, 0, 1.5, 1.5, 3, false) {
      stateimpact("kDefault", 100201);
    };
	
    lockframe(200, "jianshi_pugong_02", true, 0, 100, 1, 150);
	shakecamera(200, true, 120, 40, 100, 0.03);

	setanimspeed(255, "jianshi_pugong_02", 1);
  };

  section(50)
  {
	movechild(0, "1_JianShi_w_01", "ef_backweapon01");
    movechild(0, "1_JianShi_w_02", "ef_leftweapon02");
    movechild(0, "1_JianShi_w_03", "ef_leftweapon01");
  };
};

skill(3)
{
  section(428)
  {
	//全局参数
    addbreaksection(10, 328, 478);
    movecontrol(true);
    animation("jianshi_pugong_03");

    movechild(0, "1_JianShi_w_02", "ef_backweapon03");
    movechild(0, "1_JianShi_w_03", "ef_backweapon02");
    movechild(0, "1_JianShi_w_01", "ef_rightweapon01");

    startcurvemove(100, true, 0.05, 0, 0, 0, 0, 0, 160, 0.05, 0, 0, 8, 0, 0, 320);
	setanimspeed(100, "jianshi_pugong_03", 1.5);

	setanimspeed(167, "jianshi_pugong_03", 3);
	
    charactereffect("Hero_FX/1_jianshi/1_hero_jianshi_pugong_03", 200, "Bone010", 178);
    areadamage(189, 0, 1.5, 1.5, 3, false) {
      stateimpact("kDefault", 100301);
    };

    //sound("Sound/JianShi/JS_PuGong_0303",211);
    lockframe(211, "jianshi_pugong_03", true, 0, 100, 1, 150);
	shakecamera(211, true, 120, 40, 100, 0.03);

	setanimspeed(244, "jianshi_pugong_03", 1);
  };

  section(50)
  {
	movechild(0, "1_JianShi_w_01", "ef_backweapon01");
    movechild(0, "1_JianShi_w_02", "ef_leftweapon02");
    movechild(0, "1_JianShi_w_03", "ef_leftweapon01");
  };
};

skill(4)
{
  section(1963)
  {
	//全局参数
    addbreaksection(10, 1900, 1966);
    movecontrol(true);
    animation("jianshi_pugong_04");

    //movechild(0, "1_JianShi_w_02", "ef_backweapon03");
    //movechild(0, "1_JianShi_w_03", "ef_backweapon02");
    //movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
	movechild(0, "1_JianShi_w_01", "ef_backweapon01");
    movechild(0, "1_JianShi_w_03", "ef_leftweapon01");
    movechild(0, "1_JianShi_w_02", "ef_rightweapon01");

	//movechild(66, "1_JianShi_w_01", "ef_backweapon01");
    //movechild(66, "1_JianShi_w_02", "ef_leftweapon02");
    //movechild(66, "1_JianShi_w_03", "ef_leftweapon01");

	setanimspeed(400, "jianshi_pugong_04", 3);

	setanimspeed(433, "jianshi_pugong_04", 6);
    //movechild(433, "1_JianShi_w_02", "ef_rightweapon01");
    charactereffect("Hero_FX/1_jianshi/1_hero_jianshi_pugong_04", 200, "Bone010", 433);

    //sound("Sound/JianShi/JS_PuGong_0304",439);
	
    areadamage(444, 0, 2, 2, 4, false) {
      stateimpact("kDefault", 100401);
    };
	
	setanimspeed(461, "jianshi_pugong_04", 0.25);
	
	setanimspeed(1130, "jianshi_pugong_04", 1);

    movechild(1866, "1_JianShi_w_02", "ef_leftweapon02");
  };

  section(50)
  {
	movechild(0, "1_JianShi_w_01", "ef_backweapon01");
    movechild(0, "1_JianShi_w_02", "ef_leftweapon02");
    movechild(0, "1_JianShi_w_03", "ef_leftweapon01");
  };
};
