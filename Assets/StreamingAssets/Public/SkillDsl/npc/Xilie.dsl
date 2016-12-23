//浮空
skill(380201)
{
  section(967)
  {
    movecontrol(true);
    animation("Skill_03");
    findmovetarget(300, vector3(0,0,0), 5, 60, 0.5, 0.5, 0, -1);
    startcurvemove(320, true, 0.1, 0, 0, 20, 0, 0, 0);
    playsound(250, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_fukong", false);
    charactereffect("Monster_FX/Boss/6_Mon_Xilie_Fukong_01", 800, "Bone_Root", 310, true);
    //sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Fukong_01", 3000, vector3(0,0,0), 310, eular(0,0,0), vector3(1,1,1), true);
    colliderdamage(320, 100, false, false, 0, 0)
    {
      stateimpact("kDefault", 38020101);
      stateimpact("kLauncher", 38020102);
      sceneboxcollider(vector3(3, 2, 3), vector3(0, 1, 0.5), eular(0, 0, 0), true, false);
    };
    shakecamera2(350, 200, false, false, vector3(0,0.6,0), vector3(0,100,0),vector3(0,3,0),vector3(0,80,0));
    playsound(350, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
};

//爆气
skill(380202)
{
  section(1933)
  {
    animation("Skill_01");
    settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeSelf",false,true);
    addimpacttoself(0, 30010003);
    playsound(370, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_baoqi", false);
    areadamage(450, 0, 1, 0, 3, false) {
      stateimpact("kDefault", 38020201);
    };
    sceneeffect("Monster_FX/Boss/5_Mon_Berserker_BaoQi_01", 3000, vector3(0,1.2,0), 300, eular(0,0,0), vector3(1.5,1.5,1.5));
    shakecamera2(450,550,false,false,vector3(0.5,0.5,0.5),vector3(50,50,50),vector3(5,5,5),vector3(85,85,85));
  };
};

//开炮
skill(380203)
{
  section(1333)
  {
    movecontrol(true);
    animation("Attack_01");
    summonnpc(10, 101, "Monster_FX/Campaign_Wild/6_Monster_YuJing_01", 380204, vector3(0, 0, 0));
    playsound(780, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_kaipao", false);
    charactereffect("Monster_FX/Boss/6_Mon_Xilie_PaoKou_01", 800, "ef_weapon01", 798, false);
    startcurvemove(798, true, 0.1, 0, 0, -4, 0, 0, 0);
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    destroysummonnpc(0);
  };
};


//单发炮弹
skill(380204)
{
  section(1000)
  {
    movecontrol(true);
    setenable(0, "Visible", false);
    settransform(1," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
    findmovetarget(5,vector3(0,0,0),25,70,0.5,0.5,0,0);
    settransform(10," ",vector3(0,0,0),eular(0,0,0),"RelativeTarget",false, true);
    setenable(20, "Visible", true);
    playsound(790, "baozha", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhixiao_01", false);
    areadamage(800, 0, 1.2, 0, 3, false) {
      stateimpact("kDefault", 38020301);
      stateimpact("kLauncher", 38020302);
    };
    destroyself(4000);
    setenable(850, "Visible", false);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Baozha_03", 800, vector3(0,0,0),800);
  };
};


//巨炮打击
skill(380205)
{
  //一段出招
  section(933)
  {
    animation("Skill_02A");
    charactereffect("Monster_FX/Boss/6_Mon_Xilie_Xuli_01", 3800, "ef_weapon01", 0, true);
    shakecamera2(100, 3600, false, false, vector3(0.08,0.08,0.08), vector3(25,25,25),vector3(1.5,1.5,1.5),vector3(100,100,100));
    setanimspeed(10, "Skill_02A", 0.25, true);
    setanimspeed(930, "Skill_02A", 1, true);
  };
  //二段开炮
  section(500)
  {
    movecontrol(true);
    animation("Skill_02B");
    summonnpc(10, 101, "Monster_FX/Boss/6_Mon_Xilie_YuJing_01", 380206, vector3(0, 0, 0));
    playsound(140, "kaipao", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_jupaodaji_01", false);
    charactereffect("Monster_FX/Boss/6_Mon_Xilie_PaoKou_01", 800, "ef_weapon01", 160, false);
    startcurvemove(170, true, 0.1, 0, 0, -3, 0, 0, 0);
    shakecamera2(165, 200, false, false, vector3(0.3,0.3,0.3), vector3(100,100,100),vector3(2,2,2),vector3(60,60,60));
  };
  //三段收招
  section(400)
  {
    animation("Skill_02C");
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
  };
};


//巨炮落地
skill(380206)
{
  section(4000)
  {
    movecontrol(true);
    setenable(0, "Visible", false);
    settransform(1," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
    findmovetarget(5, vector3(0,0,0), 25, 360, 0.5, 0.5, 0, 0);
    settransform(10," ",vector3(0,0,0),eular(0,0,0),"RelativeTarget",false,true);
    setenable(20, "Visible", true);
    areadamage(3000, 0, 1.5, 0, 4, false) {
      stateimpact("kDefault", 38020501);
      stateimpact("kLauncher", 38020502);
    };
    shakecamera2(3010,550,false,false,vector3(0.5,1.3,0.5),vector3(50,80,50),vector3(5,7,5),vector3(85,85,85));
    destroyself(4000);
    setenable(3005, "Visible", false);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Paodan_01", 1500, vector3(0,0,0),2800);
    playsound(2950, "baozha", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_jupaodaji_02", false);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Baozha_01", 800, vector3(0,0,0),3000);
  };
};


//连发炮弹
skill(380207)
{
  //一段出招
  section(470)
  {
    animation("Skill_02A");
    setanimspeed(5,"Skill_02A",2);
  };
  //二段开炮
  section(500)
  {
    movecontrol(true);
    animation("Skill_02B");
    summonnpc(110, 101, "Monster_FX/Campaign_Wild/6_Monster_YuJing_01", 380208, vector3(0, 0, 0));
    playsound(146, "kaipao1", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_paodanlianfa", false);
    charactereffect("Monster_FX/Boss/6_Mon_Xilie_PaoKou_01", 800, "ef_weapon01", 160, false);
    startcurvemove(170, true, 0.1, 0, 0, -3, 0, 0, 0);
  };
  section(500)
  {
    movecontrol(true);
    animation("Skill_02B");
    playsound(146, "kaipao2", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_paodanlianfa", false);
    charactereffect("Monster_FX/Boss/6_Mon_Xilie_PaoKou_01", 800, "ef_weapon01", 160, false);
    startcurvemove(170, true, 0.1, 0, 0, -3, 0, 0, 0);
  };
  section(500)
  {
    movecontrol(true);
    animation("Skill_02B");
    playsound(146, "kaipao3", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_paodanlianfa", false);
    charactereffect("Monster_FX/Boss/6_Mon_Xilie_PaoKou_01", 800, "ef_weapon01", 160, false);
    startcurvemove(170, true, 0.1, 0, 0, -3, 0, 0, 0);
  };
  //三段收招
  section(400)
  {
    animation("Skill_02C");
  };
};


//炮弹轰击
skill(380208)
{
  section(4000)
  {
    movecontrol(true);
    setenable(0, "Visible", false);
    settransform(1," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
    setenable(20, "Visible", true);
    //循环段
    findmovetarget(5,vector3(0,0,0),20,360,0.5,0.5,0,0);
    settransform(10," ",vector3(0,0,0),eular(0,0,0),"RelativeTarget",false,true);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Paodan_02", 1500, vector3(0,0,0),70);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Baozha_02", 800, vector3(0,0,0),310);
    areadamage(320, 0, 1.2, 0, 2, true) {
      stateimpact("kDefault", 38020701);
      stateimpact("kLauncher", 38020702);
    };
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Paodan_02", 1500, vector3(0,0,0),750);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Baozha_02", 800, vector3(0,0,0),990);
    areadamage(1000, 0, 1.2, 0, 2, true) {
      stateimpact("kDefault", 38020701);
      stateimpact("kLauncher", 38020702);
    };
    //
    findmovetarget(1005,vector3(0,0,0),20,360,0.5,0.5,0,0);
    settransform(1010," ",vector3(0,0,0),eular(0,0,0),"RelativeTarget",false,true);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Paodan_02", 1500, vector3(0,0,0),1550);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Baozha_02", 800, vector3(0,0,0),1790);
    areadamage(1800, 0, 1.2, 0, 2, true) {
      stateimpact("kDefault", 38020701);
      stateimpact("kLauncher", 38020702);
    };
    //
    findmovetarget(1805,vector3(0,0,0),20,360,0.5,0.5,0,0);
    settransform(1810," ",vector3(0,0,0),eular(0,0,0),"RelativeTarget",false,true);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Paodan_02", 1500, vector3(0,0,0),2350);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_Baozha_02", 800, vector3(0,0,0),2590);
    areadamage(2600, 0, 1.2, 0, 2, true) {
      stateimpact("kDefault", 38020701);
      stateimpact("kLauncher", 38020702);
    };
    //
    shakecamera2(320,100,false,false,vector3(0,0.3,0),vector3(0,50,0),vector3(0,3,0),vector3(85,85,85));
    shakecamera2(1000,100,false,false,vector3(0,0.3,0),vector3(0,50,0),vector3(0,3,0),vector3(85,85,85));
    shakecamera2(1800,100,false,false,vector3(0,0.3,0),vector3(0,50,0),vector3(0,3,0),vector3(85,85,85));
    shakecamera2(2600,100,false,false,vector3(0,0.3,0),vector3(0,50,0),vector3(0,3,0),vector3(85,85,85));
    playsound(310, "baozha", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhixiao_01", false);
    playsound(990, "baozha1", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhixiao_01", false);
    playsound(1790, "baozha2", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhixiao_01", false);
    playsound(2590, "baozha3", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhixiao_01", false);
    setenable(2600, "Visible", false);
    destroyself(3000);
  };
};

//冲撞
skill(380209)
{
  section(1533)
  {
    movecontrol(true);
    animation("Skill_04");
    findmovetarget(400,vector3(0,0,0),8,90,0.5,0.5,0,0.3);
    startcurvemove(480, true, 0.15, 0, 0, 80, 0, 0, 0);
    playsound(470, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_chongji", false);
    charactereffect("Monster_FX/Boss/6_Mon_Xilie_ChongJi_01",1000,"Bone_Root",470,true) {
      transform(vector3(0,0.9,0));
    };
    colliderdamage(490, 150, false, false, 0, 0)
    {
      stateimpact("kDefault", 38020901);
      stateimpact("kLauncher", 38020902);
      sceneboxcollider(vector3(3, 3, 4), vector3(0, 1, 0), eular(0, 0, 0), true, false);
    };
    playsound(520, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(540, 250, true, true, vector3(0,0,0.3), vector3(0,0,150),vector3(0,0,0.6),vector3(0,0,80));
    setanimspeed(850,"Skill_04",2,true);
  };
};

//连击
skill(380210)
{
  section(1733)
  {
    movecontrol(true);
    animation("Skill_05");
    findmovetarget(317, vector3(0,0,0),4,50,0.5,0.5,0,-2);
    startcurvemove(327, true, 0.15, 0, 0, 7, 0, 0, 0);
    findmovetarget(902, vector3(0,0,0),4,50,0.5,0.5,0,-2);
    startcurvemove(907, true, 0.15, 0, 0, 7, 0, 0, 0);
    //playsound(1290, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuda_01", false);
    areadamage(392, 0, 1, 1, 3, true) {
      stateimpact("kDefault", 38021001);
      stateimpact("kLauncher", 38021002);
    };
    //playsound(1330, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    cleardamagestate(800);
    //playsound(2625, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuda_01", false);
    areadamage(987, 0, 1, 1, 3, true) {
      stateimpact("kDefault", 38021003);
      stateimpact("kLauncher", 38021004);
    };
    //playsound(2665, "hit1", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    //charactereffect("Monster_FX/Boss/6_Mon_Xilie_DaoGuang_01", 160, "Bone_Root", 700);
    //charactereffect("Monster_FX/Boss/6_Mon_Xilie_DaoGuang_02", 160, "Bone_Root", 1300);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_DaoGuang_01_01", 3000, vector3(0,-0.2,1), 367, eular(0,0,0), vector3(1,1,1), true);
    sceneeffect("Monster_FX/Boss/6_Mon_Xilie_DaoGuang_01_02", 3000, vector3(0,-0.2,1), 967, eular(0,0,0), vector3(1,1,1), true);
    shakecamera2(397, 150, true, true, vector3(0,0,0.3), vector3(0,0,50),vector3(0,0,1),vector3(0,0,80));
    shakecamera2(997, 180, true, true, vector3(0,0,0.4), vector3(0,0,50),vector3(0,0,1.2),vector3(0,0,80));
    playsound(367, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_henglun_01", false);
    playsound(967, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_xilie_henglun_02", false);
    playsound(397, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    playsound(997, "hit1", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
};
