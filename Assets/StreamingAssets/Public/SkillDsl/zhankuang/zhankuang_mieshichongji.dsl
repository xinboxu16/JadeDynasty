skill(161401)
{
	section(1050)
	{
		//全局参数
		addbreaksection(1, 700, 1100);
		addbreaksection(10, 700, 1100);
		addbreaksection(20, 0, 1100);
		addbreaksection(30, 700, 1100);

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
	};
};
