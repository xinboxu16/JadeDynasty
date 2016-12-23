skill(161501)
{
	section(6000)
	{
		//全局参数
		addbreaksection(1, 5600, 6000);
		addbreaksection(10, 5600, 6000);
		addbreaksection(20, 5600, 6000);
		addbreaksection(30, 5600, 6000);
		movecontrol(true);

		addimpacttoself(0, 16150199);
		addimpacttoself(0, 16150198);
		addimpacttoself(0, 16150197);
		
		setuivisible(200, "SkillBar", false);
		setuivisible(5100, "SkillBar", true);

		storepos(0);

		settransform(1, "", vector3(0, 0, 0), eular(0, 0, 0), "RelativeWorld", false);

		restorepos(2);

		timescale(0, 0.5, 300);

		movecamera(0, true, 0.1, 0, 100, 100, 0, -2000, -2000, 0.1, 0, -100, -100, 0, 400, 400);

		movecamera(200, false, 0.1, 0, 10, 80, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0);
		
		//rotatecamera(0, 200, vector3(-215, 0, 0));

		//rotatecamera(200, 5000, vector3(0, 0, 0));

		blackscene(0, "Hero/5_zhankuang/blackscene", 1, 100, 5400, 500, "Character");

		areadamage(0, 0, 0, 0, 50, true) 
		{
			stateimpact("kDefault", 16150101);
		};

		sceneeffect("Hero_FX/1_JianShi/1_Hero_JianShi_ChaoDa_01",1500,vector3(0,0,0),0);

		oncross(300, 4900, 100)
		{
			stateimpact("kDefault", 16150102);
			message("loop", false, "ExLeft", 100, "ExRight", 100, "ExLeft2", 100, "ExRight2", 100);
		};

		sceneeffect("Hero_FX/1_JianShi/1_Hero_JianShi_ChaoDa_01",1000,vector3(0,2,0),5200);

		areadamage(5200, 0, 4, 0, 20, true) 
		{
			stateimpact("kDefault", 16150103);
		};

		sceneeffect("Hero_FX/1_JianShi/1_Hero_JianShi_ChaoDa_03",10000,vector3(0,14,0),5500);
	};

	onmessage("ExLeft")
	{
		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_ex_01_01", 1500, "Bone010", 0, false);
		animation("zhankuang_fengkuanglianzhan_02");	
		//帧1
		setanimspeed(33, "zhankuang_fengkuanglianzhan_02", 3);

		//帧10
		setanimspeed(133, "zhankuang_fengkuanglianzhan_02", 0.1);
		//帧11
		playsound(0, "ex0001", "Sound/zhankuang/zhankuang_sound", 500, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04","Sound/Npc/guaiwu_shouji_05", "Sound/Npc/guaiwu_shouji_06", "Sound/Npc/guaiwu_shouji_07", "Sound/Npc/guaiwu_shouji_08", "Sound/Npc/guaiwu_shouji_09", "Sound/Npc/guaiwu_shouji_10", "Sound/Npc/guaiwu_shouji_11", "Sound/Npc/guaiwu_shouji_12", "Sound/Npc/guaiwu_shouji_13");
		};
	};

	onmessage("ExRight")
	{
		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_ex_01_02", 1500, "Bone010", 0, false);
		animation("zhankuang_fengkuanglianzhan_03");
		//帧1
		setanimspeed(33, "zhankuang_fengkuanglianzhan_03", 3.5);
		
		//帧8
		setanimspeed(100, "zhankuang_fengkuanglianzhan_03", 0.1);
		//帧9
		playsound(0, "ex0002", "Sound/zhankuang/zhankuang_sound", 500, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04","Sound/Npc/guaiwu_shouji_05", "Sound/Npc/guaiwu_shouji_06", "Sound/Npc/guaiwu_shouji_07", "Sound/Npc/guaiwu_shouji_08", "Sound/Npc/guaiwu_shouji_09", "Sound/Npc/guaiwu_shouji_10", "Sound/Npc/guaiwu_shouji_11", "Sound/Npc/guaiwu_shouji_12", "Sound/Npc/guaiwu_shouji_13");
		};
	};

	onmessage("ExLeft2")
	{
		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_ex_01_05", 1500, "Bone010", 0, false);
		animation("zhankuang_fengkuanglianzhan_05");
		//帧1
		setanimspeed(33, "zhankuang_fengkuanglianzhan_05", 3);
		
		//帧4
		setanimspeed(66, "zhankuang_fengkuanglianzhan_05", 1);
		
		//帧5
		setanimspeed(100, "zhankuang_fengkuanglianzhan_05", 4);

		//帧9
		setanimspeed(133, "zhankuang_fengkuanglianzhan_05", 0.1);
		//帧10
		playsound(0, "ex0003", "Sound/zhankuang/zhankuang_sound", 500, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04","Sound/Npc/guaiwu_shouji_05", "Sound/Npc/guaiwu_shouji_06", "Sound/Npc/guaiwu_shouji_07", "Sound/Npc/guaiwu_shouji_08", "Sound/Npc/guaiwu_shouji_09", "Sound/Npc/guaiwu_shouji_10", "Sound/Npc/guaiwu_shouji_11", "Sound/Npc/guaiwu_shouji_12", "Sound/Npc/guaiwu_shouji_13");
		};
	};

	onmessage("ExRight2")
	{
		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_ex_01_04", 1500, "Bone010", 0, false);
		animation("zhankuang_fengkuanglianzhan_06");
		//帧1
		setanimspeed(33, "zhankuang_fengkuanglianzhan_06", 2);
		
		//帧3
		setanimspeed(66, "zhankuang_fengkuanglianzhan_06", 3);

		//帧9
		setanimspeed(133, "zhankuang_fengkuanglianzhan_06", 0.1);
		//帧11
		playsound(0, "ex0004", "Sound/zhankuang/zhankuang_sound", 500, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04","Sound/Npc/guaiwu_shouji_05", "Sound/Npc/guaiwu_shouji_06", "Sound/Npc/guaiwu_shouji_07", "Sound/Npc/guaiwu_shouji_08", "Sound/Npc/guaiwu_shouji_09", "Sound/Npc/guaiwu_shouji_10", "Sound/Npc/guaiwu_shouji_11", "Sound/Npc/guaiwu_shouji_12", "Sound/Npc/guaiwu_shouji_13");
		};
	};

	oninterrupt()
	{
	};

	onstop()
	{
	};
};
