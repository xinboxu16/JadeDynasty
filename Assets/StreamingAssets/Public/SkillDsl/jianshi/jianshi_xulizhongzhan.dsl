skill(110501)
{
	section(2050)
	{
		//全局参数
		addbreaksection(10, 1200, 2050);
		addbreaksection(30, 1200, 2050);
		movecontrol(true);
		animation("jianshi_xulizhongzhan_01");

		sceneeffect("Hero_FX/1_jianshi/1_hero_jianshi_xulizhongzhan_02",1500,vector3(0,1,-0.8),200);
	
		setanimspeed(433, "jianshi_xulizhongzhan_01", 0.5);
		movechild(433, "1_JianShi_w_02", "ef_rightweapon01");

		setanimspeed(700, "jianshi_xulizhongzhan_01", 1);

		sceneeffect("Hero_FX/1_jianshi/1_hero_jianshi_xulizhongzhan_01",1000,vector3(0,1,0),900);
	
		areadamage(900, 0, 1, 1.5, 4.5, true) 
		{	
			stateimpact("kDefault", 1105011);
		};
    
	};

	oninterrupt()
	{
		movechild(0, "1_JianShi_w_01", "ef_backweapon01");
		movechild(0, "1_JianShi_w_02", "ef_leftweapon02");
		movechild(0, "1_JianShi_w_03", "ef_leftweapon01");
	};

	onstop()
	{
		movechild(0, "1_JianShi_w_01", "ef_backweapon01");
		movechild(0, "1_JianShi_w_02", "ef_leftweapon02");
		movechild(0, "1_JianShi_w_03", "ef_leftweapon01");
	};
};