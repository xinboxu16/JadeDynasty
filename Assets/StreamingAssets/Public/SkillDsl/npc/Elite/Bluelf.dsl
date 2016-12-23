//烧瓶哥布林多瓶投掷
skill(370101)
{
  section(3000)
  {
    movecontrol(true);
    sceneeffect("Monster_FX/Campaign_Wild/6_Monster_YuJing_01", 4500, vector3(0,0.2,6.7));
    sceneeffect("Monster_FX/Campaign_Wild/6_Monster_YuJing_01", 4500, vector3(-3.3,0.2,6.7));
    sceneeffect("Monster_FX/Campaign_Wild/6_Monster_YuJing_01", 4500, vector3(3.3,0.2,6.7));
    animation("Bluelf04_Attack_01");
    startcurvemove(2100, true, 0.1, 0, 0, 3, 0, 0, 0);
    setchildvisible(2480,"5_Mon_Bluelf_w_04",false);
    summonnpc(2480, 101, "Monster/Campaign_Wild/01_Bluelf/6_Mon_Bluelf_Bomb_01", 370102, vector3(0, 0, 0));
    summonnpc(2480, 101, "Monster/Campaign_Wild/01_Bluelf/6_Mon_Bluelf_Bomb_01", 370103, vector3(0, 0, 0));
    summonnpc(2480, 101, "Monster/Campaign_Wild/01_Bluelf/6_Mon_Bluelf_Bomb_01", 370104, vector3(0, 0, 0));
    setchildvisible(2950,"5_Mon_Bluelf_w_04",true);
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    setchildvisible(0,"5_Mon_Bluelf_w_04",true);
  };
  onstop() //技能正常结束时会运行该段逻辑
  {
    setchildvisible(0,"5_Mon_Bluelf_w_04",true);
  };
};


//飞行燃烧瓶1
skill(370102)
{
  section(5500)
  {
    movecontrol(true);
    setenable(0, "Visible", true);
    rotate(10, 2500, vector3(-720, 0, 0));
    settransform(0," ",vector3(0,1,0),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(10, true, 3,-3,5,4.5,0,-9,0);
    colliderdamage(10, 3000, false, false, 0, 0)
    {
      stateimpact("kDefault", 30010600);
      oncollidelayer("Terrains", "onterrain");
      oncollidelayer("Default", "onterrain");
      boneboxcollider(vector3(0.8, 0.8, 0.8), "Bone", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    destroyself(6000);
  };
  onmessage("onterrain")
  {
    cleardamagepool(0);//清除之前造成伤害的记录
    playsound(0, "baozha", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhixiao_01", false);
    setenable(0, "CurveMove", false);
    setenable(0, "Rotate", false);
    setenable(0, "Visible", false);
//爆炸伤害
    areadamage(10, 0, 0.5, 0, 3, false) {
      stateimpact("kDefault", 30010601);
    };
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Mon_Bluelf_BaoZha_01", 1500, vector3(0,0,0));
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Mon_Bluelf_RanShao_01", 5000, vector3(0,0,0));
//燃烧持续伤害
    cleardamagepool(200);//清除之前造成伤害的记录
    colliderdamage(500, 3500, true , true, 1000, 0) {
      stateimpact("kDefault", 30010602);
      sceneboxcollider(vector3(3,3,3), vector3(0, 0, 0), eular(0, 0, 0), false, false);
    };
    playsound(400, "ranshao", "Sound/Npc/Mon", 3000, "Sound/Npc/guaiwu_touzhixiao_02", false);
  };
};

//飞行燃烧瓶2
skill(370103)
{
  section(5500)
  {
    movecontrol(true);
    setenable(0, "Visible", true);
    rotate(10, 2500, vector3(-720, 0, 0));
    settransform(0," ",vector3(0,1,0),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(10, true, 3,0,5,4.5,0,-9,0);
    colliderdamage(10, 3000, false, false, 0, 0)
    {
      stateimpact("kDefault", 30010600);
      oncollidelayer("Terrains", "onterrain");
      oncollidelayer("Default", "onterrain");
      boneboxcollider(vector3(0.8, 0.8, 0.8), "Bone", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    destroyself(6000);
  };
  onmessage("onterrain")
  {
    cleardamagepool(0);//清除之前造成伤害的记录
    playsound(0, "baozha", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhixiao_01", false);
    setenable(0, "CurveMove", false);
    setenable(0, "Rotate", false);
    setenable(0, "Visible", false);
//爆炸伤害
    areadamage(10, 0, 0.5, 0, 3, false) {
      stateimpact("kDefault", 30010601);
    };
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Mon_Bluelf_BaoZha_01", 1500, vector3(0,0,0));
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Mon_Bluelf_RanShao_01", 5000, vector3(0,0,0));
//燃烧持续伤害
    cleardamagepool(200);//清除之前造成伤害的记录
    colliderdamage(500, 3500, true , true, 1000, 0) {
      stateimpact("kDefault", 30010602);
      sceneboxcollider(vector3(3,3,3), vector3(0, 0, 0), eular(0, 0, 0), false, false);
    };
    playsound(400, "ranshao", "Sound/Npc/Mon", 3000, "Sound/Npc/guaiwu_touzhixiao_02", false);
  };
};

//飞行燃烧瓶3
skill(370104)
{
  section(5500)
  {
    movecontrol(true);
    setenable(0, "Visible", true);
    rotate(10, 2500, vector3(-720, 0, 0));
    settransform(0," ",vector3(0,1,0),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(10, true, 3,3,5,4.5,0,-9,0);
    colliderdamage(10, 3000, false, false, 0, 0)
    {
      stateimpact("kDefault", 30010600);
      oncollidelayer("Terrains", "onterrain");
      oncollidelayer("Default", "onterrain");
      boneboxcollider(vector3(0.8, 0.8, 0.8), "Bone", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    destroyself(6000);
  };
  onmessage("onterrain")
  {
    cleardamagepool(0);//清除之前造成伤害的记录
    playsound(0, "baozha", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhixiao_01", false);
    setenable(0, "CurveMove", false);
    setenable(0, "Rotate", false);
    setenable(0, "Visible", false);
//爆炸伤害
    areadamage(10, 0, 0.5, 0, 3, false) {
      stateimpact("kDefault", 30010601);
    };
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Mon_Bluelf_BaoZha_01", 1500, vector3(0,0,0));
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Mon_Bluelf_RanShao_01", 5000, vector3(0,0,0));
//燃烧持续伤害
    cleardamagepool(200);//清除之前造成伤害的记录
    colliderdamage(500, 3500, true , true, 1000, 0) {
      stateimpact("kDefault", 30010602);
      sceneboxcollider(vector3(3,3,3), vector3(0, 0, 0), eular(0, 0, 0), false, false);
    };
    playsound(400, "ranshao", "Sound/Npc/Mon", 3000, "Sound/Npc/guaiwu_touzhixiao_02", false);
  };
};