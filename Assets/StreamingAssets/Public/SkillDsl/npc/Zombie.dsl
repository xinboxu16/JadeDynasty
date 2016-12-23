//僵尸赤爪击1
skill(310101)
{
  section(2333)
  {
    movecontrol(true);
    animation("Attack_01");
    charactereffect("Monster_FX/Campaign_Dungeon/01_Zombie/6_Mon_Zombie_Zhuaji_01", 2000, "ef_righthand", 200);
//    sound("Sound/Swordman/TiaoKong", 620);//无定帧时的音效开始时间
    findmovetarget(1425, vector3(0,0,0),2,50,0.5,0.5,0,-1.5);
    startcurvemove(1445, true, 0.05, 0, 0, 2, 0, 0, 0);
    playsound(1470, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuxiao_02", false);
    areadamage(1500, 0, 1, 0.5, 1.2, false) {
      stateimpact("kDefault", 31010101);
    };
    playsound(1510, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
//    lockframe(720, "Bluelf03_Attack_01", true, 0, 100, 1, 100);
    shakecamera2(1520, 250, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,30));
  };
};

//僵尸赤爪击2
skill(310102)
{
  section(2666)
  {
    movecontrol(true);
    animation("Skill_01");
    charactereffect("Monster_FX/Campaign_Dungeon/01_Zombie/6_Mon_Zombie_Zhuaji_01", 2000, "ef_righthand", 300);
    charactereffect("Monster_FX/Campaign_Dungeon/01_Zombie/6_Mon_Zombie_Zhuaji_01", 2000, "ef_lefthand", 300);
//    sound("Sound/Swordman/TiaoKong", 620);//无定帧时的音效开始时间
    findmovetarget(1800, vector3(0,0,0),2,50,0.5,0.5,0,-1.5);
    startcurvemove(1825, true, 0.05, 0, 0, 2, 0, 0, 0);
    playsound(1875, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuxiao_02", false);
    playsound(1885, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuxiao_03", false);
    areadamage(1916, 0, 1, 0.5, 1.2, false) {
      stateimpact("kDefault", 31010201);
    };
    playsound(1924, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
//    lockframe(720, "Bluelf03_Attack_01", true, 0, 100, 1, 100);
    shakecamera2(1930, 250, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,30));
  };
};


//僵尸青爪击1
skill(310103)
{
  section(2233)
  {
    movecontrol(true);
    animation("Attack_01");
    charactereffect("Monster_FX/Campaign_Dungeon/01_Zombie/6_Mon_Zombie_Zhuaji_02", 2000, "ef_righthand", 100);
//    sound("Sound/Swordman/TiaoKong", 620);//无定帧时的音效开始时间
    findmovetarget(1666, vector3(0,0,0),2,50,0.5,0.5,0,-1.5);
    startcurvemove(1680, true, 0.05, 0, 0, 2, 0, 0, 0);
    playsound(1740, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuxiao_02", false);
    areadamage(1766, 0, 1, 0.5, 1.2, false) {
      stateimpact("kDefault", 31010301);
    };
    playsound(1774, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
//    lockframe(720, "Bluelf03_Attack_01", true, 0, 100, 1, 100);
    shakecamera2(1780, 250, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,30));
  };
};

//僵尸青爪击2
skill(310104)
{
  section(3100)
  {
    movecontrol(true);
    animation("Skill_01");
    charactereffect("Monster_FX/Campaign_Dungeon/01_Zombie/6_Mon_Zombie_Zhuaji_02", 2000, "ef_righthand", 200);
    charactereffect("Monster_FX/Campaign_Dungeon/01_Zombie/6_Mon_Zombie_Zhuaji_02", 2000, "ef_lefthand", 900);
//    sound("Sound/Swordman/TiaoKong", 620);//无定帧时的音效开始时间
    findmovetarget(1700, vector3(0,0,0),2,50,0.5,0.5,0,-1.5);
    startcurvemove(1725, true, 0.05, 0, 0, 2, 0, 0, 0);
    playsound(1765, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuxiao_02", false);
    areadamage(1785, 0, 1, 0.5, 1.4, false) {
      stateimpact("kDefault", 31010401);
    };
    playsound(1790, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    playsound(2430, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuxiao_02", false);
    cleardamagestate(2400);//清除当前产生过伤害的状态，重置为未造成过伤害
    areadamage(2455, 0, 1, 0.5, 1.4, false) {
      stateimpact("kDefault", 31010402);
    };
    playsound(2460, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
//    lockframe(720, "Bluelf03_Attack_01", true, 0, 100, 1, 100);
    shakecamera2(1800, 250, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,30));
    cleardamagestate(2400);
    shakecamera2(2470, 250, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,30));
  };
};


//僵尸女啃咬
skill(310105)
{
  section(2533)
  {
    movecontrol(true);
    animation("Attack_01");
    charactereffect("Monster_FX/Campaign_Dungeon/01_Zombie/6_Mon_Zombie_Zhuaji_01", 2000, "Bip001 Head", 100);
//    sound("Sound/Swordman/TiaoKong", 620);//无定帧时的音效开始时间
    findmovetarget(1825, vector3(0,0,0),1.5,50,0.5,0.5,0,-1);
    startcurvemove(1830, true, 0.05, 0, 0, 2, 0, 0, 0);
    areadamage(1786, 0, 1, 0.5, 1, false) {
      stateimpact("kDefault", 31010501);
    };
    playsound(1790, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
//    lockframe(720, "Bluelf03_Attack_01", true, 0, 100, 1, 100);
    shakecamera2(1800, 250, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,30));
  };
};

//僵尸女爪击2
skill(310106)
{
  section(2666)
  {
    movecontrol(true);
    animation("Skill_01");
    charactereffect("Monster_FX/Campaign_Dungeon/01_Zombie/6_Mon_Zombie_Zhuaji_01", 2000, "ef_righthand", 100);
    charactereffect("Monster_FX/Campaign_Dungeon/01_Zombie/6_Mon_Zombie_Zhuaji_01", 2000, "ef_lefthand", 100);
//    sound("Sound/Swordman/TiaoKong", 620);//无定帧时的音效开始时间
    findmovetarget(1825, vector3(0,0,0),1.8,50,0.5,0.5,0,-1.2);
    startcurvemove(1835, true, 0.05, 0, 0, 2, 0, 0, 0);
    playsound(1855, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuxiao_02", false);
    playsound(1870, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuxiao_03", false);
    areadamage(1882, 0, 1, 0.5, 1.2, false) {
      stateimpact("kDefault", 31010601);
    };
    playsound(1890, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
//    lockframe(720, "Bluelf03_Attack_01", true, 0, 100, 1, 100);
    shakecamera2(1900, 250, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,30));
  };
};

