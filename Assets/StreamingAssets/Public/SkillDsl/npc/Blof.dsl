//爪子狼人跳跃攻击
skill(300201)
{
  section(1882)
  {
    movecontrol(true);
    animation("Skill_01");
    setanimspeed(180, "Skill_01", 2);
    setanimspeed(630, "Skill_01", 1);
    findmovetarget(120, vector3(0,0,0),6,60,0.5,0.5,0,-1);
    startcurvemove(180, true, 0.15, 0, 0, 20, 0, 0, 0, 0.3, 0, 0, 20, 0, 0, -60);
    sceneeffect("Monster_FX/Campaign_Wild/02_Blof/6_Mon_Blof_ChenTu_01", 1000, vector3(0,0,0), 650);
    charactereffect("Monster_FX/Campaign_Wild/02_Blof/6_Mon_Blof_ZhuaJi_02", 1000, "Bone_Root", 520);
    playsound(495, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuxiao_03", false);
    areadamage(535, 0, 1, 0.5, 4.5, true) {
      stateimpact("kDefault", 30020101);
    };
    playsound(540, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(570, 250, true, true, vector3(0,0.6,0.15), vector3(0,100,200),vector3(0,0.6,0.15),vector3(0,80,50));
    cleardamagestate(1000);
    startcurvemove(1070, true, 0.1, 0, 0, 10, 0, 0, 0);
    playsound(1160, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuxiao_03", false);
    charactereffect("Monster_FX/Campaign_Wild/02_Blof/6_Mon_Blof_ZhuaJi_03", 1000, "Bone_Root", 1080);
    areadamage(1103, 0, 1, 0.5, 4.5, true) {
      stateimpact("kDefault", 30020102);
    };
    playsound(1110, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(1140, 250, true, true, vector3(0,0,0.12), vector3(0,0,100),vector3(0,0,0.3),vector3(0,0,30));
  };
};

//爪子狼人普攻
skill(300202)
{
  section(2083)
  {
    movecontrol(true);
    animation("Attack_01");
    setanimspeed(1500, "Attack_01", 2);
    findmovetarget(1630, vector3(0,0,0),3,60,0.5,0.5,0,-1);
    startcurvemove(1680, true, 0.12, 0, 0, 15, 0, 0, 0);
    playsound(1665, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuxiao_03", false);
    charactereffect("Monster_FX/Campaign_Wild/02_Blof/6_Mon_Blof_ZhuaJi_01", 1000, "Bone_Root", 1690);
    areadamage(1710, 0, 1, 0.5, 4, true) {
      stateimpact("kDefault", 30020201);
    };
    playsound(1720, "hit", "", 1000, "Sound/Npc/guaiwu_jizhong_tongyong_01", true);
    shakecamera2(1750, 250, true, true, vector3(0,0,0.2), vector3(0,0,150),vector3(0,0,0.5),vector3(0,0,30));
  };
};


//锤子狼人普攻
skill(300203)
{
  section(1900)
  {
    movecontrol(true);
    animation("Attack_01") {
      speed(1.5);
    };
    setanimspeed(1300, "Attack_01", 1);
    charactereffect("Monster_FX/Campaign_Wild/01_Bluelf/6_Mon_Bluelf_DaoGuang_01", 2000, "Bone_Root", 785);
    findmovetarget(1170, vector3(0,0,0),2,60,0.5,0.5,0,-1);
    startcurvemove(1220, true, 0.15, 0, 0, 1.5, 0, 0, 0);
    //sound("Sound/Swordman/TiaoKong", 1150);//无定帧时的音效开始时间
    playsound(1215, "huiwu", "Sound/Npc/Mon", 1000, "Sound/Npc/guaiwu_huiwuda_01", false);
    areadamage(1255, 0, 1, 0.5, 3, true) {
      stateimpact("kDefault", 30020301);
    };
    shakecamera2(1290, 250, true, true, vector3(0,0,0.2), vector3(0,0,100),vector3(0,0,0.8),vector3(0,0,50));//效果还不错
  };
};

//锤子狼人投掷
skill(300204)
{
  section(1500)
  {
    movecontrol(true);
    animation("Skill_01") ;
//    sceneeffect("Monster_FX/Demo_01/01_Bluelf/6_Monster_AttackSign_01", 1000, vector3(0,0,12));
//    charactereffect("Monster_FX/Demo_01/01_Bluelf/6_Mon_Bluelf_SwordS_01", 4000, "ef_body", 10);
    setchildvisible(840,"5_Mon_Blof_02_w_01",false);
    summonnpc(840, 101, "Monster/Campaign_Wild/02_Blof/5_Mon_Blof_02_w_01", 390001, vector3(0, 0, 0));
    setchildvisible(1450,"5_Mon_Blof_02_w_01",true);
  };
  oninterrupt() //技能在被打断时会运行该段逻辑
  {
    setchildvisible(1450,"5_Mon_Blof_02_w_01",true);
  };
  onstop() //技能正常结束时会运行该段逻辑
  {
    setchildvisible(1450,"5_Mon_Blof_02_w_01",true);
  };
};
