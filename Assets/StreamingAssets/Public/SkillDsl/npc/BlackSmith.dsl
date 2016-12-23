//壮汉单发
skill(300601)
{
  section(2567)
  {
    movecontrol(true);
    animation("Attack_01");
    summonnpc(10, 101, "Monster_FX/Campaign_Wild/6_Monster_YuJing_01", 300602, vector3(0, 0, 0));
    playsound(1715, "kaipao", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_shoothuopao_01", false);
    charactereffect("Monster_FX/Campaign_Wild/06_BlackSmith/6_Mon_KAC_PaoKou_01", 800, "ef_weapon01", 1748, false);
    startcurvemove(1768, true, 0.1, 0, 0, -3, 0, 0, 0);
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    destroysummonnpc(0);
  };
};

//单发炮弹
skill(300602)
{
  section(3000)
  {
    movecontrol(true);
    setenable(0, "Visible", true);
    settransform(1, " ", vector3(0, 0, 0), eular(0,0,0), "RelativeOwner", false);
    findmovetarget(2, vector3(0,0,0),10,60,0.5,0.5,0,-1);
    settransform(10," ",vector3(0,0,0),eular(0,0,0),"RelativeTarget",false,true);
    playsound(1738, "baozha", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhixiao_01", false);
    areadamage(1748, 0, 1, 0, 3, false) {
      stateimpact("kDefault", 30060101);
    };
    destroyself(3000);
    setenable(1800, "Visible", false);
    sceneeffect("Monster_FX/Campaign_Wild/06_BlackSmith/6_Mon_KAC_BaoZha_01", 1500, vector3(0,0,0),1748);
  };
};

//壮汉连发
skill(300603)
{
  section(3733)
  {
    movecontrol(true);
    animation("Skill_01");
    summonnpc(10, 101, "Monster_FX/Campaign_Wild/6_Monster_YuJing_01", 300604, vector3(0, 0, 0));
    playsound(1682, "kaipao", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_shoothuopao_01", false);
    charactereffect("Monster_FX/Campaign_Wild/06_BlackSmith/6_Mon_KAC_PaoKou_01", 800, "ef_weapon01", 1712, false);
    startcurvemove(1718, true, 0.1, 0, 0, -3, 0, 0, 0);
    playsound(2215, "kaipao1", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_shoothuopao_01", false);
    charactereffect("Monster_FX/Campaign_Wild/06_BlackSmith/6_Mon_KAC_PaoKou_01", 800, "ef_weapon01", 2150, false);
    startcurvemove(2160, true, 0.1, 0, 0, -1, 0, 0, 0);
    playsound(2685, "kaipao2", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_shoothuopao_01", false);
    charactereffect("Monster_FX/Campaign_Wild/06_BlackSmith/6_Mon_KAC_PaoKou_01", 800, "ef_weapon01", 2717, false);
    startcurvemove(2728, true, 0.1, 0, 0, -3, 0, 0, 0);
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    destroysummonnpc(0);
  };
};

//连发炮弹
skill(300604)
{
  section(3500)
  {
    //movecontrol(true);
    setenable(0, "Visible", true);
    settransform(0," ",vector3(0,0.3,6),eular(0,0,0),"RelativeOwner",false);
    //第一下
    playsound(1712, "baozha", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhixiao_01", false);
    areadamage(1722, 0, 0, 0, 3, true) {
      stateimpact("kDefault", 30060301);
    };
    sceneeffect("Monster_FX/Campaign_Wild/06_BlackSmith/6_Mon_KAC_BaoZha_01", 1500, vector3(0,0,0), 1722);
    //第二下
    playsound(2150, "baozha1", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhixiao_01", false);
    areadamage(2160, 0, 0, 0, 3, true) {
      stateimpact("kDefault", 30060302);
    };
    sceneeffect("Monster_FX/Campaign_Wild/06_BlackSmith/6_Mon_KAC_BaoZha_01", 1500, vector3(0,0,0), 2160);
    //第三下
    playsound(2717, "baozha2", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhixiao_01", false);
    areadamage(2727, 0, 0, 0, 3, true) {
      stateimpact("kDefault", 30060303);
    };
    sceneeffect("Monster_FX/Campaign_Wild/06_BlackSmith/6_Mon_KAC_BaoZha_01", 1500, vector3(0,0,0), 2727);
    destroyself(3500);
    setenable(1800, "Visible", false);
  };
};


//壮汉劈砍
skill(300605)
{
  section(2233)
  {
    animation("Attack_01");
    setanimspeed(1633,"Attack_01",1.5);
    playsound(1625, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuda_01", false);
    playsound(1655, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_zadixiao_01", false);
    charactereffect("Monster_FX/Campaign_Wild/06_BlackSmith/6_Mon_KAW_ZhongJi_01", 2000, "Bone_Root", 1670, false);
    areadamage(1723, 0, 0.5, 2.8, 2.8, true) {
      stateimpact("kDefault", 30060501);
    };
    playsound(1730, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(1750, 300, true, true, vector3(0,0.7,0.3), vector3(0,100,100),vector3(0,5,2),vector3(0,80,80));
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
  };
};

//壮汉横抡
skill(300606)
{
  section(2033)
  {
    movecontrol(true);
    animation("Skill_01");
    findmovetarget(0, vector3(0,0,0),3,60,0.5,0.5,0,-2);
    startcurvemove(1160, true, 0.1, 0, 0, 5, 0, 0, 0);
    charactereffect("Monster_FX/Campaign_Wild/06_BlackSmith/6_Mon_KAW_DaoGuang_01", 2000, "Bone_Root", 1230);
    playsound(1210, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuda_01", false);
    areadamage(1250, 0, 0.5, 2, 2.2, false) {
      stateimpact("kDefault", 30060601);
    };
    playsound(1260, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
//    lockframe(720, "Bluelf03_Attack_01", true, 0, 100, 1, 100);
    shakecamera2(1260, 200, true, true, vector3(0,0,0.1), vector3(0,0,50),vector3(0,0,1),vector3(0,0,50));
  };
};

