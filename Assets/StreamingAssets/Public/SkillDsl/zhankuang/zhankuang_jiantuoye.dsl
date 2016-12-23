skill(160601)
{
	section(333)
	{
		//全局参数
		addbreaksection(1, 2033, 2033);
		addbreaksection(10, 2033, 2033);
		addbreaksection(20, 0, 2033);
		addbreaksection(30, 2033, 2033);
		movecontrol(true);
		animation("zhankuang_jiantuoye_01");
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		
		startcurvemove(200, true, 0.066, 0, 0, 40, 0, 0, -200, 0.066, 0, 0, 10, 0, 0, 0);
		
		//帧
		//setanimspeed(933, "zhankuang_xuanfengzhan_01", 0.1);
		//帧10

		areadamage(250, 0, 1, 0, 3, true) 
		{
			stateimpact("kDefault", 16060101);
		};
	};

	section(1700)
	{
		enablechangedir(0, 1700);

		animation("zhankuang_jiantuoye_02")
		{
			wrapmode(2);	
		};
		
		startcurvemove(0, false, 1.7, 0, 0, 15, 0, 0, 0);

		colliderdamage(0, 1700, true, true, 100, 14)
		{
			stateimpact("kDefault", 16060102);
			bonecollider("hero/5_zhankuang/jiantuoyecollider","Bone010", true); 
		};

		playsound(100, "skill0001", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/KongZhongLianJi_02", false);
		playsound(900, "skill0002", "Sound/zhankuang/zhankuang_sound", 1000, "Sound/zhankuang/KongZhongLianJi_2", false);

		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");

	};

	oninterrupt()
	{
	};

	onstop()
	{
	};
};