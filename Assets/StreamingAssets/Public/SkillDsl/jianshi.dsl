skill(110001)
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

skill(110002)
{
  section(473)
  {
	//全局参数
    addbreaksection(10, 373, 523);
    addbreaksection(30, 0, 155);
    addbreaksection(30, 278, 523);
    movecontrol(true);
    animation("jianshi_pugong_02");

    movechild(0, "1_JianShi_w_02", "ef_backweapon03");
    movechild(0, "1_JianShi_w_03", "ef_backweapon02");
    movechild(0, "1_JianShi_w_01", "ef_rightweapon01");

    startcurvemove(100, true, 0.05, 0, 0, 0, 0, 0, 160, 0.05, 0, 0, 8, 0, 0, 320);
	setanimspeed(100, "jianshi_pugong_02", 1.5);
	
    charactereffect("Hero_FX/1_jianshi/1_hero_jianshi_pugong_02", 200, "Bone010", 122);

	setanimspeed(167, "jianshi_pugong_02", 3);

	//sound("Sound/JianShi/JS_PuGong_0302",178);
    areadamage(178, 0, 1.5, 1.5, 3, false) {
      stateimpact("kDefault", 100201);
    };
	
    lockframe(200, "jianshi_pugong_02", true, 0, 100, 1, 150);
	shakecamera(200, true, 120, 60, 100, 0.03);

	setanimspeed(255, "jianshi_pugong_02", 1);
  };

  section(50)
  {
	movechild(0, "1_JianShi_w_01", "ef_backweapon01");
    movechild(0, "1_JianShi_w_02", "ef_leftweapon02");
    movechild(0, "1_JianShi_w_03", "ef_leftweapon01");
  };
};

skill(110003)
{
  section(428)
  {
	//全局参数
    addbreaksection(10, 328, 478);
    addbreaksection(30, 0, 155);
    addbreaksection(30, 255, 478);
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
	shakecamera(211, true, 120, 60, 100, 0.03);

	setanimspeed(244, "jianshi_pugong_03", 1);
  };

  section(50)
  {
	movechild(0, "1_JianShi_w_01", "ef_backweapon01");
    movechild(0, "1_JianShi_w_02", "ef_leftweapon02");
    movechild(0, "1_JianShi_w_03", "ef_leftweapon01");
  };
};

