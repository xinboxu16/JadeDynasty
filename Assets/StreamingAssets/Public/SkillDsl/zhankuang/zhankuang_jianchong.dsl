skill(160801)
{
	section(900)
	{
		//全局参数
		addbreaksection(1, 1800, 1950);
		addbreaksection(10, 1800, 1950);
		addbreaksection(20, 0, 1950);
		addbreaksection(30, 900, 1950);
		movecontrol(true);
		animation("zhankuang_chongzhuang_02");
		movechild(0, "1_JianShi_w_01", "ef_backweapon01");

		//帧1
		setanimspeed(33, "zhankuang_chongzhuang_02", 0.5);
		
		//帧4
		setanimspeed(233, "zhankuang_chongzhuang_02", 3);
		
		//帧13
		setanimspeed(333, "zhankuang_chongzhuang_02", 1);
		//帧49
		
		findmovetarget(0, vector3(0, 0, 0), 2, 180, 0.8, 0.2, 0, 0, false);
		startcurvemove(10, true, 0.1, 0, 0, -2, 0, 0, 10);
		
		areadamage(333, 0, 0, 1, 2.5, false) 
		{
			stateimpact("kDefault", 16120201);
		};
		//sceneeffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fenglunzhan_02_01",1000,vector3(0,0,1.2),1050);
		sceneeffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fenglunzhan_02_03",1000,vector3(0,0,1.8),333,eular(0,0,0),vector3(1.3,1.3,1.3),true);
		areadamage(533, 0, 0, 3, 2.5, false) 
		{
			stateimpact("kDefault", 16120202);
		};
		sceneeffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fenglunzhan_02_03",1000,vector3(0,0,3),533,eular(0,0,0),vector3(1.6,1.6,1.6),true);
		areadamage(833, 0, 0, 4.5, 2.5, false) 
		{
			stateimpact("kDefault", 16120203);
		};
		sceneeffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fenglunzhan_02_03",1000,vector3(0,0,4.5),833,eular(0,0,0),vector3(2,2,2),true);
		
		enablechangedir(850, 900);
		cleardamagepool(850);

		playsound(350, "skill08011", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/zhankuang/zhankuang_jiaosha_01", false);
		
		//shakecamera2(1050, 100, false, true, vector3(0.3, 0.4, 0), vector3(40, 40, 0), vector3(16, 16, 0), vector3(80, 60, 0));
	};
	section(1050)
	{
		enablechangedir(0, 200);
		animation("zhankuang_tiaokongsha_02");
		movechild(100, "1_JianShi_w_01", "ef_rightweapon01");

		//帧1
		setanimspeed(33, "zhankuang_tiaokongsha_02", 2);

		//帧9
		setanimspeed(166, "zhankuang_tiaokongsha_02", 1);

		//sound("Sound/JianShi/JianChong_02",233);
		playsound(250, "skill0802", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/zhankuang/Jiaosha_Voice_ZTF_01", false);
		
		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_jianchong_01_02",1000,"Bone010",266, false);
	
		colliderdamage(250, 950, true, true, 50, 14)
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kDefault", 16080201);
			bonecollider("hero/5_zhankuang/jianchongcollider","Bone010", true);
		};

		playsound(300, "hit08021", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04","Sound/Npc/guaiwu_shouji_05", "Sound/Npc/guaiwu_shouji_06", "Sound/Npc/guaiwu_shouji_07", "Sound/Npc/guaiwu_shouji_08", "Sound/Npc/guaiwu_shouji_09", "Sound/Npc/guaiwu_shouji_10", "Sound/Npc/guaiwu_shouji_11", "Sound/Npc/guaiwu_shouji_12", "Sound/Npc/guaiwu_shouji_13");
		};
		playsound(550, "hit08022", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04","Sound/Npc/guaiwu_shouji_05", "Sound/Npc/guaiwu_shouji_06", "Sound/Npc/guaiwu_shouji_07", "Sound/Npc/guaiwu_shouji_08", "Sound/Npc/guaiwu_shouji_09", "Sound/Npc/guaiwu_shouji_10", "Sound/Npc/guaiwu_shouji_11", "Sound/Npc/guaiwu_shouji_12", "Sound/Npc/guaiwu_shouji_13");
		};
		playsound(800, "hit08023", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04","Sound/Npc/guaiwu_shouji_05", "Sound/Npc/guaiwu_shouji_06", "Sound/Npc/guaiwu_shouji_07", "Sound/Npc/guaiwu_shouji_08", "Sound/Npc/guaiwu_shouji_09", "Sound/Npc/guaiwu_shouji_10", "Sound/Npc/guaiwu_shouji_11", "Sound/Npc/guaiwu_shouji_12", "Sound/Npc/guaiwu_shouji_13");
		};
	};

	
	oninterrupt()
	{
		stopeffect(0);
	};
	
	onstop()
	{
		stopeffect(800);
	};
};

skill(160802)
{
	section(1050)
	{
		//全局参数
		addbreaksection(1, 900,1400);
		addbreaksection(10, 900, 1400);
		addbreaksection(20, 0, 1400);
		addbreaksection(30, 700, 1400);

		movecontrol(true);
		animation("zhankuang_tiaokongsha_02");
		movechild(100, "1_JianShi_w_01", "ef_rightweapon01");

		//帧1
		setanimspeed(33, "zhankuang_tiaokongsha_02", 2);

		//帧9
		setanimspeed(166, "zhankuang_tiaokongsha_02", 1);

		//sound("Sound/JianShi/JianChong_02",233);
		playsound(250, "skill0802", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/zhankuang/zhankuang_jiaosha_Voice_01", false);
		
		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_jianchong_01_02",1000,"Bone010",266, false);
	
		colliderdamage(250, 950, true, true, 50, 14)
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kDefault", 16080201);
			bonecollider("hero/5_zhankuang/jianchongcollider","Bone010", true);
		};

	};
};