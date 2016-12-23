//矿工投掷不稳定水晶
skill(310301)
{
  section(2533)
  {
    movecontrol(true);
    sceneeffect("Monster_FX/Campaign_Wild/6_Monster_YuJing_01", 3000, vector3(0,0.2,6));
    animation("Attack_01");
    startcurvemove(1760, true, 0.1, 0, 0, 2, 0, 0, 0);
    setchildvisible(1760,"5_Mon_BombMiner_01_w_01",false);
    summonnpc(1740, 101, "Monster/Campaign_Dungeon/03_Miner/5_Mon_BombMiner_01_w_01", 310302, vector3(0, 0, 0));
    setchildvisible(2500,"5_Mon_BombMiner_01_w_01",true);
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    setchildvisible(2500,"5_Mon_BombMiner_01_w_01",true);
  };
  onstop() //技能正常结束时会运行该段逻辑
  {
    setchildvisible(2500,"5_Mon_BombMiner_01_w_01",true);
  };
};


//投掷水晶
skill(310302)
{
  section(6000)
  {
    movecontrol(true);
    setenable(0, "Visible", true);
    rotate(10, 2500, vector3(40, 0, 0));
    settransform(0," ",vector3(0,1.5,0),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(10, true, 3,0,5,4.5,0,-9,0);
    colliderdamage(10, 3000, false, false, 0, 0)
    {
      stateimpact("kDefault", 31030101);
      oncollidelayer("Terrains", "onterrain");
      oncollidelayer("Default", "onterrain");
      boneboxcollider(vector3(0.8, 0.8, 0.8), "Bone001", vector3(0, 0, 0), eular(0, 0, 0), true, false);
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
      stateimpact("kDefault", 31030102);
    };
    sceneeffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDM_BaoZha_01", 2000, vector3(0,0,0));
  };
};


//矿工劈砍
skill(310303)
{
  section(2433)
  {
    movecontrol(true);
    animation("Attack_01");
    charactereffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Mon_Bluelf_DaoGuang_02", 2000, "Bone_Root", 1070) {
      transform(vector3(0,0.5,-0.1));
    };
    findmovetarget(1533, vector3(0,0,0),3,50,0.5,0.5,0,-2);
    startcurvemove(1566, true, 0.07, 0, 0, 4, 0, 0, 100);
    areadamage(1667, 0, 1, 0.5, 2, true) {
      stateimpact("kDefault", 31030301);
    };
    //sound("Sound/Swordman/TiaoKong", 0);//无定帧时的音效开始时间
    shakecamera2(1670, 250, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,40,60));
  };
};

//矿工挥砍
skill(310304)
{
  section(2267)
  {
    animation("Skill_01");
    charactereffect("Monster_FX/Campaign_Dungeon/03_Miner/6_Mon_HoeMiner_DaoGuang_01", 2000, "Bone_Root", 1510);
    //sound("Sound/Swordman/TiaoKong", 1500);//无定帧时的音效开始时间
    areadamage(1533, 0, 1, 1, 2.5, false) {
      stateimpact("kDefault", 31030401);
    };
//    lockframe(720, "Bluelf03_Attack_01", true, 0, 100, 1, 100);
//    shakecamera(740, true, 40, 20, 100, 0.5);
    shakecamera2(1540, 200, true, true, vector3(0,0,0.1), vector3(0,0,50),vector3(0,0,1),vector3(0,0,50));
  };
};

