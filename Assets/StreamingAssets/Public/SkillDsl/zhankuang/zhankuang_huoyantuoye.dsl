skill(161601)
{
	section(2200)
	{
		//全局参数
		addbreaksection(1, 2200, 2200);
		addbreaksection(10, 2200, 2200);
		addbreaksection(20, 0, 2200);
		addbreaksection(30, 0, 2200);
		movecontrol(true);
		animation("zhankuang_huoyantuoye_01");
		movechild(0, "1_JianShi_w_01", "ef_backweapon01");
		
		addimpacttoself(0, 16160199);

		//帧6
		//setanimspeed(200, "zhankuang_huoyantuoye_01", 3);
			
		//startcurvemove(200, true, 0.1, 0, 0, 5, 0, 0, 100, 0.03, 0, 0, 6, 0, 0, -80, 0.3, 0, 3, 0, 0, 30, 50, 0.1, 0, 3, 15, 0, -60, -120, 0.2, 0, -3, 10, 0, -300, -40);

		colliderdamage(200, 1500, true, true, 100, 16)
		{
			stateimpact("kKnockDown", 16000000);
			stateimpact("kLauncher", 16160102);
			stateimpact("kDefault", 16160101);
			bonecollider("hero/5_zhankuang/huoyanbaopocollider","Bone010", true);
		};
		
		charactereffect("Hero_FX/1_JianShi/1_Hero_JianShi_HuoYanBaoPo_02", 5000, "ef_leftweapon01", 0, true);
		sceneeffect("Hero_FX/1_JianShi/1_Hero_JianShi_HuoYanBaoPo_03",1000,vector3(-0.03, 1.99, 0.54),200);
		sceneeffect("Hero_FX/1_JianShi/1_Hero_JianShi_HuoYanBaoPo_03",1000,vector3(-0.91, 1.06, 1.15),300);
		sceneeffect("Hero_FX/1_JianShi/1_Hero_JianShi_HuoYanBaoPo_03",1000,vector3(-0.36, 0.99, 0.94),400);
		sceneeffect("Hero_FX/1_JianShi/1_Hero_JianShi_HuoYanBaoPo_03",1000,vector3(0.28, 1.34, 2.01),500);
		sceneeffect("Hero_FX/1_JianShi/1_Hero_JianShi_HuoYanBaoPo_03",1000,vector3(0.36, 1.11, 2.44),600);
		sceneeffect("Hero_FX/1_JianShi/1_Hero_JianShi_HuoYanBaoPo_03",1000,vector3(-0.45, 1.01, 0.87),700);
		sceneeffect("Hero_FX/1_JianShi/1_Hero_JianShi_HuoYanBaoPo_03",1000,vector3(-0.78, 0.09, 0.65),800);
		sceneeffect("Hero_FX/1_JianShi/1_Hero_JianShi_HuoYanBaoPo_03",1000,vector3(-0.71, 0.05, 1.42),900);
		sceneeffect("Hero_FX/1_JianShi/1_Hero_JianShi_HuoYanBaoPo_03",1000,vector3(0.69, 1.09, 1.1),1000);
		sceneeffect("Hero_FX/1_JianShi/1_Hero_JianShi_HuoYanBaoPo_03",1000,vector3(-0.73, 1.84, 1.46),1100);
		sceneeffect("Hero_FX/1_JianShi/1_Hero_JianShi_HuoYanBaoPo_03",1000,vector3(-0.57, 0.69, 1.56),1200);
		sceneeffect("Hero_FX/1_JianShi/1_Hero_JianShi_HuoYanBaoPo_03",1000,vector3(-0.43, 1.43, 1.43),1300);
		sceneeffect("Hero_FX/1_JianShi/1_Hero_JianShi_HuoYanBaoPo_03",1000,vector3(0.29, 0.72, 1.04),1400);
		sceneeffect("Hero_FX/1_JianShi/1_Hero_JianShi_HuoYanBaoPo_03",1000,vector3(0.84, 1.83, 1.61),1500);
		sceneeffect("Hero_FX/1_JianShi/1_Hero_JianShi_HuoYanBaoPo_03",1000,vector3(0.61, 0.86, 1.24),1600);
		sceneeffect("Hero_FX/1_JianShi/1_Hero_JianShi_HuoYanBaoPo_03",1000,vector3(0.13, 0.9, 2.3),1700);
		
		areadamage(1900, 0, 1.5, 1, 4, true) 
		{
			stateimpact("kDefault", 16160103);
		};

		sceneeffect("Hero_FX/1_JianShi/1_Hero_JianShi_HuoYanBaoPo_04",2000,vector3(0, 2, 2.5),1900);
	};
	
	oninterrupt()
	{
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		stopeffect(0);
	};
	
	onstop()
	{
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		stopeffect(500);
	};
};

