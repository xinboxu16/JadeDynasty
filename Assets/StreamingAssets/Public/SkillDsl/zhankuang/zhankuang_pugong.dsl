skill(160001)
{
	section(450)
	{
		addbreaksection(1, 350, 500);
		addbreaksection(10, 300, 500);
		addbreaksection(20, 0, 450);
		addbreaksection(30, 0, 120);
		addbreaksection(30, 210, 500);
		movecontrol(true);
		animation("zhankuang_pugong_01");
		
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		//findmovetarget(100, vector3(0, 0, 0), 3, 180, 0.8, 0.2, 0, -0.8);
		startcurvemove(166, true, 0.05, 0, 0, 4, 0, 0, 80, 0.05, 0, 0, 8, 0, 0, -80);
		

		//֡1
		setanimspeed(33, "zhankuang_pugong_01", 2);

		//֡3
		setanimspeed(66, "zhankuang_pugong_01", 1);

		//֡5
		//setanimspeed(133, "zhankuang_pugong_01", 1);

		//֡6
		//setanimspeed(166, "zhankuang_pugong_01", 1.5);

		//֡10.5
		//setanimspeed(266, "zhankuang_julitiaokong_01", 1);
		//֡11
		
		playsound(285, "skill0001", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_01_new", false);
		
		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_pugong_01_01", 500, "Bone010", 285, false);
    
		areadamage(285, 0, 1.5, 1, 2, true) 
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kLauncher", 16000102);
			stateimpact("kDefault", 16000101);
		};

		playsound(300, "hit0001", "Sound/zhankuang/zhankuang_sound", 1500, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04","Sound/Npc/guaiwu_shouji_05", "Sound/Npc/guaiwu_shouji_06", "Sound/Npc/guaiwu_shouji_07", "Sound/Npc/guaiwu_shouji_08", "Sound/Npc/guaiwu_shouji_09", "Sound/Npc/guaiwu_shouji_10", "Sound/Npc/guaiwu_shouji_11", "Sound/Npc/guaiwu_shouji_12", "Sound/Npc/guaiwu_shouji_13");
		};
   
		//lockframe(111, "zhankuang_pugong_01", true, 0, 100, 1, 150);
		//shakecamera2(200, 100, false, true, vector3(0.2, 0.3, 0), vector3(50, 50, 0), vector3(8, 8, 0), vector3(80, 60, 0));
	};
	
	oninterrupt()
	{
		stopeffect(300);
	};
	
	onstop()
	{
		stopeffect(300);
	};
};

skill(160002)
{
	section(450)
	{
		addbreaksection(1, 420, 600);
		addbreaksection(10, 370, 600);
		addbreaksection(20, 0, 600);
		addbreaksection(30, 0, 50);
		addbreaksection(30, 130, 600);
		movecontrol(true);
		animation("zhankuang_pugong_02");
		
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		//findmovetarget(100, vector3(0, 0, 0), 1.5, 180, 0.8, 0.2, 0, -0.8);
		startcurvemove(166, true, 0.05, 0, 0, 8, 0, 0, 80, 0.05, 0, 0, 12, 0, 0, -160);
		
		playsound(300, "skill0002", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/zhankuang_pugong_02", false);

		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_pugong_02_01", 500, "Bone010", 300, false);
    
		areadamage(300, 0, 1.5, 1, 2, true) 
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kLauncher", 16000202);
			stateimpact("kDefault", 16000201);
		};
		
		playsound(310, "hit0002", "Sound/zhankuang/zhankuang_sound", 1500, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04","Sound/Npc/guaiwu_shouji_05", "Sound/Npc/guaiwu_shouji_06", "Sound/Npc/guaiwu_shouji_07", "Sound/Npc/guaiwu_shouji_08", "Sound/Npc/guaiwu_shouji_09", "Sound/Npc/guaiwu_shouji_10", "Sound/Npc/guaiwu_shouji_11", "Sound/Npc/guaiwu_shouji_12", "Sound/Npc/guaiwu_shouji_13");
		};
		//lockframe(111, "zhankuang_pugong_02", true, 0, 100, 1, 150);
		//shakecamera2(100, 100, false, true, vector3(0.2, 0.3, 0), vector3(50, 50, 0), vector3(8, 8, 0), vector3(80, 60, 0));
	};
	
	oninterrupt()
	{
		stopeffect(300);
	};
	
	onstop()
	{
		stopeffect(300);
	};
};

