skill(160301)
{
	section(900)
	{
			//全局参数
		addbreaksection(1, 600, 900);
		addbreaksection(10, 266, 900);
		addbreaksection(20, 266, 900);
		addbreaksection(30, 266, 900);
		movecontrol(true);
		animation("zhankuang_shanshen_01_01");

		//帧11
		setanimspeed(366, "zhankuang_shanshen_01_01", 4);

		//帧15
		setanimspeed(400, "zhankuang_shanshen_01_01", 1);

		//帧16
		setanimspeed(433, "zhankuang_shanshen_01_01", 0.25);

		//帧18
		setanimspeed(700, "zhankuang_shanshen_01_01", 1);
		//帧27
		
		addimpacttoself(0, 16030101, -1);		
		
		startcurvemove(0, true, 0.15, 0, 0, 47.5, 0, 0, -100);

		//setenable(0, "Visible", false);

		//setenable(100, "Visible", true);

		areadamage(400, 0, 1, 0, 3, true) 
		{
			stateimpact("kDefault", 16030103);
		};	

		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_shanshen_01_01", 500, "Bone010", 33);

		sceneeffect("Hero_FX/5_zhankuang/5_hero_zhankuang_shanshen_01_02",1000,vector3(0,0,0),370);
		
	};

	oninterrupt()
	{
		
	};

	onstop()
	{
	};
};

skill(160302)
{
	section(1466)
	{
		//全局参数
		addbreaksection(1, 420, 1466);
		addbreaksection(10, 420, 1466);
		addbreaksection(20, 0, 1466);
		addbreaksection(30, 420, 1466);
		movecontrol(true);
		animation("zhankuang_shanshen_02");
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		
		findmovetarget(0, vector3(0, 0, 8), 3, 60, 0.8, 0.2, 0, -1);
		startcurvemove(50, true, 0.2, 0, 0, 25, 0, 0, 0);

		startcurvemove(250, true, 0.05, 0, 0, 20, 0, 0, -200, 0.5, 0, 0, 10, 0, 0, -20);
		
		//帧1
		setanimspeed(33, "zhankuang_shanshen_02", 4);
		
		//帧29
		setanimspeed(266, "zhankuang_shanshen_02", 7);

		//帧36
		setanimspeed(300, "zhankuang_shanshen_02", 1);
		//帧72
		
		addimpacttoself(0, 16030102, -1);	

		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_shanshen_02_01", 1000, "Bone010", 180);

		areadamage(100, 0, 1, 0, 3.5, true) 
		{
			stateimpact("kDefault", 16030201);
		};		

		areadamage(200, 0, 1, 0, 3.5, true) 
		{
			stateimpact("kDefault", 16030201);
		};		

		areadamage(300, 0, 1, 0, 3.5, true) 
		{
			stateimpact("kDefault", 16030201);
		};		
	};
	
	oninterrupt()
	{
	};

	onstop()
	{
	};
};