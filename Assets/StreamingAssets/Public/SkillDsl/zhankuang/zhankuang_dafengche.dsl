skill(161101)
{
	section(133)
	{
		//全局参数
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		movecontrol(true);
		animation("zhankuang_dafengche_01");
		
		//startcurvemove(350, true, 0.1, 0, 0, 0, 0, 0, 900, 0.2, 0, 0, 15, 0, 0, -75);

		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_dafengche_01",3000,"Bone010",100);

	};
	section(1490)
	{
		//全局参数
		
		enablechangedir(0, 1490);
		animation("zhankuang_dafengche_02")
		{
			wrapmode(2);
		};

		setanimspeed(200, "zhankuang_dafengche_02", 2);

		setanimspeed(800, "zhankuang_dafengche_02", 1.66);

		setanimspeed(1040, "zhankuang_dafengche_02", 1.33);

		startcurvemove(0, false, 1.5, 0, 0, 15, 0, 0, 0);

		
		charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_dafengche_02",1000,"Bone010",1400);
	};
	section(500)
	{
		//帧1
		//setanimspeed(33, "zhankuang_xuanfengzhan_01", 3);

		//帧7
		//setanimspeed(100, "zhankuang_xuanfengzhan_01", 1);


		animation("zhankuang_dafengche_03");
	};

	oninterrupt()
	{
		stopeffect(0);
	};

	onstop()
	{
		stopeffect(500);
	};
};
