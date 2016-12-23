//捶地连击
skill(300111)
{
  section(4833)
  {
    movecontrol(true);
    animation("Skill_01");
    charactereffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Boss_Goblin_DaoGuang_01", 160, "Bone_Root", 1010);
    charactereffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Boss_Goblin_DaoGuang_01", 160, "Bone_Root", 2490);
    ////sound("Sound/Swordman/TiaoKong", 620);//无定帧时的音效开始时间
    findmovetarget(667, vector3(0,0,0),5,50,0.5,0.5,0,-3);
    startcurvemove(670, true, 0.33, 0, 0, 3, 0, 0, 0);
    startcurvemove(1825, true, 0.15, 0, 0, 1.5, 0, 0, 0);
    startcurvemove(2280, true, 0.15, 0, 0, 8, 0, 0, 0);
    playsound(990, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuda_01", false);
    playsound(1005, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_zadixiao_01", false);
    areadamage(1030, 0, 1, 2, 3, true) {
      stateimpact("kDefault", 30011101);
    };
    playsound(1040, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(1035, 400, true, true, vector3(0.12,0.5,0.12), vector3(50,100,50),vector3(0.2,1,0.2),vector3(60,50,60));
    cleardamagestate(1500);
    playsound(2460, "huiwu2", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuda_01", false);
    playsound(2475, "huiwu3", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_zadixiao_01", false);
    areadamage(2500, 0, 1, 2, 3, true) {
      stateimpact("kDefault", 30011102);
    };
    playsound(2510, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(2505, 600, true, true, vector3(0.15,0.7,0.15), vector3(50,100,50),vector3(0.2,1.4,0.2),vector3(70,60,70));
  };
};

//普攻砸地
skill(300112)
{
  section(3433)
  {
    movecontrol(true);
    animation("Skill_03");
    charactereffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Boss_Goblin_DaoGuang_01", 160, "Bone_Root", 955);
    //findmovetarget(1425, vector3(0,0,0),3.5,50,0.5,0.5,0,-2);
    //startcurvemove(1612, true, 0.1, 0, 0, 3, 0, 0, 0);
    playsound(930, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuda_01", false);
    playsound(945, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_zadixiao_01", false);
    areadamage(970, 0, 1, 2, 3, false) {
      stateimpact("kDefault", 30011201);
    };
    playsound(980, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    //lockframe(720, "Bluelf03_Attack_01", true, 0, 100, 1, 100);
    shakecamera2(985, 200, false, true, vector3(0.12,0.4,0.12), vector3(40,80,40),vector3(0.15,0.8,0.15),vector3(60,50,60));
  };
};

//震荡波
skill(300113)
{
  section(3633)
  {
    movecontrol(true);
    animation("Skill_03");
    setanimspeed(500, "Skill_03",0.667);
    setanimspeed(1100, "Skill_03",1);
    charactereffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Boss_Goblin_XiaPi_01", 1000, "Bone_Root", 1140);
    playsound(1120, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuda_01", false);
    summonnpc(1140, 101, "Monster_FX/Campaign_Wild/01_Bluelf/6_Mon_Bluelf_RanShao_01", 300116, vector3(0, 0, 1));
    //charactereffect("Monster_FX/Campaign_Dungeon/02_Skeleton/6_Mon_Skeleton_DaoGuang_01_02", 2000, "Bone_Root", 1640);
    //sound("Sound/Swordman/TiaoKong", 620);//无定帧时的音效开始时间
    //startcurvemove(1612, true, 0.1, 0, 0, 3, 0, 0, 0);
    //shakecamera2(1180, 200, false, true, vector3(0.12,0.4,0.12), vector3(40,80,40),vector3(0.15,0.8,0.15),vector3(60,50,60));
  };
};

//蓄力攻击
skill(300114)
{
  section(6333)
  {
    movecontrol(true);
    animation("Skill_02");
    addimpacttoself(0, 30011402);
    //shakecamera2(667, 1200, false, false, vector3(0.05,0.05,0.05), vector3(50,50,50),vector3(1,1,1),vector3(100,100,100));
    //shakecamera2(1867, 1300, false, false, vector3(0.1,0.1,0.1), vector3(50,50,50),vector3(2,2,2),vector3(100,100,100));
    charactereffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Boss_Goblin_Xuli_01", 3000, "Bone_Root", 667) {
      transform(vector3(0,0.5,0));
    };
    charactereffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Boss_Goblin_Xuli_03", 500, "ef_weapon01", 1000, false);
    charactereffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Boss_Goblin_Xuli_03", 500, "ef_weapon01", 3000, false);
    charactereffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Boss_Goblin_Xuli_02", 2300, "ef_weapon01", 1000);
    charactereffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Boss_Goblin_XiaPi_01", 1000, "Bone_Root", 3220);
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Boss_Goblin_ZaDi_02", 1200, vector3(0,0,3), 3220, eular(0,0,0), vector3(2,2,2));
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Boss_Goblin_YuJing_01", 3000, vector3(0,0.5,3), 200, eular(0,0,0), vector3(1.5,1.5,1.5));
    //sound("Sound/Swordman/TiaoKong", 620);//无定帧时的音效开始时间
    //findmovetarget(1425, vector3(0,0,0),3.5,50,0.5,0.5,0,-2);
    //startcurvemove(1612, true, 0.1, 0, 0, 3, 0, 0, 0);
    playsound(3170, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuda_01", false);
    playsound(3210, "huiwu1", "Sound/Npc/Mon", 2000, "Sound/Npc/guaiwu_zadida_01", false);
    areadamage(3230, 0, 1, 3, 4.5, false) {
      stateimpact("kDefault", 30011401);
    };
    playsound(3240, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
//    lockframe(720, "Bluelf03_Attack_01", true, 0, 100, 1, 100);
    shakecamera2(3240, 600, false, false, vector3(0.1,2,0.1), vector3(50,100,50),vector3(0.2,5,0.2),vector3(90,45,90));
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    stopeffect(0);
  };
};

//二连击
skill(300115)
{
  section(4600)
  {
    movecontrol(true);
    animation("Skill_04");
    // charactereffect("Monster_FX/Campaign_Dungeon/02_Skeleton/6_Mon_Skeleton_DaoGuang_01_01", 2000, "Bone_Root", 1315);
//    sound("Sound/Swordman/TiaoKong", 620);//无定帧时的音效开始时间
    findmovetarget(1425, vector3(0,0,0),4,50,0.5,0.5,0,-2);
    startcurvemove(855, true, 0.5, 0, 0, 2, 0, 0, 0);
    startcurvemove(1510, true, 0.5, 0, 0, 1, 0, 0, 0);
    findmovetarget(2245, vector3(0,0,0),4,50,0.5,0.5,0,-2);
    startcurvemove(2258, true, 0.4, 0, 0, 5, 0, 0, 0);
    startcurvemove(3840, true, 0.5, 0, 0, 0.8, 0, 0, 0);
    playsound(1290, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuda_01", false);
    areadamage(1325, 0, 1, 2, 3, true) {
      stateimpact("kDefault", 30011501);
    };
    playsound(1330, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    cleardamagestate(1500);
    playsound(2625, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuda_01", false);
    areadamage(2660, 0, 1, 2, 3, true) {
      stateimpact("kDefault", 30011502);
    };
    playsound(2665, "hit1", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    charactereffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Boss_Goblin_DaoGuang_02", 160, "Bone_Root", 1300);
    charactereffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Boss_Goblin_DaoGuang_02", 160, "Bone_Root", 2560);
    shakecamera2(1330, 150, true, true, vector3(0,0,0.3), vector3(0,0,50),vector3(0,0,1),vector3(0,0,80));
    shakecamera2(2667, 180, true, true, vector3(0,0,0.4), vector3(0,0,50),vector3(0,0,1.2),vector3(0,0,80));
  };
};

//震荡波召唤物
skill(300116)
{
  section(600)
  {
    movecontrol(true);
    settransform(0," ",vector3(0,0,1.5),eular(0,0,0),"RelativeOwner",false);
    startcurvemove(10, true, 0.6,0,0,20,0,0,0);
    colliderdamage(40, 600, false, false, 0, 0)
    {
      stateimpact("kDefault", 30011301);
      sceneboxcollider(vector3(3, 3, 3), vector3(0, 0, 0), eular(0, 0, 0), true, false);
    };
    destroyself(600);
    playsound(30, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_zadixiao_01", false){
      position(vector3(0,0,0), false);
    };;
    playsound(200, "huiwu1", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_zadixiao_01", false){
      position(vector3(0,0,0), false);
    };;
    playsound(370, "huiwu2", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_zadixiao_01", false){
      position(vector3(0,0,0), false);
    };;
    playsound(540, "huiwu3", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_zadixiao_01", false){
      position(vector3(0,0,0), false);
    };;
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Boss_Goblin_ZhenDangBo_01", 1500, vector3(0,0,0),40);
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Boss_Goblin_ZhenDangBo_01", 1500, vector3(0,0,0),210);
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Boss_Goblin_ZhenDangBo_01", 1500, vector3(0,0,0),380);
    sceneeffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Boss_Goblin_ZhenDangBo_01", 1500, vector3(0,0,0),550);
    shakecamera2(40, 100, false, true, vector3(0,0.4,0), vector3(0,50,0),vector3(0,1.6,0),vector3(0,50,0));
    shakecamera2(210, 100, false, true, vector3(0,0.4,0), vector3(0,50,0),vector3(0,1.6,0),vector3(0,50,0));
    shakecamera2(380, 100, false, true, vector3(0,0.4,0), vector3(0,50,0),vector3(0,1.6,0),vector3(0,50,0));
    shakecamera2(550, 100, false, true, vector3(0,0.4,0), vector3(0,50,0),vector3(0,1.6,0),vector3(0,50,0));
//    shakecamera2(0, 250, false, false, vector3(0,0,0.2), vector3(0,0,100),vector3(0,0,0.8),vector3(0,0,50));//效果还不错
  };
};
