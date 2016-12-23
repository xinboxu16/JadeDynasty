//镭射枪兵
skill(300401)
{
  section(3000)
  {
    animation("Attack_01A");
    playsound(1000, "yujing", "Sound/Npc/Mon", 2000, "Sound/Npc/guaiwu_shoothongwaixian_01", false);
    charactereffect("Monster_FX/Campaign_Wild/Public/6_Monster_YuJing_RedLine_01", 2000, "ef_weapon01", 1000);//预警
    stopsound(2990,"yujing");
  };
  section(2867)
  {
    animation("Attack_01B")
    {
      wrapmode(2);
    };
    playsound(0, "kaipao", "Sound/Npc/Mon", 3000, "Sound/Npc/guaiwu_shootjiguang_01", false);
    charactereffect("Monster_FX/Campaign_Wild/04_SilverShield/6_Mon_SSLas_QiangKou_01", 2900, "ef_weapon01", 0);//枪口
    charactereffect("Monster_FX/Campaign_Wild/04_SilverShield/6_Mon_SSLas_Laser_01", 2867, "ef_weapon01", 0);//光束
    colliderdamage(0, 2867, false, false, 400, 0)
    {
      stateimpact("kDefault", 30040101);
      boneboxcollider(vector3(0.3, 0.3, 20), "ef_weapon01", vector3(0, 0, 10.2), eular(0, 0, 0), true, false);
    };
  };
  section(467)
  {
    animation("Attack_01C");
  };
    oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
    setenable(0, "Damage", false);
    stopsound(0,"yujing");
    stopsound(0,"kaipao");
  };
};


//镭射枪兵扔雷
skill(300402)
{
  section(1333)
  {
    movecontrol(true);
    sceneeffect("Monster_FX/Campaign_Wild/6_Monster_YuJing_01", 1200, vector3(0,0.2,6.7));
    animation("Skill_01");
//    startcurvemove(2100, true, 0.1, 0, 0, 3, 0, 0, 0);
    summonnpc(590, 101, "Monster/Campaign_Wild/03_KDArmy/6_Mon_KDM_Bomb_01", 390006, vector3(0, 0, 0));
  };
};
