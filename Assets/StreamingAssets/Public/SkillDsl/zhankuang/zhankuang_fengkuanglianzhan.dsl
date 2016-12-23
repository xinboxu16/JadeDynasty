skill(160701)
{
	section(833)
	{
		//全局参数
		addbreaksection(1, 433, 833);
		addbreaksection(10, 433, 833);
		addbreaksection(30, 433, 833);
		movecontrol(true);
		animation("zhankuang_fengkuanglianzhan_01");

		//帧4
		//setanimspeed(133, "zhankuang_fengkuanglianzhan_01", 2);
		
		//帧8
		//setanimspeed(200, "zhankuang_fengkuanglianzhan_01", 0.5);

		//帧10
		//setanimspeed(433, "zhankuang_fengkuanglianzhan_01", 1);
		//帧22

		playsound(200, "skill0701", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_fengkuanglianzhan_01", false);

		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fengkuanglianzhan_01_01",1000,"Bone010",200);
		
		addimpacttoself(233, 16070101, 5000);

		addimpacttoself(233, 16070102, 5000);
	};
		oninterrupt()
	{
	};

	onstop()
	{
	};
};

skill(160702)
{
  section(400)
	{
		//全局参数
		addbreaksection(1, 160, 400);
		addbreaksection(10, 160, 400);
		addbreaksection(20, 0, 400);
		addbreaksection(30, 160, 400);
		movecontrol(true);
		animation("zhankuang_fengkuanglianzhan_02");

		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");

		startcurvemove(120, true, 0.05, 0, 0, 25, 0, 0, -300);
		
		//帧1
		setanimspeed(33, "zhankuang_fengkuanglianzhan_02", 3);

		//帧10
		setanimspeed(133, "zhankuang_fengkuanglianzhan_02", 1);
		//帧11
	
		areadamage(140, 0, 1, 1, 2.2, false) 
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kLauncher", 16070202);
			stateimpact("kDefault", 16070201);
		};
		playsound(120, "skill0702", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_01_new", false);
		
		playsound(160, "hit0702", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		};
		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fengkuanglianzhan_04_01",1000,"Bone010",130);
	};

	oninterrupt()
	{
	};

	onstop()
	{
	};
};

skill(160703)
{
  section(400)
	{
		//全局参数
		addbreaksection(1, 150, 350);
		addbreaksection(10, 150, 350);
		addbreaksection(20, 150, 350);
		addbreaksection(30, 150, 350);
		movecontrol(true);
		animation("zhankuang_fengkuanglianzhan_03");

		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		
		startcurvemove(40, true, 0.05, 0, 0, 25, 0, 0, -300);
		
		//帧1
		setanimspeed(33, "zhankuang_fengkuanglianzhan_03", 3.5);
		
		//帧8
		setanimspeed(100, "zhankuang_fengkuanglianzhan_03", 0.1);
		//帧9
		
		areadamage(40, 0, 1, 1, 2.2, false) 
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kLauncher", 16070302);
			stateimpact("kDefault", 16070301);
		};

		playsound(20, "skill0703", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_02", false);
		
		playsound(90, "hit0703", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		};

		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fengkuanglianzhan_04_02",1000,"Bone010",30);
	};

	oninterrupt()
	{
	};

	onstop()
	{
	};
};

skill(160704)
{
  section(800)
	{
		//全局参数
		addbreaksection(1, 350, 800);
		addbreaksection(10, 350, 800);
		addbreaksection(20, 0, 800);
		addbreaksection(30, 350, 800);
		movecontrol(true);
		animation("zhankuang_fengkuanglianzhan_04");

		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		
		startcurvemove(80, true, 0.15, 0, 0, 10, 0, 0, -20);
		
		//帧1
		setanimspeed(33, "zhankuang_fengkuanglianzhan_04", 2.5);
		
		//帧6
		setanimspeed(100, "zhankuang_fengkuanglianzhan_04", 1);
		
		//帧8
		setanimspeed(166, "zhankuang_fengkuanglianzhan_04", 3);

		//帧14
		setanimspeed(233, "zhankuang_fengkuanglianzhan_04", 0.1);
		//帧16
		
		areadamage(170, 0, 1, 1, 2.4, false) 
		{
			stateimpact("kLauncher", 16070402);
			stateimpact("kDefault", 16070401);
		};

		areadamage(190, 0, 1, 2.5, 2.4, false) 
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kLauncher", 16070402);
			stateimpact("kDefault", 16070401);
		};
		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fengkuanglianzhan_04_05",1000,"Bone010",160);
		
		playsound(170, "skill0704", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_05", false);
		
		playsound(170, "skill070402", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_01_voice", false);

		playsound(220, "hit0704", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		};
	
	};
		oninterrupt()
	{
	};

	onstop()
	{
	};
};

skill(160705)
{
  section(433)
	{
		//全局参数
		addbreaksection(1, 150, 433);
		addbreaksection(10, 150, 433);
		addbreaksection(20, 0, 733);
		addbreaksection(30, 150, 433);
		movecontrol(true);
		animation("zhankuang_fengkuanglianzhan_05");

		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		
		startcurvemove(90, true, 0.05, 0, 0, 25, 0, 0, -300);
		
		//帧1
		setanimspeed(33, "zhankuang_fengkuanglianzhan_05", 3);
		
		//帧4
		setanimspeed(66, "zhankuang_fengkuanglianzhan_05", 1);
		
		//帧5
		setanimspeed(100, "zhankuang_fengkuanglianzhan_05", 4);

		//帧9
		setanimspeed(133, "zhankuang_fengkuanglianzhan_05", 1);
		//帧10
		
		areadamage(110, 0, 1, 1, 2.2, false) 
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kLauncher", 16070502);
			stateimpact("kDefault", 16070501);
		};
		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fengkuanglianzhan_04_03",1000,"Bone010",100);
	
		playsound(100, "skill0705", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_03", false);
		
		playsound(160, "hit0705", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		};
	};
		oninterrupt()
	{
	};

	onstop()
	{
	};
};

skill(160706)
{
  section(733)
	{
		//全局参数
		addbreaksection(1, 150, 733);
		addbreaksection(10, 150, 733);
		addbreaksection(20, 0, 733);
		addbreaksection(30, 150, 733);
		movecontrol(true);
		animation("zhankuang_fengkuanglianzhan_06");

		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		
		startcurvemove(70, true, 0.05, 0, 0, 25, 0, 0, -300);
		
		//帧1
		setanimspeed(33, "zhankuang_fengkuanglianzhan_06", 2);
		
		//帧3
		setanimspeed(66, "zhankuang_fengkuanglianzhan_06", 3);

		//帧9
		setanimspeed(133, "zhankuang_fengkuanglianzhan_06", 0.1);
		//帧11
		
		areadamage(70, 0, 1, 1, 2.2, false) 
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kLauncher", 16070602);
			stateimpact("kDefault", 16070601);
		};
		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fengkuanglianzhan_04_04",1000,"Bone010",60);
	
		playsound(60, "skill0705", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_04", false);
		
		playsound(120, "hit0705", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		};
	
	};
		oninterrupt()
	{
	};

	onstop()
	{
	};
};

skill(160707)
{
	section(1000)
	{
		//全局参数
		addbreaksection(1, 400, 1000);
		addbreaksection(10, 300, 1000);
		addbreaksection(30, 300, 1000);
		movecontrol(true);
		animation("zhankuang_fengkuanglianzhan_07");

		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		
		startcurvemove(100, true, 0.1, 0, 0, 20, 0, 0, -100);
		
		//帧1
		setanimspeed(33, "zhankuang_fengkuanglianzhan_07", 2.5);
		
		//帧6
		setanimspeed(100, "zhankuang_fengkuanglianzhan_07", 1);
		
		//帧8
		setanimspeed(166, "zhankuang_fengkuanglianzhan_07", 8);

		//帧16
		setanimspeed(200, "zhankuang_fengkuanglianzhan_07", 0.2);
		//帧22
		
		areadamage(170, 0, 1.5, 1.5, 3, false) 
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kLauncher", 16070702);
			stateimpact("kDefault", 16070701);
		};
		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fenglunzhan_01_03",1000,"Bone010",160);
	
		sceneeffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fenglunzhan_01_04",1000,vector3(0,0,2.5),180,eular(0,0,0),vector3(1,1,1),true);
	
		shakecamera2(190, 100, false, true, vector3(0.5, 0.8, 0), vector3(50, 50, 0), vector3(12, 16, 0), vector3(80, 60, 0));
	};
	
	oninterrupt()
	{
	};

	onstop()
	{
	};
};

skill(1607021)
{
  section(400)
	{
		//全局参数
		addbreaksection(1, 160, 400);
		addbreaksection(10, 160, 400);
		addbreaksection(20, 0, 400);
		addbreaksection(30, 160, 400);
		movecontrol(true);
		animation("zhankuang_fengkuanglianzhan_02");

		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");

		startcurvemove(120, true, 0.05, 0, 0, 25, 0, 0, -300);
		
		//帧1
		setanimspeed(33, "zhankuang_fengkuanglianzhan_02", 3);

		//帧10
		setanimspeed(133, "zhankuang_fengkuanglianzhan_02", 1);
		//帧11
	
		areadamage(140, 0, 1, 1, 2.2, false) 
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kLauncher", 16070202);
			stateimpact("kDefault", 16070201);
		};
		playsound(120, "skill0702", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_01_new", false);
		
		playsound(160, "hit0702", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		};
		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fengkuanglianzhan_04_01",1000,"Bone010",130);
	};

	oninterrupt()
	{
	};

	onstop()
	{
	};
};

skill(1607031)
{
  section(400)
	{
		//全局参数
		addbreaksection(1, 150, 350);
		addbreaksection(10, 150, 350);
		addbreaksection(20, 150, 350);
		addbreaksection(30, 150, 350);
		movecontrol(true);
		animation("zhankuang_fengkuanglianzhan_03");

		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		
		startcurvemove(40, true, 0.05, 0, 0, 25, 0, 0, -300);
		
		//帧1
		setanimspeed(33, "zhankuang_fengkuanglianzhan_03", 3.5);
		
		//帧8
		setanimspeed(100, "zhankuang_fengkuanglianzhan_03", 0.1);
		//帧9
		
		areadamage(40, 0, 1, 1, 2.2, false) 
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kLauncher", 16070302);
			stateimpact("kDefault", 16070301);
		};
		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fengkuanglianzhan_04_02",1000,"Bone010",30);
		playsound(20, "skill0703", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_02", false);
		
		playsound(90, "hit0703", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		};
	};

	oninterrupt()
	{
	};

	onstop()
	{
	};
};

skill(1607041)
{
  section(800)
	{
		//全局参数
		addbreaksection(1, 350, 800);
		addbreaksection(10, 350, 800);
		addbreaksection(20, 0, 800);
		addbreaksection(30, 350, 800);
		movecontrol(true);
		animation("zhankuang_fengkuanglianzhan_04");

		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		
		startcurvemove(80, true, 0.15, 0, 0, 10, 0, 0, -20);
		
		//帧1
		setanimspeed(33, "zhankuang_fengkuanglianzhan_04", 2.5);
		
		//帧6
		setanimspeed(100, "zhankuang_fengkuanglianzhan_04", 1);
		
		//帧8
		setanimspeed(166, "zhankuang_fengkuanglianzhan_04", 3);

		//帧14
		setanimspeed(233, "zhankuang_fengkuanglianzhan_04", 0.1);
		//帧16
		
		areadamage(170, 0, 1, 1, 2.4, false) 
		{
			stateimpact("kLauncher", 16070402);
			stateimpact("kDefault", 16070401);
		};

		areadamage(190, 0, 1, 3, 2.4, false) 
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kLauncher", 16070402);
			stateimpact("kDefault", 16070401);
		};
		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fengkuanglianzhan_04_05",1000,"Bone010",160);
		
		playsound(170, "skill0704", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_05", false);
		
		playsound(170, "skill070402", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_01_voice", false);

		playsound(220, "hit0704", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		};
	
	};
		oninterrupt()
	{
	};

	onstop()
	{
	};
};

skill(1607051)
{
  section(433)
	{
		//全局参数
		addbreaksection(1, 150, 433);
		addbreaksection(10, 150, 433);
		addbreaksection(20, 0, 733);
		addbreaksection(30, 150, 433);
		movecontrol(true);
		animation("zhankuang_fengkuanglianzhan_05");

		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		
		startcurvemove(90, true, 0.05, 0, 0, 25, 0, 0, -300);
		
		//帧1
		setanimspeed(33, "zhankuang_fengkuanglianzhan_05", 3);
		
		//帧4
		setanimspeed(66, "zhankuang_fengkuanglianzhan_05", 1);
		
		//帧5
		setanimspeed(100, "zhankuang_fengkuanglianzhan_05", 4);

		//帧9
		setanimspeed(133, "zhankuang_fengkuanglianzhan_05", 1);
		//帧10
		
		areadamage(110, 0, 1, 1, 2.2, false) 
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kLauncher", 16070502);
			stateimpact("kDefault", 16070501);
		};
		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fengkuanglianzhan_04_03",1000,"Bone010",100);

		playsound(100, "skill0705", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_03", false);
		
		playsound(160, "hit0705", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		};
	};
		oninterrupt()
	{
	};

	onstop()
	{
	};
};

skill(1607061)
{
  section(733)
	{
		//全局参数
		addbreaksection(1, 150, 733);
		addbreaksection(10, 150, 733);
		addbreaksection(20, 0, 733);
		addbreaksection(30, 150, 733);
		movecontrol(true);
		animation("zhankuang_fengkuanglianzhan_06");

		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		
		startcurvemove(70, true, 0.05, 0, 0, 25, 0, 0, -300);
		
		//帧1
		setanimspeed(33, "zhankuang_fengkuanglianzhan_06", 2);
		
		//帧3
		setanimspeed(66, "zhankuang_fengkuanglianzhan_06", 3);

		//帧9
		setanimspeed(133, "zhankuang_fengkuanglianzhan_06", 0.1);
		//帧11
		
		areadamage(70, 0, 1, 1, 2.2, false) 
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kLauncher", 16070602);
			stateimpact("kDefault", 16070601);
		};
		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fengkuanglianzhan_04_04",1000,"Bone010",60);

		playsound(60, "skill0705", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_04", false);
		
		playsound(120, "hit0705", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		};
	};
		oninterrupt()
	{
	};

	onstop()
	{
	};
};


skill(1607052)
{
  section(433)
	{
		//全局参数
		addbreaksection(1, 150, 433);
		addbreaksection(10, 150, 433);
		addbreaksection(20, 0, 733);
		addbreaksection(30, 150, 433);
		movecontrol(true);
		animation("zhankuang_fengkuanglianzhan_05");

		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		
		startcurvemove(90, true, 0.05, 0, 0, 25, 0, 0, -300);
		
		//帧1
		setanimspeed(33, "zhankuang_fengkuanglianzhan_05", 3);
		
		//帧4
		setanimspeed(66, "zhankuang_fengkuanglianzhan_05", 1);
		
		//帧5
		setanimspeed(100, "zhankuang_fengkuanglianzhan_05", 4);

		//帧9
		setanimspeed(133, "zhankuang_fengkuanglianzhan_05", 1);
		//帧10
		playsound(100, "skill0705", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_03", false);
		
		playsound(160, "hit0705", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		};
		areadamage(110, 0, 1, 1, 2.2, false) 
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kLauncher", 16070502);
			stateimpact("kDefault", 16070501);
		};
		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fengkuanglianzhan_04_03",1000,"Bone010",100);
	};
		oninterrupt()
	{
	};

	onstop()
	{
	};
};

skill(1607062)
{
  section(733)
	{
		//全局参数
		addbreaksection(1, 150, 733);
		addbreaksection(10, 150, 733);
		addbreaksection(20, 0, 733);
		addbreaksection(30, 150, 733);
		movecontrol(true);
		animation("zhankuang_fengkuanglianzhan_06");

		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		
		startcurvemove(70, true, 0.05, 0, 0, 25, 0, 0, -300);
		
		//帧1
		setanimspeed(33, "zhankuang_fengkuanglianzhan_06", 2);
		
		//帧3
		setanimspeed(66, "zhankuang_fengkuanglianzhan_06", 3);

		//帧9
		setanimspeed(133, "zhankuang_fengkuanglianzhan_06", 0.1);
		//帧11
		
		areadamage(70, 0, 1, 1, 2.2, false) 
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kLauncher", 16070602);
			stateimpact("kDefault", 16070601);
		};
		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fengkuanglianzhan_04_04",1000,"Bone010",60);

		playsound(60, "skill0705", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_04", false);
		
		playsound(120, "hit0705", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		};
	};
		oninterrupt()
	{
	};

	onstop()
	{
	};
};
