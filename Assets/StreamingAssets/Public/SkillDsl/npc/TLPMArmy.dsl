//剑盾兵劈砍
skill(320101)
{
  section(2300)
  {
    movecontrol(true);
    animation("Attack_01");
    findmovetarget(900, vector3(0,0,0),2,60,0.5,0.5,0,-1);
    startcurvemove(966, true, 0.15, 0, 0, 4, 0, 0, 0);
    playsound(1080, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_TLPM_DaoGuang_01", 2000, "Bone_Root", 1080);
    areadamage(1120, 0, 1, 1, 2, true) {
      stateimpact("kDefault", 32010101);
    };
    playsound(1130, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(1130, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,40,60));
  };
};

//剑盾兵防御
skill(320102)
{
  section(2567)
  {
    animation("Skill_01");
    addimpacttoself(0, 30010003, 2600);
    addimpacttoself(0, 30010005, 2600);
    setanimspeed(400, "Skill_01", 0.25);
    setanimspeed(2400, "Skill_01", 1);
  };
  onstop()
  {
    addimpacttoself(0, 30010006, 100);
  };
  oninterrupt()
  {
    addimpacttoself(0, 30010006, 100);
  };
};

//长矛兵横斩
skill(320103)
{
  section(2300)
  {
    movecontrol(true);
    animation("Attack_01");
    findmovetarget(900, vector3(0,0,0),2,60,0.5,0.5,0,-1);
    startcurvemove(966, true, 0.15, 0, 0, 4, 0, 0, 0);
    playsound(1075, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_TLPM_DaoGuang_02", 2000, "Bone_Root", 1075);
    areadamage(1115, 0, 1, 1, 2, true) {
      stateimpact("kDefault", 32010301);
    };
    playsound(1120, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(1120, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
  };
};


//长矛兵突刺
skill(320104)
{
  section(1267)
  {
    movecontrol(true);
    animation("Skill_01");
    startcurvemove(475, true, 0.07, 0, 0, 4, 0, 0, 0);
    playsound(505, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_TLPM_TuCi_01", 2000, "ef_weapon01", 505);
    colliderdamage(520, 100, false, false, 0, 0)
    {
      stateimpact("kDefault", 32010401);
      boneboxcollider(vector3(1.5, 1.5, 4), "ef_weapon01", vector3(0, 0, 1), eular(0, 0, 0), true, false);
    };
    // areadamage(525, 0, 1, 1, 2, true) {
    //   stateimpact("kDefault", 32010401);
    // };
    playsound(530, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(530, 200, true, true, vector3(0,0,0.5), vector3(0,0,100),vector3(0,0,2),vector3(0,0,70));
  };
};


//长矛兵连砍
skill(320105)
{
  section(1867)
  {
    movecontrol(true);
    animation("Skill_02");
    findmovetarget(300, vector3(0, 0, 0), 2, 45, 0.5, 0.5, 0, -1);
    startcurvemove(310, true, 0.1, 0, 0, 2, 0, 0, 0);
    playsound(400, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_TLPM_DaoGuang_03", 2000, "Bone_Root", 400);
    areadamage(414, 0, 1, 1, 2, true) {
      stateimpact("kDefault", 32010501);
    };
    playsound(420, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(420, 200, true, true, vector3(0,0,0.4), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,60));
    cleardamagestate(450);
    playsound(1080, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_TLPM_DaoGuang_04", 2000, "Bone_Root", 1080);
    areadamage(1110, 0, 1, 1, 2, true) {
      stateimpact("kDefault", 32010501);
    };
    playsound(1120, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(1120, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
  };
};

//弓手横抡
skill(320106)
{
  section(2300)
  {
    movecontrol(true);
    animation("Skill_01");
    findmovetarget(1050, vector3(0,0,0),2,60,0.5,0.5,0,-1.5);
    startcurvemove(1090, true, 0.1, 0, 0, 3, 0, 0, 0);
    playsound(1075, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_TLPM_DaoGuang_05", 2000, "Bone_Root", 1075);
    areadamage(1110, 0, 1, 1, 2, true) {
      stateimpact("kDefault", 32010601);
    };
    playsound(1115, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(1115, 200, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,50,70));
  };
};


//射箭
skill(320107)
{
  section(1800)
  {
    movecontrol(true);
    animation("Attack_01");
    setanimspeed(10, "Attack_01", 0.5);
    setanimspeed(1010, "Attack_01", 1);
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_TLPM_SheJian_02", 1160, "ef_weapon01", 0);
    summonnpc(1160, 101, "Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_TLPM_SheJian_01", 320108, vector3(0, 0, 0));
    startcurvemove(1167, true, 0.05,0,0,-2,0,0,0,0.1,0,0,-0.1,0,0,-10);
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
  };
};


//射箭
skill(320108)
{
  section(800)
  {
    movecontrol(true);
    setenable(0, "Visible", true);
    settransform(0," ",vector3(0,1.3,0),eular(0,0,0),"RelativeOwner",false);
    playsound(0, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuxiao_01", false){
      position(vector3(0,0,0),true);
    };
    startcurvemove(5, true, 0.8,0,0,30,0,0,0);
    colliderdamage(10, 800, false, false, 0, 0)
    {
      stateimpact("kDefault", 32010701);
      boneboxcollider(vector3(0.5, 0.5, 2), "Bone", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    destroyself(800);
  };
  onmessage("oncollide")
  {
    playsound(0, "hit", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", false){
      position(vector3(0,0,0), false);
    };
    shakecamera2(0, 250, false, false, vector3(0,0,0.2), vector3(0,0,100),vector3(0,0,0.8),vector3(0,0,50));//效果还不错
  };
};