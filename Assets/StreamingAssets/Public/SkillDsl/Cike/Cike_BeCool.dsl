skill(120005)
{
	section(100000)
	{
    movechild(0, "3_Cike_w_01", "ef_righthand");//初始化主武器
    movechild(0, "3_Cike_w_02", "ef_lefthand");//初始化主武器

		animation("Cike_Show");
		movecontrol(true);

		storepos(0);
		settransform(1, "", vector3(0, 0, 0), eular(0, 180, 0), "RelativeWorld", false);
		restorepos(2);
		movecamera(0, true, 0.2, 0, -50, 25, 0, 0, 0);
		movecamera(200, false,  0.1, 10, 0, 0, 0, 0, 0, 499.3, 0, 0, 0, 0, 0, 0, 0.5, 0, 12, 8, 0, 0, 0);

    movechild(1000, "3_Cike_w_01", "ef_other01");//初始化主武器
    movechild(1000, "3_Cike_w_02", "ef_other02");//初始化主武器
  };
  oninterrupt()
  {
  };
};