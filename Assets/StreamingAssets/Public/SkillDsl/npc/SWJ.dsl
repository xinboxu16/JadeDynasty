//剑盾兵格挡
skill(320201)
{
  section(300)
  {
    animation("Attack_01A");
    addimpacttoself(0, 30010003, 4028);
    addimpacttoself(0, 30010005, 4028);
    addimpacttoself(0, 30010007, 4028);
  };
  section(3728)
  {
    movecontrol(true);
    findmovetarget(0, vector3(0,0,0),10,360,0.5,0.5,0,0);
    animation("Attack_01B")
    {
      wrapmode(4);
    };
    setanimspeed(0,"Attack_01B",0.25);
    facetotarget(5, 3700, 100);
  };
  section(200)
  {
    animation("Attack_01C");
  };
};

//剑盾兵反击
skill(320202)
{
  section(1400)
  {
    movecontrol(true);
    addimpacttoself(0, 30010006, 1000);
    addimpacttoself(0, 30010003, 1400);
    animation("Attack_02");
    findmovetarget(100, vector3(0,0,0),5,60,0.5,0.5,0,-3);
    startcurvemove(116, true, 0.2, 0, 0, 3.5, 0, 0, 0);
    findmovetarget(350, vector3(0,0,0),6,60,0.5,0.5,0,-2.5);
    startcurvemove(380, true, 0.12, 0, 0, 20, 0, 0, 0);
    playsound(420, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_SWJ_DaoGuang_01_02", 1000, "Bone_Root", 430,false);
    areadamage(462, 0, 1, 1, 2.5, true) {
      stateimpact("kDefault", 32020201);
    };
    playsound(470, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(470, 200, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,60));
  };
};

//剑盾兵劈砍
skill(320203)
{
  section(1267)
  {
    movecontrol(true);
    animation("Skill_01");
    findmovetarget(620, vector3(0,0,0),4,30,0.5,0.5,0,-2.6);
    startcurvemove(630, true, 0.08, 0, 0, 3.5, 0, 0, 0);
    playsound(600, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_SWJ_DaoGuang_01_01", 1000, "Bone_Root", 615,false);
    areadamage(640, 0, 1, 1, 2.5, true) {
      stateimpact("kDefault", 32020301);
    };
    playsound(650, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(650, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,40,60));
  };
};



//长矛兵横斩
skill(320204)
{
  section(1500)
  {
    movecontrol(true);
    animation("Attack_01");
    findmovetarget(550, vector3(0,0,0),3,45,0.5,0.5,0,-1.5);
    startcurvemove(562, true, 0.11, 0, 0, 5, 0, 0, 0);
    playsound(630, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_SWJ_DaoGuang_02_01", 2000, "Bone_Root", 630)
    {
      transform(vector3(0,0,0.5));
    };
    areadamage(660, 0, 1, 1, 2.5, true) {
      stateimpact("kDefault", 32020401);
    };
    playsound(665, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(665, 200, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,70));
  };
};


//长矛兵连击
skill(320205)
{
  section(2033)
  {
    movecontrol(true);
    animation("Skill_01");
    findmovetarget(480, vector3(0, 0, 0), 3, 45, 0.5, 0.5, 0, -2);
    startcurvemove(487, true, 0.07, 0, 0, 8, 0, 0, 0);
    playsound(515, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_TLPM_TuCi_01", 2000, "ef_weapon01", 515)
    {
      transform(vector3(0,0,0.5));
    };
    colliderdamage(550, 50, true, true, 0, 0)
    {
      stateimpact("kDefault", 32020501);
      boneboxcollider(vector3(1.5, 1.5, 5), "ef_weapon01", vector3(0, 0, 1), eular(0, 0, 0), true, false);
    };
    playsound(570, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(560, 200, true, true, vector3(0,0,0.5), vector3(0,0,100),vector3(0,0,2),vector3(0,0,70));
//第二下
    cleardamagestate(800);
    playsound(835, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_TLPM_TuCi_01", 2000, "ef_weapon01", 835)
    {
      transform(vector3(0,0,0.5));
    };
    colliderdamage(870, 50, true, true, 0, 0)
    {
      stateimpact("kDefault", 32020501);
      boneboxcollider(vector3(1.5, 1.5, 5), "ef_weapon01", vector3(0, 0, 1), eular(0, 0, 0), true, false);
    };
    playsound(880, "hit1", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(880, 200, true, true, vector3(0,0,0.5), vector3(0,0,100),vector3(0,0,2),vector3(0,0,70));
//第三下
    findmovetarget(1400, vector3(0,0,0),4,45,0.5,0.5,0,-1.5);
    startcurvemove(1406, true, 0.11, 0, 0, 5, 0, 0, 0);
    playsound(1440, "huiwu2", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_SWJ_DaoGuang_02_02", 2000, "Bone_Root", 1440)
    {
     transform(vector3(0,0,0.5));
    };
    areadamage(1482, 0, 1, 1, 2.5, true) {
      stateimpact("kDefault", 32020502);
    };
    playsound(1490, "hit2", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(1490, 200, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,70));
  };
};



//飞轮手横抡
skill(320206)
{
  section(2267)
  {
    movecontrol(true);
    animation("Attack_01");
    startcurvemove(627, true, 0.32, 0, 0, 10, 0, 0, 0);
    playsound(745, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_SWJ_Flywheel_01", 300, "ef_weapon01", 700);
    colliderdamage(700, 220, false, false, 0, 0)
    {
      stateimpact("kDefault", 32020601);
      boneboxcollider(vector3(3.5, 3.5, 2), "ef_weapon01", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    playsound(755, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(755, 200, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,70));
  };
};


//投掷飞轮
skill(320207)
{
  section(1900)
  {
    animation("Skill_01");
    setchildvisible(870,"5_Mon_SWJFlywheel_01_w_01",false);
    summonnpc(870, 101, "Monster/Campaign_Desert/02_SWJ/5_Mon_SWJFlywheel_01_w_01", 320208, vector3(0, 0, 0));
    setchildvisible(1850,"5_Mon_SWJFlywheel_01_w_01",true);
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    setchildvisible(0,"5_Mon_SWJFlywheel_01_w_01",true);
  };
  onstop() //技能正常结束时会运行该段逻辑
  {
    setchildvisible(0,"5_Mon_SWJFlywheel_01_w_01",true);
  };
};


//飞轮
skill(320208)
{
  section(1410)
  {
    movecontrol(true);
    playsound(0, "huiwu", "Sound/Npc/Mon", 700, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    rotate(10, 1400, vector3(0, 2000, 0));
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_SWJ_Flywheel_01", 2000, "ef_weapon01", 0);
    setenable(0, "Visible", true);
    settransform(0," ",vector3(0.2,1.1,0.2),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(5, true, 0.7,0,0,16,0,0,0,0.7,0,0,-16,0,0,0);
    colliderdamage(10, 690, true, false, 0, 0)
    {
      stateimpact("kDefault", 32020701);
      boneboxcollider(vector3(2, 1, 2), "ef_weapon01", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    cleardamagestate(700);
    playsound(701, "huiwu", "Sound/Npc/Mon", 700, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    colliderdamage(701, 700, true, false, 0, 0)
    {
      stateimpact("kDefault", 32020702);
      boneboxcollider(vector3(2, 1, 2), "ef_weapon01", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    destroyself(1410);
  };
  onmessage("oncollide")
  {
    playsound(0, "hit", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", false) {
      position(vector3(0,0,0), false);
    };
    shakecamera2(0, 100, true, true, vector3(0,0,0.2), vector3(0,0,50),vector3(0,0,0.5),vector3(0,0,70));
  };
};