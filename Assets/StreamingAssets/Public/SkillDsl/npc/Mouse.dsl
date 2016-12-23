//钻地鼠爪击
skill(300501)
{
  section(2133)
  {
    movecontrol(true);
    animation("Attack_01");
    charactereffect("Monster_FX/Campaign_Wild/05_Mouse/5_Mon_BurrowMouse_01_003", 500, "Bone_Root", 1120);
    playsound(1110, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuxiao_03", false);
    areadamage(1140, 0, 1, 0.6, 1.8, true) {
      stateimpact("kDefault", 30050103);
    };
    playsound(1150, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(1150, 200, true, true, vector3(0,0.1,0), vector3(0,100,0),vector3(0,0.5,0),vector3(0,80,0));
  };
};


//钻地鼠翻滚
skill(300502)
{
  section(2867)//0翻滚第一段
  {
    movecontrol(true);
    animation("Skill_01A");
    findmovetarget(2330, vector3(0,0,0),5,60,0.5,0.5,0,-2);
    startcurvemove(2390, true, 0.45, 0, 0, 4, 0, 0, 6);
  };
  section(1602)//1翻滚第二段
  {
    animation("Skill_01B") {
      wrapmode(2);
    };
    startcurvemove(0, true, 1602, 0, 0, 8, 0, 0, 4);
    playsound(10, "huiwu", "Sound/Npc/Mon_Loop", 1800, "Sound/Npc/guaiwu_fangun_01", false);
    charactereffect("Monster_FX/Campaign_Wild/05_Mouse/5_Mon_BurrowMouse_FanGunYan_01", 1600, "Bone_Root", 0);
    colliderdamage(20, 1550, false, true, 300, 0)
    {
      stateimpact("kDefault", 30050201);
      boneboxcollider(vector3(1.7, 1.7, 1.7), "Bone_Root", vector3(0, 1, 0), eular(0, 0, 0), true, false);
    };
  };
  section(933)//2翻滚第三段
  {
    animation("Skill_01C");
    startcurvemove(0, true, 0.24, 0, 0, 5, 0, 0, -25);
    gotosection(910, 4, 0);
  };
  section(1883)//3撞击成功段
  {
    stopsound(0,"huiwu");
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(0, 200, false, true, vector3(0,0.5,0.5), vector3(0,150,150),vector3(0,0.6,0.6),vector3(0,80,80));
    lockframe(0,"Skill_01B",false,0,30,1,60,true);
    animation("Skill_01B")
    {
      wrapmode(2);
    };
    startcurvemove(10, true, 0.12, 0, 0, 3, 0, 0, -60, 0.4, 0, 20, -4, 0, -100, -12);
    animation("Skill_01D",534);
  };
  onmessage("oncollide")
  {
    gotosection(0, 3, 0);
  };
  section(1)//4正常结束段
  {
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
    stopsound(0,"huiwu");
  };
};


//瘟疫鼠放毒
skill(300503)
{
  section(2700)
  {
    movecontrol(true);
    animation("Attack_01");
    charactereffect("Monster_FX/Campaign_Wild/05_Mouse/5_Mon_BurrowMouse_DuPao_01", 2000, "Bip001 Spine1", 330)
    {
     transform(vector3(-0.2,0,0),eular(0,0,0),vector3(0.5,0.5,0.5));
   };
    sceneeffect("Monster_FX/Campaign_Wild/05_Mouse/6_Mon_Mouse_DuWu_01", 5000, vector3(0, 0.2, 0), 1300);
    colliderdamage(1500, 900, false, true, 1000, 0) {
      stateimpact("kDefault", 30050301);
      sceneboxcollider(vector3(5.5, 1.5, 5.5), vector3(0, 0, 0), eular(0, 0, 0), false, false);
    };
  };
};

//小鼠普攻
skill(300505)
{
  section(2133)
  {
    movecontrol(true);
    animation("Attack_01");
    playsound(1310, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuxiao_02", false);
    areadamage(1333, 0, 1, 0.5, 1.6, true) {
      stateimpact("kDefault", 30050401);
    };
    playsound(1340, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    charactereffect("Monster_FX/Campaign_Wild/05_Mouse/5_Mon_BurrowMouse_01_001", 500, "Bone_Root", 1310);
    charactereffect("Monster_FX/Campaign_Wild/05_Mouse/5_Mon_BurrowMouse_01_002", 500, "Bone_Root", 1600);
    playsound(1600, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuxiao_02", false);
    areadamage(1620, 0, 1, 0.5, 1.6, true) {
      stateimpact("kDefault", 30050402);
    };
    playsound(1630, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
};


//小鼠普攻去前摇版
skill(300504)
{
  section(1133)
  {
    movecontrol(true);
    animation("Skill_01");
    playsound(310, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuxiao_02", false);
    areadamage(333, 0, 1, 0.5, 1.6, true) {
      stateimpact("kDefault", 30050401);
    };
    playsound(340, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    charactereffect("Monster_FX/Campaign_Wild/05_Mouse/5_Mon_BurrowMouse_01_001", 500, "Bone_Root", 310);
    charactereffect("Monster_FX/Campaign_Wild/05_Mouse/5_Mon_BurrowMouse_01_002", 500, "Bone_Root", 600);
    playsound(600, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuxiao_02", false);
    areadamage(620, 0, 1, 0.5, 1.6, true) {
      stateimpact("kDefault", 30050402);
    };
    playsound(630, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
  };
};