skill(160501)
{
	section(433)
	{
		//全局参数
		movecontrol(true);
		animation("zhankuang_jujianchongji_01");
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");

		//startcurvemove(233, true, 0.1, 0, 0, 15, 0, 0, -100);
		
		//帧1
		setanimspeed(33, "zhankuang_jujianchongji_01", 3);

		//帧7
		setanimspeed(100, "zhankuang_jujianchongji_01", 1);

		//帧8
		setanimspeed(133, "zhankuang_jujianchongji_01", 0.5);

		//帧9
		setanimspeed(200, "zhankuang_jujianchongji_01", 1);

		//帧10
		setanimspeed(233, "zhankuang_jujianchongji_01", 2.5);

		//帧15
		setanimspeed(300, "zhankuang_jujianchongji_01", 1);
		//帧19

		setchildvisible(280,"1_JianShi_w_01",false);
		summonnpc(280, 101, "Hero/5_zhankuang/1_zhankuang_w_01", 151002, vector3(0, 0, 0));
		
	};

	oninterrupt()
	{
		setchildvisible(0,"1_JianShi_w_01",true);
	};

	onstop()
	{
		setchildvisible(0,"1_JianShi_w_01",true);
	};
};

skill(160502)
{
	section(433)
	{
		//全局参数
		movecontrol(true);
		animation("zhankuang_jujianchongji_01");
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");

		//startcurvemove(233, true, 0.1, 0, 0, 15, 0, 0, -100);
		
		//帧1
		setanimspeed(33, "zhankuang_jujianchongji_01", 3);

		//帧7
		setanimspeed(100, "zhankuang_jujianchongji_01", 1);

		//帧8
		setanimspeed(133, "zhankuang_jujianchongji_01", 0.5);

		//帧9
		setanimspeed(200, "zhankuang_jujianchongji_01", 1);

		//帧10
		setanimspeed(233, "zhankuang_jujianchongji_01", 2.5);

		//帧15
		setanimspeed(300, "zhankuang_jujianchongji_01", 1);
		//帧19

		setchildvisible(280,"1_JianShi_w_01",false);
		summonnpc(280, 101, "Hero/5_zhankuang/1_zhankuang_w_01", 151002, vector3(0, 0, 0));
		
	};

	oninterrupt()
	{
		setchildvisible(0,"1_JianShi_w_01",true);
	};

	onstop()
	{
		setchildvisible(0,"1_JianShi_w_01",true);
	};
};