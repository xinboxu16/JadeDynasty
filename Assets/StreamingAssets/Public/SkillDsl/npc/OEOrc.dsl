//抡砸
skill(300701)
{
  section(2333)
  {
    animation("Attack_01");
    charactereffect("Monster_FX/Campaign_Wild/07_OEOrc/6_Mon_OEOrc_DaoGuang_01", 160, "Bone_Root", 1370);
    playsound(1380, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuda_01", false);
    areadamage(1416, 0, 1, 2, 3, false) {
      stateimpact("kDefault", 30070101);
    };
    playsound(1420, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(1420, 200, false, true, vector3(0.12,0.4,0.12), vector3(40,80,40),vector3(0.15,0.8,0.15),vector3(60,50,60));
  };
};


//砸地
skill(300702)
{
  section(2800)
  {
    animation("Skill_01");
    charactereffect("Monster_FX/Campaign_Wild/07_OEOrc/6_Mon_OEOrc_DaoGuang_02", 1000, "Bone_Root", 1675);
    sceneeffect("Monster_FX/Campaign_Wild/07_OEOrc/6_Mon_OEOrc_ZhenDangBo_01", 1500, vector3(0,0,3),1700);
    playsound(1690, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuda_01", false);
    playsound(1715, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_zadixiao_01", false);
    areadamage(1736, 0, 1, 2, 3, true) {
      stateimpact("kDefault", 30070201);
    };
    playsound(1740, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(1740, 400, true, true, vector3(0.12,0.5,0.12), vector3(50,100,50),vector3(0.2,1,0.2),vector3(60,50,60));
  };
};


//跃起砸地
skill(300703)
{
  section(1967)
  {
    movecontrol(true);
    animation("Skill_01");
    findmovetarget(67, vector3(0,0,0),8,50,0.5,0.5,0,-3);
    startcurvemove(210, false, 0.62, 0, 0, 10, 0, 0, 0);
    playsound(860, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuda_01", false);
    sceneeffect("Monster_FX/Campaign_Wild/07_OEOrc/6_Mon_OEOrc_ZhenDangBo_02", 1500, vector3(0,0,2.5),880);
    areadamage(895, 0, 1, 1, 5, true) {
      stateimpact("kDefault", 30070301);
    };
    playsound(900, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", false);
    shakecamera2(900, 300, false, true, vector3(0.3,0.9,0.3), vector3(100,100,100),vector3(0.6,1.5,0.6),vector3(80,60,80));
  };
};


//投掷巨石
skill(300704)
{
  section(2267)
  {
    animation("Attack_01") ;
    setchildvisible(1530,"5_Mon_OneyeOrc_02_w_02",false);
    summonnpc(1520, 101, "Monster/Campaign_Wild/07_OEOrc/5_Mon_OneyeOrc_02_w_02", 300705, vector3(0, 0, 0));
    setchildvisible(2265,"5_Mon_OneyeOrc_02_w_02",true);
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    setchildvisible(0,"5_Mon_OneyeOrc_02_w_02",true);
  };
  onstop() //技能正常结束时会运行该段逻辑
  {
    setchildvisible(0,"5_Mon_OneyeOrc_02_w_02",true);
  };
};


//巨石飞行
skill(300705)
{
  section(1500)
  {
    movecontrol(true);
    playsound(0, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    setenable(5, "Visible", true);
    rotate(10, 1500, vector3(540, 0, 0));
    settransform(0," ",vector3(0,1.6,0.1),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(10, true, 1.5,0,1.2,18,0,-8,0);
    colliderdamage(5, 1500, false, false, 0, 0)
    {
      stateimpact("kDefault", 30070401);
      oncollidelayer("Terrains", "onterrain");
      oncollidelayer("Default", "onterrain");
      boneboxcollider(vector3(1.1, 1.1, 1.1), "Bone001", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    destroyself(1500);
  };
  onmessage("onterrain")
  {
    setenable(10, "CurveMove", false);
    setenable(10, "Rotate", false);
    setenable(10, "Damage", false);
  };
  onmessage("oncollide")
  {
    playsound(0, "hit", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", false) {
      position(vector3(0,0,0), false);
    };
    shakecamera2(0, 200, false, false, vector3(0,0.2,0.5), vector3(0,100,100),vector3(0,0.3,1),vector3(0,80,80));//效果还不错
    setenable(10, "Damage", false);
    destroyself(500);
  };
};