skill(110004)
{
  section(1963)
  {
	//全局参数
    addbreaksection(10, 1900, 1966);
    addbreaksection(30, 0, 400);
    addbreaksection(30, 500, 1966);
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

skill(110201)
{
  section(1050)
  {
	//全局参数
    addbreaksection(10, 700, 1100);
    addbreaksection(30, 700, 1100);
    movecontrol(true);
    animation("jianshi_jianchong_01");
	
	setanimspeed(100, "jianshi_jianchong_01", 1.5);
    movechild(100, "1_JianShi_w_01", "ef_rightweapon01");
	sceneeffect("Hero_FX/1_jianshi/1_hero_jianshi_jianchong_02",1000,vector3(0,0,0),100);
	
	//sound("Sound/JianShi/JianChong_02",233);
	setanimspeed(250, "jianshi_jianchong_01", 1);

  startcurvemove(350, true, 0.1, 0, 0, 0, 0, 0, 900, 0.2, 0, 0, 15, 0, 0, -75);
	charactereffect("Hero_FX/1_jianshi/1_hero_jianshi_jianchong_01",1000,"Bone010",350);
	
	shakecamera(389, true, 120, 40, 100, 0.03);
    areadamage(389, 0, 2, -1.25, 2.5, false) 
	{
      stateimpact("kDefault", 110101);
    };

    areadamage(422, 0, 1, 0.5, 1.25, false) 
	{
      stateimpact("kDefault", 110102);
    };

	areadamage(455, 0, 1, 0.25, 1, false) 
	{
      stateimpact("kDefault", 110103);
    };

	areadamage(522, 0, 1, 1.5, 1, false) 
	{
      stateimpact("kDefault", 110104);
    };
  };

  section(50)
  {
	movechild(0, "1_JianShi_w_01", "ef_backweapon01");
    movechild(0, "1_JianShi_w_02", "ef_leftweapon02");
    movechild(0, "1_JianShi_w_03", "ef_leftweapon01");
  };
};

skill(110301)
{
  section(1050)
  {
	//全局参数
    addbreaksection(10, 600, 1100);
    addbreaksection(30, 600, 1100);
    movecontrol(true);
    animation("jianshi_tiaokong_01");
	
    movechild(100, "1_JianShi_w_01", "ef_rightweapon01");

    startcurvemove(166, true, 0.1, 0, 0, 0, 0, 0, 400, 0.1, 0, 20, 0, 0, -200, 0, 0.1, 0, 0, 0, 0, -200, 0);

	areadamage(267, 0, 1, 1, 3, true) 
	{
      stateimpact("kDefault", 1103011);
    };

	setanimspeed(533, "jianshi_tiaokong_01",0.5);
  };

  section(50)
  {
	movechild(0, "1_JianShi_w_01", "ef_backweapon01");
    movechild(0, "1_JianShi_w_02", "ef_leftweapon02");
    movechild(0, "1_JianShi_w_03", "ef_leftweapon01");
  };
};


skill(110302)
{
  section(1850)
  {
	//全局参数
    addbreaksection(10, 1900, 2000);
    addbreaksection(30, 1900, 2000);
    movecontrol(true);
    animation("jianshi_kongzhonglianji_01");
    movechild(0, "1_JianShi_w_03", "ef_backweapon02");

    startcurvemove(33, true, 0.03 ,0 ,0 ,0 ,0 ,3000 ,0 ,0.03 ,0 ,90 ,0 ,0 ,-3000 ,0 ,0.023 ,0 ,0 ,0 ,0 ,0 ,0 ,0.1 ,0 ,0 ,0 ,0 ,0 ,0 ,0.03 ,0 ,0 ,0 ,0 ,500 ,0 ,0.03 ,0 ,15 ,0 ,0 ,-500 ,0 ,0.073 ,0 ,0 ,0 ,0 ,0 ,0 ,0.1 ,0 ,0 ,0 ,0 ,0 ,0 ,0.03 ,0 ,0 ,0 ,0 ,500 ,0 ,0.03 ,0 ,15 ,0 ,0 ,-500 ,0 ,0.057 ,0 ,0 ,0 ,0 ,0 ,0 ,0.1 ,0 ,0 ,0 ,0 ,0 ,0 ,0.03 ,0 ,0 ,0 ,0 ,500 ,0 ,0.03 ,0 ,15 ,0 ,0 ,-500 ,0 ,0.073 ,0 ,0 ,0 ,0 ,0 ,0 ,0.1 ,0 ,0 ,0 ,0 ,0 ,0 ,0.1 ,0 ,0 ,0 ,0 ,100 ,0 ,0.067 ,0 ,10 ,0 ,0 ,-100 ,0 ,0.467 ,0 ,6 ,0 ,0 ,-20 ,0 ,0.1 ,0 ,-80 ,0 ,0 ,-400 ,0);
	
	setanimspeed(66, "jianshi_kongzhonglianji_01",2);
	
	areadamage(100, 0, 1, 0, 3.5, true) 
	{
      stateimpact("kDefault", 1103021);
    };

	setanimspeed(117, "jianshi_kongzhonglianji_01",1);

	setanimspeed(217, "jianshi_kongzhonglianji_01",2);

	areadamage(317, 0, 1, 0, 3.5, true) 
	{
      stateimpact("kDefault", 1103022);
    };

	setanimspeed(350, "jianshi_kongzhonglianji_01",1);

	setanimspeed(450, "jianshi_kongzhonglianji_01",2);
	
	areadamage(517, 0, 1, 0, 3.5, true) 
	{
      stateimpact("kDefault", 1103023);
    };

	setanimspeed(567, "jianshi_kongzhonglianji_01",1);

	setanimspeed(667, "jianshi_kongzhonglianji_01",2);

	areadamage(733, 0, 1, 0, 3.5, true) 
	{
      stateimpact("kDefault", 1103024);
    };

	setanimspeed(800, "jianshi_kongzhonglianji_01",1);

	setanimspeed(900, "jianshi_kongzhonglianji_01",2);
	
	setanimspeed(1017, "jianshi_kongzhonglianji_01",0.5);

	setanimspeed(1483, "jianshi_kongzhonglianji_01",3);
	
	areadamage(1494, 0, 1, 0, 3.5, true) 
	{
      stateimpact("kDefault", 1103025);
    };
	
	setanimspeed(1550, "jianshi_kongzhonglianji_01",1);
	
	shakecamera2(1783, 160, false, false, vector3(0.2, 0, 0), vector3(80, 0, 0), vector3(3, 0, 0), vector3(100, 0, 0));
	areadamage(1783, 0, 1, 0, 4.5, true) 
	{
      stateimpact("kDefault", 1103026);
    };
  };

  section(50)
  {
	movechild(0, "1_JianShi_w_01", "ef_backweapon01");
    movechild(0, "1_JianShi_w_02", "ef_leftweapon02");
    movechild(0, "1_JianShi_w_03", "ef_leftweapon01");
  };
};


skill(110401)
{
  section(1250)
  {
	//全局参数
    addbreaksection(10, 900, 1300);
    addbreaksection(30, 900, 1300);
    movecontrol(true);
    animation("zhankuang_fenglunzhan_03");
	
    startcurvemove(33, true, 0.1, 0, 40, 10, 0, -400, 0, 0.5, 0, 0, 10, 0, -16, 0);

	charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fenglunzhan_03_01",1000,"Bone010",166);
	colliderdamage(200, 500, true, true, 100, 0)
    {
       stateimpact("kDefault", 1502031);
	   bonecollider("hero/5_zhankuang/fenglunzhancollider","Bone010", true);
    };

	sceneeffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fenglunzhan_03_02",1000,vector3(0,0,1.8),733);
	areadamage(733, 0, 1, 0, 3, true) 
	{
      stateimpact("kDefault", 1502032);
    };
  };

  section(50)
  {
	movechild(0, "1_JianShi_w_01", "ef_backweapon01");
    movechild(0, "1_JianShi_w_02", "ef_leftweapon02");
    movechild(0, "1_JianShi_w_03", "ef_leftweapon01");
  };
};