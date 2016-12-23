//精英哥布林旋风斩
skill(300101)
{
  section(1333)
  {
    movecontrol(true);
    animation("Bluelf03_SwordS_01") {
      speed(1.5);
    };
    addimpacttoself(0, 30010100);
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Monster_YuJing_Line_01", 1500, vector3(0, 0, 0), 10, eular(0, 0, 0), vector3(1, 1, 1), true);//场景粒子在特殊位置仍然有不同步方向问题
    startcurvemove(266, true, 0.4, 0, 0, -0.07, 0, 0, -0.5);
  };
  section(1500)
  {
    animation("Bluelf03_SwordS_02")
    {
      wrapmode(2);
    };
    startcurvemove(0, true, 1.5, 0, 0, 3.5, 0, 0, 0);
    charactereffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Mon_Bluelf_SwordS_01", 1550, "Bone_Root", 0);
    playsound(10, "huiwu", "Sound/Npc/Mon_Loop", 1500, "Sound/Npc/guaiwu_xuanfengzhan_01", false);

    areadamage(100, 0, 1, 0.5, 2.5, true) {
      stateimpact("kDefault", 30010101);
    };
    playsound(20, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(150, 250, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,30));

    cleardamagestate(650);
    areadamage(700, 0, 1, 0.5, 2.5, true) {
      stateimpact("kDefault", 30010101);
    };
    playsound(710, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(750, 250, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,30));

    cleardamagestate(1250);
    areadamage(1300, 0, 1, 0.5, 2.5, true) {
      stateimpact("kDefault", 30010101);
    };
    playsound(1310, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(1350, 250, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,30));
    stopsound(1495, "huiwu");
  };
  section(1833)
  {
    animation("Bluelf03_SwordS_03");
    startcurvemove(0, true, 0.8, 0, 0, 1.5, 0, 0, -2.5);
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
    stopsound(0, "huiwu");
  };
};

