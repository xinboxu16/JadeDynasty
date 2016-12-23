skill(110001)
{
	section(800)
	{
		addbreaksection(10, 600, 800);
		addbreaksection(30, 0, 344);
		addbreaksection(30, 489, 800);
		movecontrol(true);
		animation("jianshi_pugong_01");
		
		movechild(0, "1_JianShi_w_02", "ef_backweapon03");
		movechild(0, "1_JianShi_w_03", "ef_backweapon02");
		movechild(66, "1_JianShi_w_01", "ef_rightweapon01");

		startcurvemove(300, true, 0.05, 0, 0, 00, 0, 0, 160, 0.05, 0, 0, 8, 0, 0, 320);
		setanimspeed(300, "jianshi_pugong_01", 1.5);
	
		setanimspeed(367, "jianshi_pugong_01", 3);
		charactereffect("Hero_FX/1_jianshi/1_hero_jianshi_pugong_01", 200, "Bone010", 367);
    
		areadamage(389, 0, 1.5, 1.5, 3, false) 
		{
			stateimpact("kLauncher", 1101012);
			stateimpact("kDefault", 1101011);
		};
		//sound("Sound/JianShi/JS_PuGong_0301",389);

		//lockframe(411, "jianshi_pugong_01", true, 0, 100, 1, 150);
		shakecamera(411, true, 120, 60, 100, 0.03);

		setanimspeed(467, "jianshi_pugong_01", 1);
	};

	oninterrupt()
	{
		movechild(0, "1_JianShi_w_01", "ef_backweapon01");
		movechild(0, "1_JianShi_w_02", "ef_leftweapon02");
		movechild(0, "1_JianShi_w_03", "ef_leftweapon01");
	};

	onstop()
	{
		movechild(0, "1_JianShi_w_01", "ef_backweapon01");
		movechild(0, "1_JianShi_w_02", "ef_leftweapon02");
		movechild(0, "1_JianShi_w_03", "ef_leftweapon01");
	};
};

skill(110002)
{
	section(523)
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
		areadamage(178, 0, 1.5, 1.5, 3, false) 
		{
			stateimpact("kLauncher", 1101022);
			stateimpact("kDefault", 1101021);
		};
	
		//lockframe(200, "jianshi_pugong_02", true, 0, 100, 1, 150);
		shakecamera(200, true, 120, 60, 100, 0.03);

		setanimspeed(255, "jianshi_pugong_02", 1);
	};

	oninterrupt()
	{
		movechild(0, "1_JianShi_w_01", "ef_backweapon01");
		movechild(0, "1_JianShi_w_02", "ef_leftweapon02");
		movechild(0, "1_JianShi_w_03", "ef_leftweapon01");
	};

	onstop()
	{
		movechild(0, "1_JianShi_w_01", "ef_backweapon01");
		movechild(0, "1_JianShi_w_02", "ef_leftweapon02");
		movechild(0, "1_JianShi_w_03", "ef_leftweapon01");
	};
};

skill(110003)
{
	section(478)
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
		areadamage(189, 0, 1.5, 1.5, 3, false) 
		{
			stateimpact("kLauncher", 1101032);
			stateimpact("kDefault", 1101031);
		};

		//sound("Sound/JianShi/JS_PuGong_0303",211);
		//lockframe(211, "jianshi_pugong_03", true, 0, 100, 1, 150);
		shakecamera(211, true, 120, 60, 100, 0.03);

		setanimspeed(244, "jianshi_pugong_03", 1);
	};
	
	oninterrupt()
	{
		movechild(0, "1_JianShi_w_01", "ef_backweapon01");
		movechild(0, "1_JianShi_w_02", "ef_leftweapon02");
		movechild(0, "1_JianShi_w_03", "ef_leftweapon01");
	};

	onstop()
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
		addbreaksection(10, 1900, 1963);
		addbreaksection(30, 0, 400);
		addbreaksection(30, 500, 1963);
		addlockinputtime(10, 1500);
		movecontrol(true);
		animation("jianshi_pugong_04");

		movechild(0, "1_JianShi_w_02", "ef_backweapon03");
		movechild(0, "1_JianShi_w_03", "ef_backweapon02");
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");

		movechild(66, "1_JianShi_w_01", "ef_backweapon01");
		movechild(66, "1_JianShi_w_02", "ef_leftweapon02");
		movechild(66, "1_JianShi_w_03", "ef_leftweapon01");

		setanimspeed(400, "jianshi_pugong_04", 3);

		setanimspeed(433, "jianshi_pugong_04", 6);
		movechild(433, "1_JianShi_w_02", "ef_rightweapon01");
		charactereffect("Hero_FX/1_jianshi/1_hero_jianshi_pugong_04", 200, "Bone010", 433);

		//sound("Sound/JianShi/JS_PuGong_0304",439);
	
		areadamage(444, 0, 2, 2, 4, false) 
		{
		stateimpact("kLauncher", 1101042);
		stateimpact("kDefault", 1101041);
		};
	
		setanimspeed(461, "jianshi_pugong_04", 0.25);
	
		setanimspeed(1130, "jianshi_pugong_04", 1);

		movechild(1866, "1_JianShi_w_02", "ef_leftweapon02");
	};

	oninterrupt()
	{
		movechild(0, "1_JianShi_w_01", "ef_backweapon01");
		movechild(0, "1_JianShi_w_02", "ef_leftweapon02");
		movechild(0, "1_JianShi_w_03", "ef_leftweapon01");
	};

	onstop()
	{
		movechild(0, "1_JianShi_w_01", "ef_backweapon01");
		movechild(0, "1_JianShi_w_02", "ef_leftweapon02");
		movechild(0, "1_JianShi_w_03", "ef_leftweapon01");
	};
};