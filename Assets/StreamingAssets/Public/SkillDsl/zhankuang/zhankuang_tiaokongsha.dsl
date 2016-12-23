skill(160901)
{
	section(1400)
	{
		//帧2
		//setanimspeed(66, "zhankuang_xuanfengzhan_01", 0.5);

		//帧4
		//setanimspeed(200, "zhankuang_xuanfengzhan_01", 2);
		
		//帧8
		//setanimspeed(267, "zhankuang_xuanfengzhan_01", 0.5);
		
		//帧11
		//setanimspeed(467, "zhankuang_xuanfengzhan_01", 2);

		//帧15
		//setanimspeed(533, "zhankuang_xuanfengzhan_01", 1);

		//帧21
		//setanimspeed(733, "zhankuang_xuanfengzhan_01", 3);

		//帧27
		//setanimspeed(800, "zhankuang_xuanfengzhan_01", 1);

		//全局参数
		addbreaksection(1, 900, 1400);
		addbreaksection(10, 900, 1400);
		addbreaksection(30, 900, 1400);
		movecontrol(true);
		animation("zhankuang_xuanfengzhan_01");
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		
		startcurvemove(100, true, 0.1, 0, 0, 20, 0, 0, -100, 0.05, 0, 0, 5, 0, 0, 0, 0.05, 0, 0, 30, 0, 0, -100, 0.15, 0, 0, 5, 0, 0, 0, 0.10, 0, 0, 0, 0, 0, 0, 0.05, 0, 0, 40, 0, 0, -100);
		
		//帧3
		setanimspeed(100, "zhankuang_xuanfengzhan_01", 2.5);

		//帧8
		setanimspeed(166, "zhankuang_xuanfengzhan_01", 1);
		
		//帧10
		setanimspeed(233, "zhankuang_xuanfengzhan_01", 2.5);
		
		//帧15
		setanimspeed(300, "zhankuang_xuanfengzhan_01", 1);

		//帧17
		setanimspeed(366, "zhankuang_xuanfengzhan_01", 0.5);

		//帧20
		setanimspeed(566, "zhankuang_xuanfengzhan_01", 3.5);

		//帧27
		setanimspeed(633, "zhankuang_xuanfengzhan_01", 1);

		//帧36
		setanimspeed(933, "zhankuang_xuanfengzhan_01", 0.1);
		//帧38

		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_xuanfengzhan_01_01",1000,"Bone010",110);

		areadamage(110, 0, 1, 0, 3, true) 
		{
			stateimpact("kLauncher", 15010102);
			stateimpact("kDefault", 15010101);
		};

		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_xuanfengzhan_01_02",1000,"Bone010",300);
	
		areadamage(300, 0, 1, 0, 3, true) 
		{
			stateimpact("kLauncher", 15010112);
			stateimpact("kDefault", 15010111);
		};

		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_xuanfengzhan_01_03",1000,"Bone010",566);
	
		areadamage(566, 0, 1, 0, 3, true) 
		{
			stateimpact("kLauncher", 15010122);
			stateimpact("kDefault", 15010121);
		};
	
		lockframe(610, "zhankuang_xuanfengzhan_01", true, 0, 100, 1, 10, true, 1, 1, 1);
		shakecamera2(600, 200, false, true, vector3(0.5, 0.8, 0), vector3(40, 40, 0), vector3(24, 24, 0), vector3(80, 60, 0));
	};

	oninterrupt()
	{
	};

	onstop()
	{
	};
};

skill(160902)
{
	section(1933)
	{
		//全局参数
		addbreaksection(1, 1200, 1933);
		addbreaksection(10, 1200, 1933);
		addbreaksection(30, 1200, 1933);
		movecontrol(true);
		animation("zhankuang_xuanfengzhan_02");
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		
		startcurvemove(150, true, 0.1, 0, 0, 20, 0, 0, -80, 0.17, 0, 0, 0, 0, 0, 0, 0.05, 0, 0, 18, 0, 0, -120, 0.14, 0, 0, 0, 0, 0, 0, 0.05, 0, 0, 18, 0, 0, -120, 0.2, 0, 0, 0, 0, 0, 0, 0.05, 0, 0, 18, 0, 0, -120);
	
		//帧5
		setanimspeed(166, "zhankuang_xuanfengzhan_02", 3);

		//帧8
		setanimspeed(200, "zhankuang_xuanfengzhan_02", 0.5);

		//帧11
		setanimspeed(400, "zhankuang_xuanfengzhan_02", 2);
		
		//帧13
		setanimspeed(433, "zhankuang_xuanfengzhan_02", 3);
		
		//帧16
		setanimspeed(466, "zhankuang_xuanfengzhan_02", 0.5);
		
		//帧18
		setanimspeed(600, "zhankuang_xuanfengzhan_02", 3);
		
		//帧24
		setanimspeed(666, "zhankuang_xuanfengzhan_02", 1);

		//帧30
		setanimspeed(866, "zhankuang_xuanfengzhan_02", 3);

		//帧33
		setanimspeed(900, "zhankuang_xuanfengzhan_02", 2);

		//帧35
		setanimspeed(933, "zhankuang_xuanfengzhan_02", 1);
		
		//帧38
		setanimspeed(1033, "zhankuang_xuanfengzhan_02", 0.066);
		//帧40

		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_xuanfengzhan_02_01",1000,"Bone010",100);

		areadamage(200, 0, 1.5, 1.5, 3, true) 
		{	
			stateimpact("kLauncher", 15010202);
			stateimpact("kDefault", 15010201);
		};

		//lockframe(233, "zhankuang_xuanfengzhan_02", true, 0, 50, 0.5, 10, true, 1, 1, 1);

		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_xuanfengzhan_02_02",1000,"Bone010",366);
	
		areadamage(466, 0, 1.5, 1.5, 3, true) 
		{
			stateimpact("kLauncher", 15010212);
			stateimpact("kDefault", 15010211);
		};
		
		//lockframe(500, "zhankuang_xuanfengzhan_02", true, 0, 50, 0.5, 10, true, 1, 1, 1);

		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_xuanfengzhan_02_01",1000,"Bone010",533);
	
		areadamage(633, 0, 1.5, 1.5, 3, true) 
		{
			stateimpact("kLauncher", 15010222);
			stateimpact("kDefault", 15010221);
		};
	
		//lockframe(666, "zhankuang_xuanfengzhan_02", true, 0, 50, 1, 10, true, 1, 1, 1);

		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_xuanfengzhan_02_02",1000,"Bone010",800);
	
		areadamage(900, 0, 1.5, 1.5, 3, true) 
		{
			stateimpact("kLauncher", 15010232);
			stateimpact("kDefault", 15010231);
		};
		
		//lockframe(933, "zhankuang_xuanfengzhan_02", true, 0, 100, 2, 10, true, 1, 1, 1);

		//setcross2othertime(1000, "stand", 2);
		//setcross2othertime(1000, "run", 0.1);
	};
	
	oninterrupt()
	{
	};

	onstop()
	{
	};
};
