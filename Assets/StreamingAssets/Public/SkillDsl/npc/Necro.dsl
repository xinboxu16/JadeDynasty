//瘟疫弹
skill(310401)
{
  section(2933)
    {
      movecontrol(true);
      animation("Skill_01");
      findmovetarget(0, vector3(0,0,0),12,360,0.5,0.5,0,0);
      charactereffect("Monster_FX/Campaign_Dungeon/04_Necro/5_Mon_Necro_Toxic_01", 1750, "ef_lefthand", 520, true);
      facetotarget(5, 2300, 100);
      summonnpc(2200, 101, "Monster_FX/Campaign_Dungeon/04_Necro/5_Mon_Necro_Toxic_02", 310402, vector3(0, 0, 0));
    };
    oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
  };
};

//瘟疫弹飞行
skill(310402)
{
  section(2000)
  {
    movecontrol(true);
    setenable(0, "Visible", false);
    settransform(1," ",vector3(0,1.4,-0.1),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(5, true, 2,0,0,12,0,0,0);
    setenable(5, "Visible", true);
    colliderdamage(10, 2000, false, false, 0, 0)
    {
      stateimpact("kDefault", 31040101);
      boneboxcollider(vector3(0.5, 0.5, 0.5), "Bone", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    destroyself(2000);
  };
  onmessage("oncollide")
  {
    destroyself(5);
  };
};


//毒爆新星
skill(310403)
{
  section(2400)
  {
    animation("Attack_01");
    summonnpc(10, 101, "Monster_FX/Campaign_Wild/6_Monster_YuJing_01", 310404, vector3(0, 0, 0));
    charactereffect("Monster_FX/Campaign_Dungeon/04_Necro/5_Mon_Necro_Toxic_01", 1550, "ef_weapon01", 8, true);
  };
};

//新星爆发
skill(310404)
{
  section(3000)
  {
    movecontrol(true);
    setenable(0, "Visible", false);
    settransform(1," ",vector3(0,0.3,6),eular(0,0,0),"RelativeOwner",false);
    findmovetarget(5,vector3(0,0,0),12,360,0.5,0.5,0,0);
    settransform(10," ",vector3(0,0,0),eular(0,0,0),"RelativeTarget",false);
    setenable(15, "Visible", true);
    areadamage(1600, 0, 0, 0, 3, false) {
      stateimpact("kDefault", 31040301);
    };
    destroyself(4000);
    setenable(1600, "Visible", false);
    sceneeffect("Monster_FX/Campaign_Dungeon/04_Necro/5_Mon_Necro_ToxicNova_01", 1500, vector3(0,0,0),1570);
  };
};

//瞬移
skill(310405)
{
  section(2933)
  {
    animation("Attack_01");
    movecontrol(true);
    findmovetarget(5,vector3(0,0,0),10,90,0.5,0.5,0,1);
    createshadow(1845, 100, 1, 500, 500, "shadow", "Transparent/Cutout/Soft Edge Unlit", 0.8);
    setenable(1850, "Visible", false);
    settransform(1860," ",vector3(3,0.2,-6),eular(0,0,0),"RelativeTarget",false,true);
    setenable(2000, "Visible", true);
    sceneeffect("Hero_FX/3_Cike/3_Hero_CiKe_CiJiShouJi_HeiYing_01_002", 3000, vector3(0, 0, 0), 1845, eular(0, 0, 0), vector3(1, 1, 1), true);
  };
};


//召唤
skill(310406)
{
  section(2933)
    {
      animation("Skill_01");
    };
};


//平移
skill(310407)
{
  section(1200)
    {
      movecontrol(true);
      animation("Walk");
      startcurvemove(1, false, 1.2, 7, 0, 0.01, 0, 0, 0);
      findmovetarget(100,vector3(0,0,0),10,360,0.5,0.5,0,5);
      facetotarget(101,1000,300);
    };
};

//平移2
skill(310408)
{
  section(1200)
    {
      movecontrol(true);
      animation("Walk");
      startcurvemove(1, false, 1.2, -7, 0, 0.01, 0, 0, 0);
      findmovetarget(100,vector3(0,0,0),10,360,0.5,0.5,0,5);
      facetotarget(101,1000,300);
    };
};
