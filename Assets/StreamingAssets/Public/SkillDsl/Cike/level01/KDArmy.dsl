//来福枪兵
skill(300301)
{
  section(1000)
  {
    animation("Attack_01A");
  };
  section(1100)
  {
    movecontrol(true);
    animation("Attack_01B");
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_QiangKou_01", 500, "ef_weapon01", 0, false);
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_Bullet_01", 500, "ef_weapon01", 0, false);
    startcurvemove(30, true, 0.05,0,0,-2,0,0,0,0.1,0,0,-0.1,0,0,-10);
/*    areadamage(5,0,1,2,4,true) {
      stateimpact("kDefault",30030101);
      showtip(1000,1,0,0);
    };*/
    colliderdamage(5,100,true,true,150,0) {
    stateimpact("kDefault",30030101);
    boneboxcollider(vector3(2,2,4),"ef_weapon01",vector3(0,0,2),eular(0,0,0),false,false);
    };
  };
  section(2567)
  {
    animation("Attack_01C");
  };
};

//火枪兵
skill(300302)
{
  section(3867)
  {
    movecontrol(true);
    animation("Attack_01");
    charactereffect("Monster_FX/Campaign_Wild/Public/6_Monster_YuJing_RedLine_01", 1200, "ef_weapon01", 1300);
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDM_QiangKou_01", 500, "ef_weapon01", 2500, false);
    summonnpc(2500, 101, "Monster/Campaign_Wild/03_KDArmy/6_Mon_KDM_Bullet_01", 390004, vector3(0, 0, 0));
    startcurvemove(2530, true, 0.05,0,0,-2,0,0,0,0.1,0,0,-0.1,0,0,-10);
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
  };
};


//火枪兵扔雷
skill(300303)
{
  section(1333)
  {
    sceneeffect("Monster_FX/Campaign_Wild/6_Monster_YuJing_01", 1200, vector3(0,0.2,6.7));
    animation("Skill_01");
//    startcurvemove(2100, true, 0.1, 0, 0, 3, 0, 0, 0);
    summonnpc(590, 101, "Monster/Campaign_Wild/03_KDArmy/6_Mon_KDM_Bomb_01", 390005, vector3(0, 0, 0));
  };
};

//来福枪兵连喷
skill(300304)
{
  section(1000)
  {
    animation("Attack_01A");
  };
  section(1100)
  {
    movecontrol(true);
    animation("Attack_01B");
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_QiangKou_01", 500, "ef_weapon01", 0, false);
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_Bullet_01", 500, "ef_weapon01", 0, false);
    startcurvemove(30, true, 0.05,0,0,-2,0,0,0,0.1,0,0,-0.1,0,0,-10);
    colliderdamage(5,100,true,true,150,0) {
      stateimpact("kDefault",30030401);
      boneboxcollider(vector3(2,2,4),"ef_weapon01",vector3(0,0,2),eular(0,0,0),false,false);
    };
  };
  section(1100)
  {
    movecontrol(true);
    animation("Attack_01B");
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_QiangKou_01", 500, "ef_weapon01", 0, false);
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_Bullet_01", 500, "ef_weapon01", 0, false);
    startcurvemove(30, true, 0.05,0,0,-2,0,0,0,0.1,0,0,-0.1,0,0,-10);
    colliderdamage(5,100,true,true,150,0) {
      stateimpact("kDefault",30030401);
      boneboxcollider(vector3(2,2,4),"ef_weapon01",vector3(0,0,2),eular(0,0,0),false,false);
    };
  };
  section(2567)
  {
    animation("Attack_01C");
  };
};



//来福枪兵连喷三下
skill(300305)
{
  section(1000)
  {
    animation("Attack_01A");
  };
  section(1100)
  {
    movecontrol(true);
    animation("Attack_01B");
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_QiangKou_01", 500, "ef_weapon01", 0, false);
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_Bullet_01", 500, "ef_weapon01", 0, false);
    startcurvemove(30, true, 0.05,0,0,-2,0,0,0,0.1,0,0,-0.1,0,0,-10);
    colliderdamage(5,100,true,true,150,0) {
      stateimpact("kDefault",30030501);
      boneboxcollider(vector3(2,2,4),"ef_weapon01",vector3(0,0,2),eular(0,0,0),false,false);
    };
  };
  section(1100)
  {
    movecontrol(true);
    animation("Attack_01B");
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_QiangKou_01", 500, "ef_weapon01", 0, false);
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_Bullet_01", 500, "ef_weapon01", 0, false);
    startcurvemove(30, true, 0.05,0,0,-2,0,0,0,0.1,0,0,-0.1,0,0,-10);
    colliderdamage(5,100,true,true,150,0) {
      stateimpact("kDefault",30030501);
      boneboxcollider(vector3(2,2,4),"ef_weapon01",vector3(0,0,2),eular(0,0,0),false,false);
    };
  };
  section(1100)
  {
    movecontrol(true);
    animation("Attack_01B");
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_QiangKou_01", 500, "ef_weapon01", 0, false);
    charactereffect("Monster_FX/Campaign_Wild/03_KDArmy/6_Mon_KDS_Bullet_01", 500, "ef_weapon01", 0, false);
    startcurvemove(30, true, 0.05,0,0,-2,0,0,0,0.1,0,0,-0.1,0,0,-10);
    colliderdamage(5,100,true,true,150,0) {
      stateimpact("kDefault",30030501);
      boneboxcollider(vector3(2,2,4),"ef_weapon01",vector3(0,0,2),eular(0,0,0),false,false);
    };
  };
  section(2567)
  {
    animation("Attack_01C");
  };
};