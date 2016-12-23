//火枪兵
skill(370301)
{
  section(1933)
  {
    movecontrol(true);
    animation("Attack_01");
    setanimspeed(1, "Attack_01", 2, false);
    playsound(650, "yujing", "Sound/Npc/Mon", 1230, "Sound/Npc/guaiwu_shoothongwaixian_01", false);
    charactereffect("Monster_FX/Campaign_Wild/Public/6_Monster_YuJing_RedLine_01", 600, "ef_weapon01", 650);
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDM_QiangKou_01", 500, "ef_weapon01", 1250, false);
    playsound(1240, "kaipao", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_shoothuoqiang_01", false);
    summonnpc(1250, 101, "Monster/Campaign_Wild/03_KDArmy/6_Mon_KDM_Bullet_01", 390004, vector3(0, 0, 0));
    startcurvemove(1270, true, 0.05,0,0,-2,0,0,0,0.1,0,0,-0.1,0,0,-10);
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
  };
};


//来福枪兵
skill(370302)
{
  section(500)
  {
    animation("Attack_01A");
    setanimspeed(1,"Attack_01A",2,false);
  };
  section(550)
  {
    movecontrol(true);
    animation("Attack_01B");
    setanimspeed(1,"Attack_01B",2,false);
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_QiangKou_01", 500, "ef_weapon01", 0, false);
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_Bullet_01", 500, "ef_weapon01", 0, false);
    startcurvemove(20, true, 0.05,0,0,-2,0,0,0,0.1,0,0,-0.1,0,0,-10);
//音效
    playsound(0, "kaipao", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_shootsandan_01", false);
    colliderdamage(5,50,true,true,150,0) {
    stateimpact("kDefault",30030101);
    boneboxcollider(vector3(2,2,4),"ef_weapon01",vector3(0,0,2),eular(0,0,0),false,false);
    };
  };
  section(2233)
  {
    animation("Attack_01C");
  };
};


//来福枪兵连喷
skill(370303)
{
  section(500)
  {
    animation("Attack_01A");
    setanimspeed(1,"Attack_01A",2,false);
  };
  section(550)
  {
    movecontrol(true);
    animation("Attack_01B");
    setanimspeed(1,"Attack_01B",2,false);
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_QiangKou_01", 500, "ef_weapon01", 0, false);
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_Bullet_01", 500, "ef_weapon01", 0, false);
    startcurvemove(20, true, 0.05,0,0,-2,0,0,0,0.1,0,0,-0.1,0,0,-10);
//音效
    playsound(0, "kaipao", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_shootsandan_01", false);
    colliderdamage(5,50,true,true,150,0) {
    stateimpact("kDefault",30030101);
    boneboxcollider(vector3(2,2,4),"ef_weapon01",vector3(0,0,2),eular(0,0,0),false,false);
    };
  };
  section(550)
  {
    movecontrol(true);
    animation("Attack_01B");
    setanimspeed(1,"Attack_01B",2,false);
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_QiangKou_01", 500, "ef_weapon01", 0, false);
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_Bullet_01", 500, "ef_weapon01", 0, false);
    startcurvemove(20, true, 0.05,0,0,-2,0,0,0,0.1,0,0,-0.1,0,0,-10);
//音效
    playsound(0, "kaipao", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_shootsandan_01", false);
    colliderdamage(5,50,true,true,150,0) {
    stateimpact("kDefault",30030101);
    boneboxcollider(vector3(2,2,4),"ef_weapon01",vector3(0,0,2),eular(0,0,0),false,false);
    };
  };
  section(2233)
  {
    animation("Attack_01C");
  };
};