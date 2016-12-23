skill(110101)
{
	section(866)
	{
		//全局参数
		addbreaksection(10, 500, 866);
		addbreaksection(30, 500, 866);
		movecontrol(true);
		animation("jianshi_shanshen_01");
	
		charactereffect("Hero_FX/1_jianshi/1_hero_jianshi_shanshen_01", 500, "Bone010", 67);
		startcurvemove(67, true, 0.066, 0, 0, 0, 0, 0, 1080, 0.066, 0, 0, 72, 0, 0, -945, 0.2, 0, 0, 9, 0, 0, -45);

		setanimspeed(433, "jianshi_shanshen_01", 0.25);

		setanimspeed(700, "jianshi_shanshen_01", 1);
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