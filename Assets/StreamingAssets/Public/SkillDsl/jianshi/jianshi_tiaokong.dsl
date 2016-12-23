skill(110301)
{
	section(1100)
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


skill(110302)
{
	section(1950)
	{
		//全局参数
		addbreaksection(10, 1900, 1950);
		addbreaksection(30, 1900, 1950);
		movecontrol(true);
		animation("jianshi_kongzhonglianji_01");
		movechild(0, "1_JianShi_w_03", "ef_backweapon02");
		movechild(0, "1_JianShi_w_02", "ef_rightweapon01");
		movechild(0, "1_JianShi_w_01", "ef_backweapon01");

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