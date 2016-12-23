skill(161201)
{
	section(900)
	{
			//全局参数
		addbreaksection(1, 433, 666);
		addbreaksection(10, 433, 666);
		addbreaksection(20, 400, 666);
		addbreaksection(30, 433, 666);
		movecontrol(true);
		animation("zhankuang_chongzhuang_01");
		movechild(0, "1_JianShi_w_01", "ef_backweapon01");

		//帧1
		setanimspeed(33, "zhankuang_tiaozhan_01", 2);
		
		//帧17
		setanimspeed(300, "zhankuang_tiaozhan_01", 1);
		//帧27

		addimpacttoself(0, 16120101, -1);		
		
		startcurvemove(150, true, 0.1, 0, 0, 0, 0, 0, 900, 0.2, 0, 0, 15, 0, 0, -75);

		//setenable(0, "Visible", false);

		//setenable(100, "Visible", true);

		areadamage(200, 0, 1, 1, 2.2, true) 
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kLauncher", 16120103);
			stateimpact("kDefault", 16120102);
		};	
		playsound(160, "skill1201", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/zhankuang/zhankuang_chongzhuang_NO1_01", false);
		
		playsound(240, "hit1201", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		};
		charactereffect("Hero_FX/5_zhankuang/5_Hero_zhankuang_zhuang", 1000, "Bone010", 200);

		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_shanshen_01_01", 1000, "Bone010", 110);

	};

	oninterrupt()
	{
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
	};

	onstop()
	{
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
	};
};

skill(161202)
{
	section(1050)
	{
		//全局参数
		addbreaksection(1, 700, 1100);
		addbreaksection(10, 700, 1100);
		addbreaksection(20, 0, 1100);
		addbreaksection(30, 700, 1100);
		
		addimpacttoself(0, 16080199, -1);	
		movecontrol(true);
		animation("zhankuang_jianchong_01");
		movechild(100, "1_JianShi_w_01", "ef_rightweapon01");
	
		setanimspeed(100, "zhankuang_jianchong_01", 1.5);

		setanimspeed(250, "jianshi_jianichong_01", 1);

		//sound("Sound/JianShi/JianChong_02",233);

		startcurvemove(350, true, 0.1, 0, 0, 0, 0, 0, 900, 0.2, 0, 0, 15, 0, 0, -75);
		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_jianchong_01_01",1000,"Bone010",350);
	
		areadamage(389, 0, 2, -1.25, 2.5, false) 
		{
			stateimpact("kDefault", 16080101);
		};

		areadamage(422, 0, 1, 0.5, 1.25, false) 
		{
			stateimpact("kDefault", 16080102);
		};

		areadamage(455, 0, 1, 0.25, 1, false) 
		{	
			stateimpact("kDefault", 16080103);
		};

		areadamage(522, 0, 1, 1.5, 1, false) 
		{
			stateimpact("kDefault", 16080104);
		};

		playsound(300, "skill1202", "Sound/zhankuang/zhankuang_sound", 2000, "Sound/zhankuang/zhankuang_chongzhuang_NO2_01", false);
		
		playsound(560, "hit1202", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/Cike/guaiwu_shouji_01", true)
		{
			audiogroup("Sound/Cike/guaiwu_shouji_02", "Sound/Cike/guaiwu_shouji_03", "Sound/Cike/guaiwu_shouji_04");
		};
	};

};