//精英哥布林挥砍
skill(300102)
{
  section(1400)
  {
    movecontrol(true);
    animation("Bluelf03_Attack_01");
    charactereffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Mon_Bluelf_DaoGuang_03", 2000, "Bone_Root", 100);
    startcurvemove(630, true, 0.05, 0, 0, 6, 0, 0, 0);
    playsound(630, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    areadamage(680, 0, 1, 0.5, 3, false) {
      stateimpact("kDefault", 30010201);
    };
    playsound(690, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
//    lockframe(720, "Bluelf03_Attack_01", true, 0, 100, 1, 100);
//    shakecamera(740, true, 40, 20, 100, 0.5);
    shakecamera2(740, 250, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,30));
  };
};


//小斧哥布林劈砍
skill(300103)
{
  section(933)
  {
    movecontrol(true);
    animation("Bluelf01_Attack_01A");
    charactereffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Mon_Bluelf_DaoGuang_02", 2000, "Bone_Root", 520);
  };
  section(267)
  {
    animation("Bluelf01_Attack_01B");
    startcurvemove(10, true, 0.07, 0, 0, 5, 0, 0, 100);
    playsound(10, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuzhong_01", false);
    areadamage(50, 0, 1, 1.5, 2, true) {
      stateimpact("kDefault", 30010301);
    };
    playsound(60, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(90, 250, true, true, vector3(0,0.15,0.2), vector3(0,150,150),vector3(0,0.5,0.5),vector3(0,40,60));
  };
  section(833)
  {
    animation("Bluelf01_Attack_01C");
    startcurvemove(5, true, 0.2, 0, 0, -1, 0, 0, 0);
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
  };
};


//小斧哥布林跳砍
skill(300104)
{
  section(167)
  {
    movecontrol(true);
    animation("Bluelf01_Attack_02A");
    findmovetarget(0, vector3(0,0,0),2.5,60,0.5,0.5,0,-2);
    startcurvemove(30, true, 0.09, 0, 0, 5, 0, 0, 30, 0.04, 0, 0, 3, 0, 0, -60);
    charactereffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Mon_Bluelf_DaoGuang_01", 2000, "Bone_Root", 100) {
      transform(vector3(0,0,-0.5));
    };
  };
  section(800)
  {
    animation("Bluelf01_Attack_02B");
    startcurvemove(130, true, 0.09, 0, 0, 8, 0, 0, 0);
    playsound(275, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuxiao_02", false);
    areadamage(320, 0, 1, 0.8, 2, true) {
      stateimpact("kDefault", 30010401);
    };
    playsound(330, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(370, 250, true, true, vector3(0,0,0.3), vector3(0,0,150),vector3(0,0,0.6),vector3(0,0,80));
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
  };
};


//长矛哥布林
skill(300105)
{
  section(1433)
  {
    movecontrol(true);
    animation("Bluelf02_Attack_01") ;
    startcurvemove(130, true, 0.07, 0, 0, 5, 0, 0, 0);
    setchildvisible(770,"5_Mon_Bluelf_02_w_02",false);
    summonnpc(770, 101, "Monster/Campaign_Wild/01_Bluelf/5_Mon_Bluelf_02_w_02", 390002, vector3(0, 0, 0));
    setchildvisible(1410,"5_Mon_Bluelf_02_w_02",true);
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    setchildvisible(0,"5_Mon_Bluelf_02_w_02",true);
  };
  onstop() //技能正常结束时会运行该段逻辑
  {
    setchildvisible(0,"5_Mon_Bluelf_02_w_02",true);
  };
};


//烧瓶哥布林
skill(300106)
{
  section(3000)
  {
    movecontrol(true);
    sceneeffect("Monster_FX/Campaign_Wild/6_Monster_YuJing_01", 4500, vector3(0,0.2,6.6));
    animation("Bluelf04_Attack_01");
    startcurvemove(2100, true, 0.1, 0, 0, 3, 0, 0, 0);
    setchildvisible(2480,"5_Mon_Bluelf_w_04",false);
    summonnpc(2480, 101, "Monster/Campaign_Wild/01_Bluelf/6_Mon_Bluelf_Bomb_01", 390003, vector3(0, 0, 0));
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


//烧瓶哥布林40M
skill(300107)
{
  section(3000)
  {
    sceneeffect("Monster_FX/Campaign_Wild/6_Monster_YuJing_01", 4500, vector3(0,0.4,40));
    animation("Bluelf04_Attack_01");
    setchildvisible(2480,"5_Mon_Bluelf_w_04",false);
    summonnpc(2480, 101, "Monster/Campaign_Wild/01_Bluelf/6_Mon_Bluelf_Bomb_01", 300108, vector3(0, 0, 0));
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

//投掷燃烧瓶40M
skill(300108)
{
  section(5500)
  {
    movecontrol(true);
    setenable(0, "Visible", true);
    rotate(15, 2500, vector3(-720, 0, 0));
    settransform(0," ",vector3(0,1.1,0),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(10, true, 3,0,6,20,0,-7,0);
    colliderdamage(500, 3000, false, false, 0, 0)
    {
      stateimpact("kDefault", 30010600);
      oncollidelayer("Terrains", "onterrain");
      oncollidelayer("Default", "onterrain");
      boneboxcollider(vector3(0.4, 0.4, 0.4), "Bone", vector3(0, 0, 0), eular(0, 0, 0), true, false);
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
    // colliderdamage(500, 3500, true , true, 1000, 0) {
    //   stateimpact("kDefault", 30010602);
    //   sceneboxcollider(vector3(3,3,3), vector3(0, 0, 0), eular(0, 0, 0), false, false);
    // };
    playsound(400, "ranshao", "Sound/Npc/Mon", 3000, "Sound/Npc/guaiwu_touzhixiao_02", false);
    destroyself(4000);
  };
};


//烧瓶哥布林30M
skill(300109)
{
  section(3000)
  {
    sceneeffect("Monster_FX/Campaign_Wild/6_Monster_YuJing_01", 4500, vector3(0,0.4,30));
    animation("Bluelf04_Attack_01");
    setchildvisible(2480,"5_Mon_Bluelf_w_04",false);
    summonnpc(2480, 101, "Monster/Campaign_Wild/01_Bluelf/6_Mon_Bluelf_Bomb_01", 300110, vector3(0, 0, 0));
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

//投掷燃烧瓶30M
skill(300110)
{
  section(5500)
  {
    movecontrol(true);
    setenable(0, "Visible", true);
    rotate(15, 2500, vector3(-720, 0, 0));
    settransform(0," ",vector3(0,1.1,0),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(10, true, 3, 0, 6, 15, 0, -6.5, 0);
    colliderdamage(500, 3000, false, false, 0, 0)
    {
      stateimpact("kDefault", 30010600);
      oncollidelayer("Terrains", "onterrain");
      oncollidelayer("Default", "onterrain");
      boneboxcollider(vector3(0.4, 0.4, 0.4), "Bone", vector3(0, 0, 0), eular(0, 0, 0), true, false);
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
    destroyself(4000);
  };
};



//烧瓶哥布林20M
skill(300121)
{
  section(3000)
  {
    sceneeffect("Monster_FX/Campaign_Wild/6_Monster_YuJing_01", 4500, vector3(0,0.4,18));
    animation("Bluelf04_Attack_01");
    setchildvisible(2480,"5_Mon_Bluelf_w_04",false);
    summonnpc(2480, 101, "Monster/Campaign_Wild/01_Bluelf/6_Mon_Bluelf_Bomb_01", 300122, vector3(0, 0, 0));
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

//投掷燃烧瓶20M
skill(300122)
{
  section(5500)
  {
    movecontrol(true);
    setenable(0, "Visible", true);
    rotate(15, 2500, vector3(-720, 0, 0));
    settransform(0," ",vector3(0,1.1,0),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(10, true, 3, 0, 5, 9.8, 0, -6, 0);
    colliderdamage(500, 3000, false, false, 0, 0)
    {
      stateimpact("kDefault", 30010600);
      oncollidelayer("Terrains", "onterrain");
      oncollidelayer("Default", "onterrain");
      boneboxcollider(vector3(0.4, 0.4, 0.4), "Bone", vector3(0, 0, 0), eular(0, 0, 0), true, false);
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
    destroyself(4000);
  };
};

//烧瓶哥布林
skill(300120)
{
  section(3000)
  {
    sceneeffect("Monster_FX/Campaign_Wild/6_Monster_YuJing_01", 4500, vector3(0,0.2,6.6));
    animation("Bluelf04_Attack_01");
    setchildvisible(2480,"5_Mon_Bluelf_w_04",false);
    summonnpc(2480, 101, "Monster/Campaign_Wild/01_Bluelf/6_Mon_Bluelf_Bomb_01", 390003, vector3(0, 0, 0));
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