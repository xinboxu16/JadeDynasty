skill(150101)
{
  section(1450)
  {
	//全局参数
    addbreaksection(10, 1200, 1500);
    addbreaksection(30, 1200, 1500);
    movecontrol(true);
    animation("jianshi_xuanfengzhan_01");
	movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
	
  startcurvemove(100, true, 0.1, 0, 0, 0, 0, 0, 200, 0.1, 0, 0, 20, 0, 0, -150, 0.15, 0, 0, 5, 0, 0, 100, 0.15, 0, 0, 20, 0, 0, -100, 0.15, 0, 0, 5, 0, 0, 100, 0.15, 0, 0, 20, 0, 0, -100);

	charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_xuanfengzhan_01_01",1000,"Bone010",166);

	areadamage(266, 0, 1, 0, 3, true) 
	{
      stateimpact("kDefault", 1501011);
    };
	shakecamera(289, true, 120, 40, 100, 0.06);

	charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_xuanfengzhan_01_02",1000,"Bone010",466);
	
	areadamage(566, 0, 1, 0, 3, true) 
	{
      stateimpact("kDefault", 1501012);
    };
	shakecamera(589, true, 120, 40, 100, 0.06);

	charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_xuanfengzhan_01_03",1000,"Bone010",766);
	
	areadamage(866, 0, 1, 0, 3, true) 
	{
      stateimpact("kDefault", 1501013);
    };
	shakecamera(889, true, 120, 40, 100, 0.06);
	
  };

  section(50)
  {
	movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
  };
};

skill(150102)
{
  section(1450)
  {
	//全局参数
    addbreaksection(10, 1300, 1500);
    addbreaksection(30, 1300, 1500);
    movecontrol(true);
    animation("zhankuang_xuanfengzhan_01");
	movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
    
	charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_xuanfengzhan_02_01",1000,"Bone010",166);

	areadamage(200, 0, 1.5, 1.5, 3, true) 
	{
      stateimpact("kDefault", 1501021);
    };
	shakecamera(222, true, 120, 40, 100, 0.06);

	charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_xuanfengzhan_02_02",1000,"Bone010",566);
	
	areadamage(600, 0, 1.5, 1.5, 3, true) 
	{
      stateimpact("kDefault", 1501022);
    };
	shakecamera(622, true, 120, 40, 100, 0.06);

	charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_xuanfengzhan_02_01",1000,"Bone010",766);
	
	areadamage(800, 0, 1.5, 1.5, 3, true) 
	{
      stateimpact("kDefault", 1501023);
    };
	shakecamera(822, true, 120, 40, 100, 0.06);
	
	charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_xuanfengzhan_02_02",1000,"Bone010",1066);
	
	areadamage(1100, 0, 1.5, 1.5, 3, true) 
	{
      stateimpact("kDefault", 1501024);
    };
	shakecamera(1122, true, 120, 40, 100, 0.06);
  };

  section(50)
  {
	movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
  };
};

skill(150103)
{
  section(1450)
  {
	//全局参数
    addbreaksection(10, 1200, 1500);
    addbreaksection(30, 1200, 1500);
    movecontrol(true);
    animation("zhankuang_xuanfengzhan_01");
	movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
	
  startcurvemove(0, true, 0.1, 0, 0, 0, 0, 0, 100, 0.1, 0, 0, 10, 0, 0, -85, 0.15, 0, 0, 2, 0, 0, 100, 0.15, 0, 0, 16.5, 0, 0, -110);

	charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_xuanfengzhan_03_01",1000,"Bone010",66);

	areadamage(200, 0, 1, 0, 3.5, true) 
	{
      stateimpact("kDefault", 1501031);
    };
	shakecamera(222, true, 120, 40, 100, 0.06);

	charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_xuanfengzhan_03_02",1000,"Bone010",333);
	
	areadamage(533, 0, 1, 0, 4, true) 
	{
      stateimpact("kDefault", 1501032);
    };
	shakecamera(555, true, 120, 40, 100, 0.08);

	charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_xuanfengzhan_03_03",1000,"Bone010",563);
  };

  section(50)
  {
	movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
  };
};

skill(150201)
{
  section(1450)
  {
	//全局参数
    addbreaksection(10, 1200, 1500);
    addbreaksection(30, 1200, 1500);
    movecontrol(true);
    animation("zhankuang_fenglunzhan_03");
	movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
	
	startcurvemove(200, true, 0.1, 0, 0, 0, 0, 0, 200, 0.1, 0, 0, 10, 0, 0, -180, 0.4, 0, 0, 0, 0, 12.5, 50, 0.1, 0, 5, 20, 0, -500, -200);

	sceneeffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fenglunzhan_01_02",1000,vector3(0,0,0),267,eular(0,0,0),vector3(1,1,1),true);
	
	charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fenglunzhan_01_01",1000,"Bone010",300);

	areadamage(400, 0, 1, 0, 3, true) 
	{
      stateimpact("kDefault", 1502011);
    };
	
	charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fenglunzhan_01_01",1000,"Bone010",767);

	areadamage(900, 0, 1, 1.5, 3, true) 
	{
      stateimpact("kDefault", 1502012);
    };
	
	sceneeffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fenglunzhan_01_02",1000,vector3(0,0,0.5),1033,eular(0,0,0),vector3(1.3,1.3,1.3),true);
	sceneeffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fenglunzhan_03_02",1000,vector3(0,0,2.3),1033);
  };

  section(50)
  {
	movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
  };
};

skill(150202)
{
  section(1050)
  {
	//全局参数
    addbreaksection(10, 800, 1100);
    addbreaksection(30, 800, 1100);
    movecontrol(true);
    animation("zhankuang_fenglunzhan_03");
	movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
	
	startcurvemove(400, true, 0.05, 0, 0, 0, 0, 0, 400, 0.05, 0, 0, 20, 0, 0, -300);
	charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fenglunzhan_02_01",1000,"Bone010",400);
	
	areadamage(500, 0, 1, 1, 3, true) 
	{
      stateimpact("kDefault", 1502021);
    };

	sceneeffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fenglunzhan_02_02",1000,vector3(0,0,2.2),600);
  };

  section(50)
  {
	movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
  };
};

skill(150203)
{
  section(1250)
  {
	//全局参数
    addbreaksection(10, 900, 1300);
    addbreaksection(30, 900, 1300);
    movecontrol(true);
    animation("zhankuang_fenglunzhan_03");
	
    startcurvemove(33, true, 0.1, 0, 40, 10, 0, -400, 0, 0.5, 0, 0, 10, 0, -16, 0);

	charactereffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fenglunzhan_03_01",1000,"Bone010",166);
	
	colliderdamage(200, 500, true, true, 100, 5)
    {
       stateimpact("kDefault", 1502031);
	   bonecollider("hero/5_zhankuang/fenglunzhancollider","Bone010", true);
    };

	sceneeffect("Hero_FX/5_zhankuang/5_hero_zhankuang_fenglunzhan_03_02",1000,vector3(0,0,1.8),733);
	areadamage(733, 0, 1.5, 1, 4, true) 
	{
      stateimpact("kDefault", 1502032);
    };
  };

  section(50)
  {
	movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
  };
};
