skill(110201)
{
  section(1050)
  {
	//全局参数
    addbreaksection(10, 700, 1100);
    addbreaksection(30, 700, 1100);
    movecontrol(true);
    animation("jianshi_jianchong_01");
	
	setanimspeed(100, "jianshi_jianchong_01", 1.5);
    movechild(100, "1_JianShi_w_01", "ef_rightweapon01");
	sceneeffect("Hero_FX/1_jianshi/1_hero_jianshi_jianchong_02",1000,vector3(0,0,0),100);
	
	//sound("Sound/JianShi/JianChong_02",233);
	setanimspeed(250, "jianshi_jianchong_01", 1);

  startcurvemove(350, true, 0.1, 0, 0, 0, 0, 0, 900, 0.2, 0, 0, 15, 0, 0, -75);
	charactereffect("Hero_FX/1_jianshi/1_hero_jianshi_jianchong_01",1000,"Bone010",350);
	
	shakecamera(389, true, 120, 40, 100, 0.03);
    areadamage(389, 0, 2, -1.25, 2.5, false) 
	{
      stateimpact("kDefault", 1102011);
    };

    areadamage(422, 0, 1, 0.5, 1.25, false) 
	{
      stateimpact("kDefault", 1102012);
    };

	areadamage(455, 0, 1, 0.25, 1, false) 
	{
      stateimpact("kDefault", 1102013);
    };

	areadamage(522, 0, 1, 1.5, 1, false) 
	{
      stateimpact("kDefault", 1102014);
    };
  };

  section(50)
  {
	movechild(0, "1_JianShi_w_01", "ef_backweapon01");
    movechild(0, "1_JianShi_w_02", "ef_leftweapon02");
    movechild(0, "1_JianShi_w_03", "ef_leftweapon01");
  };
};