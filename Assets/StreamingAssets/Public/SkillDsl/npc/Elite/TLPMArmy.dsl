//快速射箭
skill(370201)
{
  section(1050)
  {
    movecontrol(true);
    animation("Attack_01");
    setanimspeed(10, "Attack_01", 2);
    setanimspeed(260, "Attack_01", 1);
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_TLPM_SheJian_02", 420, "ef_weapon01", 0);
    summonnpc(420, 101, "Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_TLPM_SheJian_01", 320108, vector3(0, 0, 0));
    startcurvemove(467, true, 0.05,0,0,-2,0,0,0,0.1,0,0,-0.1,0,0,-10);
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
  };
};


//弩箭连射
skill(370202)
{
  section(1133)
  {
    movecontrol(true);
    animation("Attack_01");
    findmovetarget(0, vector3(0,0,0),10,360,0.5,0.5,0,0);
    facetotarget(10, 1120, 150);
    setanimspeed(10, "Attack_01", 0.5);
    setanimspeed(1010, "Attack_01", 1);
    charactereffect("Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_TLPM_SheJian_02", 2160, "ef_weapon01", 0);
  };
  section(200)
  {
    animation("Attack_02")
    {
      wrapmode(4);
    };
    findmovetarget(0, vector3(0,0,0),10,360,0.5,0.5,0,0);
    summonnpc(30, 101, "Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_TLPM_SheJian_01", 320108, vector3(0, 0, 0));
    facetotarget(1, 200, 150);
  };
  section(200)
  {
    animation("Attack_02")
    {
      wrapmode(4);
    };
    summonnpc(30, 101, "Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_TLPM_SheJian_01", 320108, vector3(0, 0, 0));
    facetotarget(0, 200, 150);
  };
  section(200)
  {
    animation("Attack_02")
    {
      wrapmode(4);
    };
    summonnpc(30, 101, "Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_TLPM_SheJian_01", 320108, vector3(0, 0, 0));
    facetotarget(0, 200, 150);
  };
  section(200)
  {
    animation("Attack_02")
    {
      wrapmode(4);
    };
    summonnpc(30, 101, "Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_TLPM_SheJian_01", 320108, vector3(0, 0, 0));
    facetotarget(0, 200, 100);
  };
  section(200)
  {
    animation("Attack_02")
    {
      wrapmode(4);
    };
    summonnpc(30, 101, "Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_TLPM_SheJian_01", 320108, vector3(0, 0, 0));
    facetotarget(0, 200, 150);
  };
  section(200)
  {
    animation("Attack_02")
    {
      wrapmode(4);
    };
    setanimspeed(50, "Attack_02", 0.25, true);
    summonnpc(30, 101, "Monster_FX/Campaign_Desert/01_TLPMArmy/6_Mon_TLPM_SheJian_01", 370203, vector3(0, 0, 0));
    facetotarget(0, 200, 150);
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
  };
};


//射箭
skill(370203)
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
      stateimpact("kDefault", 37020201);
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


//圣卫军投掷飞轮
skill(370204)
{
  section(1900)
  {
    animation("Skill_01");
    setchildvisible(870,"5_Mon_SWJFlywheel_01_w_01",false);
    summonnpc(870, 101, "Monster/Campaign_Desert/02_SWJ/5_Mon_SWJFlywheel_01_w_01", 370205, vector3(0, 0, 0));
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
skill(370205)
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
      stateimpact("kDefault", 37020401);
      boneboxcollider(vector3(2, 1, 2), "ef_weapon01", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    cleardamagestate(700);
    playsound(701, "huiwu", "Sound/Npc/Mon", 700, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    colliderdamage(701, 700, true, false, 0, 0)
    {
      stateimpact("kDefault", 37020402);
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