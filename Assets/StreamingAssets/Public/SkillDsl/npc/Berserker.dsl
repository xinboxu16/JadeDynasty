//普攻
skill(380101)
{
  //一段出招
  section(1433)
  {
    animation("Attack_01A");
  };
  //二段挥砍
  section(1767)
  {
    movecontrol(true);
    animation("Attack_01B");
    findmovetarget(120, vector3(0,0,0),6,60,0.5,0.5,0,-2);
    startcurvemove(250, true, 0.1, 0, 0, 20, 0, 0, 0, 0.18, 0, 0, 20, 0, 0, -100);
    sceneeffect("Monster_FX/Campaign_Wild/02_Blof/6_Mon_Blof_ChenTu_01", 1000, vector3(0,0,0), 240);
    playsound(305, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_mohuakuangzhanshi_liankan_01", false);
    charactereffect("Monster_FX/Boss/5_Mon_Berserker_DaoGuang_01_01", 1000, "Bone_Root", 310);
    areadamage(335, 0, 1, 1, 3.5, true) {
      stateimpact("kDefault", 38010101);
      stateimpact("kLauncher", 38010102);
    };
    shakecamera2(340, 250, true, true, vector3(0,0,0.5), vector3(0,0,100),vector3(0,0,3),vector3(0,0,70));
    playsound(340, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    cleardamagestate(500);
    playsound(725, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_mohuakuangzhanshi_liankan_02", false);
    charactereffect("Monster_FX/Boss/5_Mon_Berserker_DaoGuang_01_02", 1000, "Bone_Root", 730);
    findmovetarget(730, vector3(0,0,0),6,30,0.5,0.5,0,-2);
    startcurvemove(750, true, 0.05, 0, 0, 20, 0, 0, 0, 0.11, 0, 0, 20, 0, 0, -150);
    areadamage(780, 0, 1, 1, 3.5, true) {
      stateimpact("kDefault", 38010101);
      stateimpact("kLauncher", 38010102);
    };
    shakecamera2(785, 250, true, true, vector3(0,0,0.5), vector3(0,0,100),vector3(0,0,3),vector3(0,0,70));
    playsound(785, "hit1", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
};

//砸地震荡
skill(380102)
{
  //一段出招
  section(920)
  {
    animation("Skill_01A");
    setanimspeed(5, "Skill_01A", 2);
    setanimspeed(915, "Skill_01A", 1);
  };
  //二段出子物体
  section(1567)
  {
    animation("Skill_01B") {
      speed(1);
    };
    playsound(350, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_mohuakuangzhanshi_zadijifei_01", false);
    summonnpc(380, 101, "Monster_FX/Campaign_Wild/6_Monster_YuJing_01", 380107, vector3(0, 0, 0));
    charactereffect("Monster_FX/Campaign_Wild/Public/6_Mon_Baodian_01", 800, "ef_lefthand", 380, false);
    shakecamera2(380, 200, false, true, vector3(0, 0.8, 0), vector3(0, 50, 0), vector3(0, 1, 0), vector3(0, 80, 0));
  };
};


//疯狂连砍
skill(380103)
{
  //一段两下
  section(960)
  {
    movecontrol(true);
    animation("Skill_02");
    findmovetarget(300, vector3(0,0,0),6,60,0.5,0.5,0,-2);
    startcurvemove(330, true, 0.1, 0, 0, 12, 0, 0, 0, 0.10, 0, 0, 10, 0, 0, -60);
    sceneeffect("Monster_FX/Campaign_Wild/02_Blof/6_Mon_Blof_ChenTu_01", 1000, vector3(0,0,0), 320);
    playsound(325, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_mohuakuangzhanshi_liankan_01", false);
    charactereffect("Monster_FX/Boss/5_Mon_Berserker_DaoGuang_01_03", 1000, "Bone_Root", 330);
    summonnpc(370, 101, "Monster_FX/Boss/5_Mon_Berserker_DaoPian_01_02", 380109, vector3(0, 0, 0));
    areadamage(390, 0, 1, 1, 3, true) {
      stateimpact("kDefault", 38010301);
      stateimpact("kLauncher", 38010302);
    };
    shakecamera2(400, 250, true, true, vector3(0,0.6,0.15), vector3(0,100,200),vector3(0,0.6,0.15),vector3(0,80,50));
    playsound(400, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    cleardamagestate(500);
    playsound(705, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_mohuakuangzhanshi_liankan_02", false);
    charactereffect("Monster_FX/Boss/5_Mon_Berserker_DaoGuang_01_04", 1000, "Bone_Root", 710);
    summonnpc(680, 101, "Monster_FX/Boss/5_Mon_Berserker_DaoPian_01_01", 380108, vector3(0, 0, 0));
    areadamage(700, 0, 1, 1, 3, true) {
      stateimpact("kDefault", 38010301);
      stateimpact("kLauncher", 38010302);
    };
    shakecamera2(710, 250, true, true, vector3(0,0,0.12), vector3(0,0,100),vector3(0,0,0.3),vector3(0,0,30));
    playsound(710, "hit1", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
    //二段两下
  section(1767)
  {
    movecontrol(true);
    animation("Attack_01B");
    findmovetarget(120, vector3(0,0,0),6,60,0.5,0.5,0,-2);
    startcurvemove(250, true, 0.1, 0, 0, 20, 0, 0, 0, 0.18, 0, 0, 20, 0, 0, -100);
    sceneeffect("Monster_FX/Campaign_Wild/02_Blof/6_Mon_Blof_ChenTu_01", 1000, vector3(0,0,0), 240);
    playsound(305, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_mohuakuangzhanshi_liankan_01", false);
    charactereffect("Monster_FX/Boss/5_Mon_Berserker_DaoGuang_01_01", 1000, "Bone_Root", 310);
    summonnpc(315, 101, "Monster_FX/Boss/5_Mon_Berserker_DaoPian_01_01", 380108, vector3(0, 0, 0));
    areadamage(335, 0, 1, 1, 3.5, true) {
      stateimpact("kDefault", 38010301);
      stateimpact("kLauncher", 38010302);
    };
    shakecamera2(340, 250, true, true, vector3(0,0,0.5), vector3(0,0,100),vector3(0,0,3),vector3(0,0,70));
    playsound(340, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    cleardamagestate(500);
    playsound(725, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_mohuakuangzhanshi_liankan_02", false);
    charactereffect("Monster_FX/Boss/5_Mon_Berserker_DaoGuang_01_02", 1000, "Bone_Root", 730);
    findmovetarget(730, vector3(0,0,0),6,30,0.5,0.5,0,-2);
    startcurvemove(750, true, 0.05, 0, 0, 20, 0, 0, 0, 0.11, 0, 0, 20, 0, 0, -150);
    summonnpc(760, 101, "Monster_FX/Boss/5_Mon_Berserker_DaoPian_01_02", 380109, vector3(0, 0, 0));
    areadamage(780, 0, 1, 1, 3.5, true) {
      stateimpact("kDefault", 38010301);
      stateimpact("kLauncher", 38010302);
    };
    shakecamera2(785, 250, true, true, vector3(0,0,0.5), vector3(0,0,100),vector3(0,0,3),vector3(0,0,70));
    playsound(785, "hit1", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
};


//咆哮
skill(380104)
{
  //一段出招
  section(267)
  {
    animation("Skill_03A")
    {
      speed(0.3,true);
    };
    setanimspeed(265, "Skill_03A", 1, true);
  };
  section(300)
  {
    animation("Skill_03B")
    {
      speed(0.3,true);
    };
    playsound(180, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_mohuakuangzhanshi_nuhou_01", false);
    sceneeffect("Monster_FX/Boss/5_Mon_Berserker_NuHou_01", 3000, vector3(0, 0.1, 0), 185, eular(0, 0, 0), vector3(1, 1, 1), true);
    shakecamera2(205, 150, false, true, vector3(0.5,0.2,0.5), vector3(100,20,100),vector3(3,1,3),vector3(70,10,70));
    areadamage(200, 0, 1, 0, 8, false) {
      stateimpact("kDefault", 38010401);
    };
  };
};

//位移打击
skill(380105)
{
  //一段位移
  section(600)
  {
    movecontrol(true);
    animation("Attack_01A");
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 0), 9, eular(0, 0, 0), vector3(1, 1, 1), true);
    findmovetarget(0, vector3(0, 0, 0), 25, 360, 0.5, 0.5, 0, 1);
    summonnpc(10, 101, "Monster/Boss/5_BOSS_NiuTou_02", 380110, vector3(0, 0, 0));
    setenable(10, "Visible", false);
    addimpacttoself(15, 30010003, 920);
    settransform(585, " ", vector3( 0, 0.2, -2.5), eular(0, 0, 0), "RelativeTarget", false, true);
    setenable(580, "Visible", true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 0), 586, eular(0, 0, 0), vector3(1, 1, 1), true);
  };
  section(1767)
  {
    movecontrol(true);
    animation("Attack_01B");
    findmovetarget(120, vector3(0,0,0),6,60,0.5,0.5,0,-2);
    startcurvemove(250, true, 0.1, 0, 0, 20, 0, 0, 0, 0.18, 0, 0, 20, 0, 0, -100);
    sceneeffect("Monster_FX/Campaign_Wild/02_Blof/6_Mon_Blof_ChenTu_01", 1000, vector3(0,0,0), 240);
    playsound(305, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_mohuakuangzhanshi_liankan_01", false);
    charactereffect("Monster_FX/Boss/5_Mon_Berserker_DaoGuang_01_01", 1000, "Bone_Root", 310);
    areadamage(335, 0, 1, 1, 3.5, true) {
      stateimpact("kDefault", 38010501);
      stateimpact("kLauncher", 38010502);
    };
    shakecamera2(340, 250, true, true, vector3(0,0,0.5), vector3(0,0,100),vector3(0,0,3),vector3(0,0,70));
    playsound(340, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    cleardamagestate(500);
    playsound(725, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_mohuakuangzhanshi_liankan_02", false);
    charactereffect("Monster_FX/Boss/5_Mon_Berserker_DaoGuang_01_02", 1000, "Bone_Root", 730);
    findmovetarget(730, vector3(0,0,0),6,30,0.5,0.5,0,-2);
    startcurvemove(750, true, 0.05, 0, 0, 20, 0, 0, 0, 0.11, 0, 0, 20, 0, 0, -150);
    areadamage(780, 0, 1, 1, 3.5, true) {
      stateimpact("kDefault", 38010501);
      stateimpact("kLauncher", 38010502);
    };
    shakecamera2(785, 250, true, true, vector3(0,0,0.5), vector3(0,0,100),vector3(0,0,3),vector3(0,0,70));
    playsound(785, "hit1", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
    setenable(0, "Visible", true);
    destroysummonnpc(0);
  };
};


//爆气
skill(380106)
{
  //一段出招
  section(267)
  {
    addimpacttoself(0, 30010003);
    settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeSelf",false,true);
    animation("Skill_03A")
    {
      speed(0.3,true);
    };
    setanimspeed(265, "Skill_03A", 1, true);
  };
  section(1000)
  {
    animation("Skill_03B")
    {
      speed(0.3);
    };
    playsound(180, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_mohuakuangzhanshi_baoqi_01", false);
    sceneeffect("Monster_FX/Boss/5_Mon_Berserker_BaoQi_01", 2000, vector3(0,1.2,0), 190, eular(0,0,0), vector3(1.5,1.5,1.5));
    shakecamera2(205,400,false,true,vector3(2.5,2.5,2.5),vector3(40,40,40),vector3(10,10,10),vector3(65,65,65));
    areadamage(200, 0, 1, 0, 4, false) {
      stateimpact("kDefault", 38010601);
    };
  };
};

//砸地召唤物
skill(380107)
{
  section(1500)
  {
    movecontrol(true);
    setenable(0, "Visible", false);
    settransform(1," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
    findmovetarget(5,vector3(0,0,0),25,360,0.5,0.5,0,0);
    settransform(10," ",vector3(0,0,0),eular(0,0,0),"RelativeTarget",false);
    setenable(20, "Visible", true);
    playsound(880, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/boss_mohuakuangzhanshi_zadijifei_02", false);
    areadamage(1000, 0, 0, 0, 3, false) {
      stateimpact("kDefault", 38010201);
    };
    destroyself(4000);
    setenable(1050, "Visible", false);
    sceneeffect("Monster_FX/Boss/5_Mon_Berserker_ZhenDangBo_01", 850, vector3(0,0,0),800);
    shakecamera2(1000, 100, false, true, vector3(0, 0.8, 0), vector3(0, 50, 0), vector3(0, 1, 0), vector3(0, 80, 0));
  };
};

//飞行召唤物
skill(380108)
{
  section(1500)
  {
    movecontrol(true);
    playsound(0, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhida_01", false) {
      position(vector3(0,0,0),true);
    };
    rotate(10, 1500, vector3(0, 0, -100));
    settransform(0," ",vector3(0,0.3,2),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(10, true, 1.5,0,0,20,0,0,0);
    colliderdamage(10, 1400, false, false, 0, 0)
    {
      stateimpact("kDefault", 38010303);
      stateimpact("kLauncher", 38010304);
      //boneboxcollider(vector3(0.5, 0.5, 1.5), "Bone010", vector3(0, 0, 0), eular(0, 0, 0), true, false);
      sceneboxcollider(vector3(4.5,3,3), vector3(0,2,2), eular(0,0,0), true, false);
    };
    destroyself(1500);
  };
  onmessage("oncollide")
  {
    playsound(0, "hit", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", false) {
      position(vector3(0,0,0), false);
    };
  };
};

//飞行召唤物
skill(380109)
{
  section(1500)
  {
    movecontrol(true);
    playsound(0, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhida_01", false) {
      position(vector3(0,0,0),true);
    };
    rotate(10, 1500, vector3(0, 0, 100));
    settransform(0," ",vector3(0,0.3,2),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(10, true, 1.5,0,0,20,0,0,0);
    colliderdamage(10, 1400, false, false, 0, 0)
    {
      stateimpact("kDefault", 38010303);
      stateimpact("kLauncher", 38010304);
      //boneboxcollider(vector3(0.5, 0.5, 1.5), "Bone010", vector3(0, 0, 0), eular(0, 0, 0), true, false);
      sceneboxcollider(vector3(4.5,3,3), vector3(0,2,2), eular(0,0,0), true, false);
    };
    destroyself(1500);
  };
  onmessage("oncollide")
  {
    playsound(0, "hit", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", false) {
      position(vector3(0,0,0), false);
    };
  };
};


//位移召唤物
skill(380110)
{
  section(1250)
  {
    movecontrol(true);
    setenable(0, "Visible", true);
    animation("Run");
    settransform(0," ",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
    playsound(0, "huiwu", "Sound/Npc/Mon", 1500, "Sound/Npc/boss_mohuakuangzhanshi_weiyidaji_01", false) {
      position(vector3(0,1,0),true);
    };
    startcurvemove(10, true, 0.7,0,0,11,0,0,0);
    charactereffect("Monster_FX/Campaign_Wild/02_Blof/6_Mon_Blof_ChenTu_01", 1000, "Bone_Root", 0);
    colliderdamage(10, 650, false, false, 0, 0)
    {
      stateimpact("kDefault", 38010503);
      boneboxcollider(vector3(3, 2, 3), "Bip001", vector3(0, 1, 0), eular(0, 0, 0), true, false);
    };
    setenable(700, "Visible", false);
    destroyself(1240);
  };
  onmessage("oncollide")
  {
    setenable(2, "Visible", false);
  };
};