skill(160003)
{
	section(800)
	{
		addbreaksection(1, 616, 800);
		addbreaksection(10, 566, 800);
		addbreaksection(20, 0, 800);
		addbreaksection(30, 0, 233);
		addbreaksection(30, 330, 480);
		addbreaksection(30, 530, 800);
		movecontrol(true);
		animation("zhankuang_pugong_03");
		
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		
		//findmovetarget(100, vector3(0, 0, 0), 1.5, 180, 0.8, 0.2, 0, -0.8);
		startcurvemove(100, true, 0.05, 0, 0, 8, 0, 0, 80, 0.05, 0, 0, 12, 0, 0, -160 );
		
		//findmovetarget(100, vector3(0, 0, 0), 1.5, 180, 0.8, 0.2, 0, -0.8);
		startcurvemove(433, true, 0.05, 0, 0, 8, 0, 0, 80, 0.05, 0, 0, 12, 0, 0, -160 );
		
		//֡1
		setanimspeed(33, "zhankuang_pugong_03", 2);

		//֡7
		setanimspeed(133, "zhankuang_pugong_03", 1);
		
		playsound(250, "skill00031", "Sound/zhankuang/zhankuang_sound", 1500, "Sound/zhankuang/zhankuang_pugong_03", false);

		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_pugong_03_01", 500, "Bone010", 233, false);
		
		playsound(550, "skill00032", "Sound/zhankuang/zhankuang_sound", 1500, "Sound/zhankuang/zhankuang_pugong_04", false);

		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_pugong_03_02", 500, "Bone010", 550, false);
    
		areadamage(250, 0, 1.5, 1, 2, true) 
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kLauncher", 16000302);
			stateimpact("kDefault", 16000301);
		};
		
		areadamage(550, 0, 1.5, 1, 2, true) 
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kLauncher", 16000312);
			stateimpact("kDefault", 16000311);
		};
		
		playsound(260, "hit00031", "Sound/zhankuang/zhankuang_sound", 1500, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04","Sound/Npc/guaiwu_shouji_05", "Sound/Npc/guaiwu_shouji_06", "Sound/Npc/guaiwu_shouji_07", "Sound/Npc/guaiwu_shouji_08", "Sound/Npc/guaiwu_shouji_09", "Sound/Npc/guaiwu_shouji_10", "Sound/Npc/guaiwu_shouji_11", "Sound/Npc/guaiwu_shouji_12", "Sound/Npc/guaiwu_shouji_13");
		};
		
		playsound(560, "hit00032", "Sound/zhankuang/zhankuang_sound", 1500, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04","Sound/Npc/guaiwu_shouji_05", "Sound/Npc/guaiwu_shouji_06", "Sound/Npc/guaiwu_shouji_07", "Sound/Npc/guaiwu_shouji_08", "Sound/Npc/guaiwu_shouji_09", "Sound/Npc/guaiwu_shouji_10", "Sound/Npc/guaiwu_shouji_11", "Sound/Npc/guaiwu_shouji_12", "Sound/Npc/guaiwu_shouji_13");
		};
		//lockframe(111, "zhankuang_pugong_03", true, 0, 100, 1, 150);
		//shakecamera2(100, 100, false, true, vector3(0.2, 0.3, 0), vector3(50, 50, 0), vector3(8, 8, 0), vector3(80, 60, 0));
	};

	oninterrupt()
	{
		stopeffect(300);
	};

	onstop()
	{
		stopeffect(300);
	};
};

skill(160004)
{
	section(600)
	{
		addbreaksection(1, 600, 600);
		addbreaksection(10, 600, 600);
		addbreaksection(20, 0, 600);
		addbreaksection(30, 0, 200);
		addbreaksection(30, 400, 600);
		movecontrol(true);
		animation("zhankuang_pugong_04");
		
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		//findmovetarget(100, vector3(0, 0, 0), 1.5, 180, 0.8, 0.2, 0, -0.8);
		startcurvemove(0, true, 0.1, 0, 0, 0, 0, 0, 80, 0.13, 0, 0, 8, 0, 0, -40, 0.05, 0, 0, 2, 0, 0, 320, 0.05, 0, 0, 10, 0, 0, -160);
		
		//֡1
		setanimspeed(33, "zhankuang_pugong_04", 1.5);
		
		//֡4
		setanimspeed(100, "zhankuang_pugong_04", 0.5);

		//֡5
		setanimspeed(166, "zhankuang_pugong_04", 1);
		
		//֡8
		setanimspeed(266, "zhankuang_pugong_04", 2);

		//֡12
		setanimspeed(343, "zhankuang_pugong_04", 1);
		
		playsound(300, "skill0004", "Sound/zhankuang/zhankuang_sound", 1500, "Sound/zhankuang/zhankuang_pugong_05", false);

		playsound(310, "skill000402", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/Pugong_Voice_ZTF_01", false)
		{
			audiogroup("Sound/zhankuang/Pugong_Voice_ZTF_02", "Sound/zhankuang/Pugong_Voice_ZTF_03");
		};
		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_pugong_04_01", 500, "Bone010", 300, false);
    
		areadamage(300, 0, 1.5, 1, 2.3, false) 
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kLauncher", 16000402);
			stateimpact("kDefault", 16000401);
		};

		playsound(310, "hit0004", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04","Sound/Npc/guaiwu_shouji_05", "Sound/Npc/guaiwu_shouji_06", "Sound/Npc/guaiwu_shouji_07", "Sound/Npc/guaiwu_shouji_08", "Sound/Npc/guaiwu_shouji_09", "Sound/Npc/guaiwu_shouji_10", "Sound/Npc/guaiwu_shouji_11", "Sound/Npc/guaiwu_shouji_12", "Sound/Npc/guaiwu_shouji_13");
		};
		//lockframe(111, "zhankuang_pugong_02", true, 0, 100, 1, 150);
		//shakecamera2(370, 100, false, true, vector3(0.2, 0.3, 0), vector3(50, 50, 0), vector3(8, 8, 0), vector3(80, 60, 0));
	};

	oninterrupt()
	{
		stopeffect(300);
	};
	
	onstop()
	{
		stopeffect(300);
	};
};