skill(161602)
{
	section(300)
	{
		movechild(0, "1_JianShi_w_01", "ef_backweapon01");
		movecontrol(true);
		animation("zhankuang_huoyantuoye_02");
		addimpacttoself(0, 16160299);
		sceneeffect("Hero_FX/1_JianShi/1_hero_jianshi_jianchong_02",1000,vector3(0, 2.5, 0),100);
		findmovetarget(230, vector3(0, 1, 0.5), 2, 180, 0.8, 0.2, 0, 0, false);
    {
      filtsupperarmer();
    };
		gotosection(200, 1, 100) 
		{
			targetcondition(true);
		};
		gotosection(300, 3, 0);
	};
	section(3000)
	{
		addimpacttotarget(0, 701, -1);
		//grabtarget(1, true, "ef_rightweapon01", "ef_body", 100);
		grabtarget(100, true, "ef_rightweapon01", "Bip001", 0);
		enablechangedir(0, 3000);
		startcurvemove(101, false, 3, 0, 0, 14, 0, 0, 0);
		colliderdamage(100, 2900, true, true, 0, 14)
		{
			stateimpact("kDefault", 16160206);
			bonecollider("hero/5_zhankuang/huoyantuoyecollider","ef_rightweapon01", true);
		};
		animation("zhankuang_huoyantuoye_03", 102)
		{
			wrapmode(2);
		};
		playsound(1300, "skill0402", "Sound/zhankuang/zhankuang_sound", 3500, "Sound/zhankuang/EX_Voice_ZTF_01", false);
		charactereffect("Hero_FX/1_JianShi/1_Hero_JianShi_HuoYanBaoPo_05", 5000, "ef_rightweapon01", 0, true);
	};
	section(1533)
	{
		movecontrol(true);
		animation("zhankuang_huoyantuoye_04");
		startcurvemove(0, false, 0.17, 0, 50, 0, 0, -200, 0, 0.33, 0, 5, 0, 0, -20, 0, 0.07, 0, -300, 0, 0, 0, 0);
		setanimspeed(33, "zhankuang_huoyantuoye_04", 2.5);
		setanimspeed(166, "zhankuang_huoyantuoye_04", 0.5);
		setanimspeed(500, "zhankuang_huoyantuoye_04", 2.5);
		setanimspeed(566, "zhankuang_huoyantuoye_04", 1);
		areadamage(570, 0, 1.5, 0, 4, true)
		{
			stateimpact("kDefault", 16160205);
		};
		addimpacttotarget(510, 16160202, -1);
		grabtarget(511, false);
		sceneeffect("Hero_FX/5_zhankuang/5_hero_zhankuang_tiaozhan_01_04",1500,vector3(0, 0, 0.7),566);
	};
	section(10)
	{
	};
	oninterrupt()
	{
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		stopeffect(0);
		grabtarget(0, false);
		addimpacttotarget(0, 16160202, -1);
	};
	onstop()
	{
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
		stopeffect(500);
		grabtarget(0, false);
	};
};