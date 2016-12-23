skill(110401)
{
	section(2176)
	{
		//全局参数
		addbreaksection(10, 1600, 2176);
		addbreaksection(30, 1600, 2176);
		movecontrol(true);
		animation("jianshi_shenguizhan_01");
	
		movechild(0, "1_JianShi_w_03", "ef_backweapon02");
		movechild(0, "1_JianShi_w_02", "ef_rightweapon01");
	
		setanimspeed(33, "jianshi_shenguizhan_01", 1.5);
	
		setanimspeed(576, "jianshi_shenguizhan_01", 1);
		colliderdamage(576, 1000, true, true, 200, 5)
		{
			stateimpact("kLauncher", 1104012);
			stateimpact("kDefault", 1104011);
			bonecollider("hero/1_jianshi/shenguizhancollider","Bone010", true);
		};
		sceneeffect("Hero_FX/1_jianshi/1_hero_jianshi_shenguizhan_01",2000,vector3(0,0,1),576);
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