//投掷飞锤
skill(390001)
{
  section(1500)
  {
    movecontrol(true);
    playsound(0, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    rotate(30, 1400, vector3(-540, 0, 0));
    settransform(0," ",vector3(0,1,0),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(10, true, 1.5,0,2,12,0,-8,0);
    colliderdamage(10, 1400, false, false, 0, 0)
    {
      stateimpact("kDefault", 30020401);
      oncollidelayer("Terrains", "onterrain");
      oncollidelayer("Default", "onterrain");
      boneboxcollider(vector3(0.8, 0.8, 0.8), "Bone009", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    destroyself(1500);
  };
  onmessage("onterrain")
  {
    setenable(20, "CurveMove", false);
    setenable(20, "Rotate", false);
    setenable(10, "Damage", false);
    sceneeffect("Monster_FX/Campaign_Wild/02_Blof/6_Mon_Blof_ChenTu_02", 1000, vector3(0,0,0));
  };
  onmessage("oncollide")
  {
    playsound(0, "hit", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", false) {
      position(vector3(0,0,0), false);
    };
//    shakecamera2(0, 250, false, false, vector3(0,0,0.2), vector3(0,0,100),vector3(0,0,0.8),vector3(0,0,50));//效果还不错
    destroyself(5);
  };
};


//投掷飞矛
skill(390002)
{
  section(1500)
  {
    movecontrol(true);
    playsound(0, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhida_01", false) {
      position(vector3(0,0,0),true);
    };
    rotate(10, 1500, vector3(25, 0, 0));
    settransform(0," ",vector3(0,1,0),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(10, true, 2,0,1,13,0,-4,0);
    colliderdamage(10, 1400, false, false, 0, 0)
    {
      stateimpact("kDefault", 30010501);
      oncollidelayer("Terrains", "onterrain");
      oncollidelayer("Default", "onterrain");
      boneboxcollider(vector3(0.5, 0.5, 1.5), "Bone010", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    destroyself(1500);
  };
  onmessage("onterrain")
  {
    setenable(1, "CurveMove", false);
    setenable(1, "Rotate", false);
    setenable(1, "Damage", false);
    sceneeffect("Monster_FX/Campaign_Wild/02_Blof/6_Mon_Blof_ChenTu_02", 1000, vector3(0,0.1,0.5));
  };
  onmessage("oncollide")
  {
    playsound(0, "hit", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", false) {
      position(vector3(0,0,0), false);
    };
    destroyself(5);
  };
};


//投掷燃烧瓶
skill(390003)
{
  section(5500)
  {
    movecontrol(true);
    setenable(0, "Visible", true);
    rotate(15, 2500, vector3(-720, 0, 0));
    settransform(0," ",vector3(0,1,0),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(10, true, 3,0,5,4.5,0,-9,0);
    colliderdamage(200, 3000, false, false, 0, 0)
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
    destroyself(4000);
  };
};


//火枪兵子弹
skill(390004)
{
  section(1000)
  {
    movecontrol(true);
    setenable(0, "Visible", true);
    settransform(0,"ef_weapon01",vector3(0,0,0),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(5, true, 0.4,0,0,40,0,0,0);
    rotate(5, 1000, vector3(0, 0, 3600));
    colliderdamage(10, 1400, false, false, 0, 0)
    {
      stateimpact("kDefault", 30030201);
      oncollidelayer("Terrains", "onterrain");
      oncollidelayer("Default", "onterrain");
      boneboxcollider(vector3(0.3, 0.3, 0.3), "Bone", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    destroyself(1000);
  };
  onmessage("onterrain")
  {
    setenable(60, "CurveMove", false);
    setenable(40, "Rotate", false);
    setenable(10, "Damage", false);
    setenable(10, "Visible", false);
    sceneeffect("Monster_FX/Campaign_Wild/02_Blof/6_Mon_Blof_ChenTu_02", 1000, vector3(0,0,0),0,eular(0,0,0),vector3(0.2,0.2,0.2));
    destroyself(5);
  };
  onmessage("oncollide")
  {
    playsound(0, "hit", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", false){
      position(vector3(0,0,0), false);
    };
    shakecamera2(0, 250, false, false, vector3(0,0,0.2), vector3(0,0,100),vector3(0,0,0.8),vector3(0,0,50));//效果还不错
    destroyself(5);
  };
};


//投掷手雷
skill(390005)
{
  section(3000)
  {
    movecontrol(true);
    setenable(0, "Visible", true);
    rotate(10, 2500, vector3(-720, 0, 0));
    settransform(0," ",vector3(0,1.5,0),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(10, true, 3,0,5,4.5,0,-9,0);
    colliderdamage(500, 3000, false, false, 0, 0)
    {
      stateimpact("kDefault", 30030301);
      oncollidelayer("Terrains", "onterrain");
      oncollidelayer("Default", "onterrain");
      boneboxcollider(vector3(0.5, 0.5, 0.5), "Bone", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    destroyself(3000);
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
      stateimpact("kDefault", 30030302);
    };
    sceneeffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDM_BaoZha_01", 2000, vector3(0,0,0));
  };
};

//投掷手雷
skill(390006)
{
  section(3000)
  {
    movecontrol(true);
    setenable(0, "Visible", true);
    rotate(10, 2500, vector3(-720, 0, 0));
    settransform(0," ",vector3(0,1.5,0),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(10, true, 3,0,5,4.5,0,-9,0);
    colliderdamage(10, 3000, false, false, 0, 0)
    {
      stateimpact("kDefault", 30040201);
      oncollidelayer("Terrains", "onterrain");
      oncollidelayer("Default", "onterrain");
      boneboxcollider(vector3(0.8, 0.8, 0.8), "Bone", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    destroyself(3000);
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
      stateimpact("kDefault", 30040202);
    };
    sceneeffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDM_BaoZha_01", 2000, vector3(0,0,0));
  };
};


//投掷骨锤
skill(390007)
{
  section(1500)
  {
    movecontrol(true);
    playsound(0, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_touzhida_01", false){
      position(vector3(0,0,0),true);
    };
    rotate(30, 1400, vector3(-540, 0, 0));
    settransform(0," ",vector3(0,1.5,0),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(10, true, 1.5,0,2,15,0,-8,0);
    colliderdamage(10, 1400, false, false, 0, 0)
    {
      stateimpact("kDefault", 31020401);
      oncollidelayer("Terrains", "onterrain");
      oncollidelayer("Default", "onterrain");
      boneboxcollider(vector3(0.8, 0.8, 0.8), "Bone001", vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    destroyself(1500);
  };
  onmessage("onterrain")
  {
    setenable(20, "CurveMove", false);
    setenable(20, "Rotate", false);
    setenable(10, "Damage", false);
    sceneeffect("Monster_FX/Campaign_Wild/02_Blof/6_Mon_Blof_ChenTu_02", 1000, vector3(0,0,0));
  };
  onmessage("oncollide")
  {
    playsound(0, "hit", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", false){
      position(vector3(0,0,0), false);
    };
//    shakecamera2(0, 250, false, false, vector3(0,0,0.2), vector3(0,0,100),vector3(0,0,0.8),vector3(0,0,50));//效果还不错
    destroyself(5);
  };
};
