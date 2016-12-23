skill(120000)
{
	section(600)
	{
    movechild(500, "3_Cike_w_01", "ef_other01");//初始化主武器
    movechild(500, "3_Cike_w_02", "ef_other02");//初始化主武器
		addbreaksection(1, 0, 500);
		addbreaksection(10, 0, 500);
		addbreaksection(11, 0, 500);
		animation("Cike_Combat2Idle");
		//movechild(866, "1_JianShi_w_01", "ef_backweapon01");
  };
  oninterrupt()
  {
		//movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
  };
};