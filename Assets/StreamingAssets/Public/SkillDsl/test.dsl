skill(100)
{
  section(1266)
  {
    addbreaksection(1, 300, 610);
    addbreaksection(10, 300, 610);
    addbreaksection(30, 210, 610);
    movecontrol(true);
    animation("zhankuang_xuanfengzhan_01");
    setanimspeed(10, "zhankuang_xuanfengzhan_01", 0.4, true);
    movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
    startcurvemove(130, true, 0.05, 0, 0, 0, 0, 0, 160, 0.05, 0, 0, 8, 0, 0, 320);
    colliderdamage(10, 4000, false, false, 0, 0) {
      attachenemy("ef_body", eular(0, 180, 0), 701, -1, 702, -1, 702, -1);
      boneboxcollider(vector3(0.12, 0.08, 3), "ef_rightweapon01", vector3(0, 0, 1.5), eular(0, 0, 0), true, true);
    };
  };
  oninterrupt()
  {
  };
  onstop()
  {
  };
};
