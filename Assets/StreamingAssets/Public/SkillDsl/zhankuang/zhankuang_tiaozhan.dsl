skill(161001)
{
	section(2030)
	{
		//全局参数
		addbreaksection(1, 1900, 2030);
		addbreaksection(10, 1900, 2030);
		addbreaksection(20, 1050, 2030);
		addbreaksection(30, 1900, 2030);

		movecontrol(true);
		animation("zhankuang_tiaozhan_01");
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
	
		addimpacttoself(0, 16100199, -1);	

		//帧10
		setanimspeed(333, "zhankuang_tiaozhan_01", 0.5);

		//帧19
		setanimspeed(933, "zhankuang_tiaozhan_01", 2);

		//帧25
		setanimspeed(1033, "zhankuang_tiaozhan_01", 1);
		//帧39
		enablechangedir(0, 900);
		//findmovetarget(0, vector3(0, 0, 4), 5, 90, 0.8, 0.2, 0, -0.8);
		startcurvemove(0, false, 0.1, 0, 30, 30, 0, -100, -150, 0.1, 0, 20, 15, 0, -100, -120, 0.73, 0, 10, 3, 0, -20, 0);
		findmovetarget(920, vector3(0, 0, 3), 3, 90, 0.8, 0.2, 0, -0.8);
		startcurvemove(930, true, 0.1, 0, -50, 20, 0, -500, 0);
		sceneeffect("Hero_FX/5_zhankuang/5_hero_zhankuang_tiaozhan_01_01",1000,vector3(0,0,0),0,eular(0,0,0),vector3(1,1,1),true);

		sceneeffect("Hero_FX/5_zhankuang/5_hero_zhankuang_tiaozhan_01_02",2500,vector3(0,0,1),1030,eular(0,0,0),vector3(1,1,1),true);
		
		playsound(0, "skill10011", "Sound/zhankuang/zhankuang_sound", 3000, "Sound/zhankuang/zhankuang_tiaozhan_01", false);
		
		playsound(1000, "skill10012", "Sound/zhankuang/zhankuang_sound", 3000, "Sound/zhankuang/Tiaozhan_Voice_ZTF_01", false);
		
		playsound(1200, "skill10013", "Sound/zhankuang/zhankuang_sound", 3000, "Sound/zhankuang/ShenGuiZhan_01", false);

		colliderdamage(1030, 1000, true, true, 100, 10)
		{
			stateimpact("kLauncher", 16100101);
			stateimpact("kDefault", 16100102);
			bonecollider("hero/1_jianshi/shenguizhancollider","Bone010", true);
		};

		shakecamera2(1030, 200, false, true, vector3(0.5, 0.8, 0), vector3(40, 40, 0), vector3(24, 24, 0), vector3(80, 60, 0));
		
	};
};