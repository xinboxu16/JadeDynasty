

/****    瞬狱影杀阵一段    ****/

skill(120741)
{
  section(1)//初始化
  {
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器
    movecontrol(true);

    destroysummonnpc(0);

    setuivisible(0, "SkillBar", true);

    //自身增加霸体buff
    addimpacttoself(0, 12990003, 10000);
    addimpacttoself(0, 12990005, 10000);
  };

  section(3500)//第一段
  {

    ////////////     第一步 释放阶段     //////////////


    playsound(10, "Hit", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_EX_01", false);

    //起手动作
    animation("Cike_Creart");
    //
    //添加冻结效果
    //timescale(0, 0.1, 10000);

    areadamage(50, 0, 0, 0, 30, true)
		{
			stateimpact("kDefault", 12070101);
      //showtip(200, 0, 1, 0);
		};

    //释放冻结特效
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_EX_baoqihuan", 3000, vector3(0, 0, 0), 50, eular(0, 0, 0), vector3(1, 1, 1), true);


    //镜头移动 一
    movecamera(0, true, 0.5, 0, 15, 0, 0, 0, 0);
    setenable(0,  "CameraFollow", false);
    blackscene(0, "Hero/3_Cike/BlackScene", 0.5, 1000, 4500, 0, "Character");
    //
    //进入输入时间
    oninput(200, 3000, 200, 7, 7, "ontouch", "onrequire_touch");



    ////////////     第二步 连线阶段     //////////////
    //
    //召唤影子进行穿梭
    //summonnpc(3200, 101, "Hero/3_Cike/3_Cike_02", 125402, vector3(0, 0, 0));



    //setenable(6000,  "CameraFollow", true);



    //movecamera(5000, true, 0.5, 0, -15, 0, 0, 0, 0);
    //setenable(5000,  "CameraFollow", false);
    //movecamera(15000, true, 0.5, 0, 15, 0, 0, 0, 0);
  };

  section(7000)
  {
    blackscene(0, "Hero/3_Cike/BlackScene", 1, 500, 8000, 0, "Character");
    //
    //穿梭动作
   animation("Cike_Skill07_02_02", 0);
    //
    //保存当前玩家位置
    storepos(0);
    //
    //模型消失
    setenable(800, "Visible", false);
    //模型显示
    setenable(1300, "Visible", true);
    //
    //特效
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_EX_ShunShen_01", 3000, vector3(0, 0, 0), 750, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //碰撞盒
    colliderdamage(100, 3100, true, true, 0, 1)
    {
      stateimpact("kDefault", 12070201);
      sceneboxcollider(vector3(2, 2, 2), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    //进行穿梭攻击
    // 攻击1 //
    crosssummonmove(1400, false, -1, 0, 50);
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_EX_Xian_02", 3000, "Bone_Root", 1450);
    //声音
    playsound(1400, "Hit01", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_EX_08", false);
    //模型消失
    setenable(1500, "Visible", false);
    //模型显示
    setenable(1600, "Visible", true);

    // 攻击2 //
    crosssummonmove(1700, false, 0, 1, 50);
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_EX_Xian_02", 3000, "Bone_Root", 1750);
    //声音
    playsound(1700, "Hit02", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_EX_09", false);
    //模型消失
    setenable(1800, "Visible", false);
    //模型显示
    setenable(1900, "Visible", true);

    // 攻击3 //
    crosssummonmove(2100, false, 1, 2, 50);
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_EX_Xian_02", 3000, "Bone_Root", 2150);
    //声音
    playsound(2100, "Hit03", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_EX_10", false);
    //模型消失
    setenable(2200, "Visible", false);
    //模型显示
    setenable(2300, "Visible", true);

    // 攻击4 //
    crosssummonmove(2400, false, 2, 3, 50);
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_EX_Xian_02", 3000, "Bone_Root", 2450);
    //声音
    playsound(2400, "Hit04", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_EX_08", false);
    //模型消失
    setenable(2500, "Visible", false);
    //模型显示
    setenable(2600, "Visible", true);

    // 攻击5 //
    crosssummonmove(2600, false, 3, 4, 50);
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_EX_Xian_02", 3000, "Bone_Root", 2650);
    //声音
    playsound(2600, "Hit05", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_EX_09", false);
    //模型消失
    setenable(2700, "Visible", false);
    //模型显示
    setenable(2800, "Visible", true);

    // 攻击6 //
    crosssummonmove(2800, false, 4, 5, 50);
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_EX_Xian_02", 3000, "Bone_Root", 2850);
    //声音
    playsound(2800, "Hit06", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_EX_10", false);
    //模型消失
    setenable(2900, "Visible", false);
    //模型显示
    setenable(3000, "Visible", true);

    // 攻击7 //
    crosssummonmove(3000, false, 5, 6, 50);
    charactereffect("Hero_FX/3_Cike/3_Hero_CiKe_EX_Xian_02", 3000, "Bone_Root", 3050);
    //声音
    playsound(3000, "Hit07", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_EX_08", false);


    //传送特效
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_EX_ShunShen_01", 3000, vector3(0, 0, 0), 3130, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_EX_ShunShen_01", 3000, vector3(0, 0, 0), 4010, eular(0, 0, 0), vector3(1, 1, 1), true);
    //
    //传送到当前位置
    restorepos(3150);

   //模型消失
    setenable(3150, "Visible", false);
    //模型显示
    setenable(3800, "Visible", true);

    animation("Cike_Skill07_03_01", 3800);

    //打击点特效
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_EX_FaZhen_02", 3000, vector3(0, 0, 0), 4750, eular(0, 0, 0), vector3(1, 1, 1), true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_EX_baoqihuan_02", 3000, vector3(0, 0, 0), 6280, eular(0, 0, 0), vector3(1, 1, 1), true);
    //声音
    playsound(4750, "Hit08", "Sound/Cike/CikeSkillSound01", 1000, "Sound/Cike/Cike_EX_12", false);

    ///////////////////////////
    //
    //最终受击
    areadamage(4800, 0, 0, 0, 20, true)
		{
			stateimpact("kLauncher", 12070301);
      //showtip(200, 0, 1, 0);
		};
    areadamage(4900, 0, 0, 0, 20, true)
		{
			stateimpact("kLauncher", 12070301);
      //showtip(200, 0, 1, 0);
		};
    areadamage(5000, 0, 0, 0, 20, true)
		{
			stateimpact("kLauncher", 12070301);
      //showtip(200, 0, 1, 0);
		};
    areadamage(5100, 0, 0, 0, 20, true)
		{
			stateimpact("kLauncher", 12070301);
      //showtip(200, 0, 1, 0);
		};
    areadamage(5200, 0, 0, 0, 20, true)
		{
			stateimpact("kLauncher", 12070301);
      //showtip(200, 0, 1, 0);
		};
    areadamage(5300, 0, 0, 0, 20, true)
		{
			stateimpact("kLauncher", 12070301);
      //showtip(200, 0, 1, 0);
		};
    areadamage(5400, 0, 0, 0, 20, true)
		{
			stateimpact("kLauncher", 12070301);
      //showtip(200, 0, 1, 0);
		};
    areadamage(5600, 0, 0, 0, 20, true)
		{
			stateimpact("kLauncher", 12070301);
      //showtip(200, 0, 1, 0);
		};
    areadamage(5700, 0, 0, 0, 20, true)
		{
			stateimpact("kLauncher", 12070301);
      //showtip(200, 0, 1, 0);
		};
    areadamage(6300, 0, 0, 0, 20, true)
    {
			stateimpact("kDefault", 12070302);
      //showtip(200, 0, 1, 0);
    };

    //销毁召唤物
    destroysummonnpc(6400);
    //
    animation("Cike_Skill07_03_02", 5700);
    movecamera(6300, true, 0.5, 0, -15, 0, 0, 0, 0);
  };

  onmessage("ontouch")
  {
    summonnpc(0, 102, "Hero/3_Cike/3_Cike_02", 125401, vector3(0, 0, 0));
  };

  onmessage("onrequire_touch")
  {
    summonnpc(0, 102, "Hero/3_Cike/3_Cike_02", 125403, vector3(0, 0, 0));
  };

  oninterrupt()
	{
    setenable(0,  "CameraFollow", true);
    setuivisible(0, "SkillBar", true);
    setenable(2300, "Visible", true);
	};

	onstop()
	{
    setenable(0,  "CameraFollow", true);
    setuivisible(0, "SkillBar", true);
    setenable(2300, "Visible", true);
	};
};
