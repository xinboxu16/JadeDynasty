skill(160005)
{
	section(500000)
	{
		addbreaksection(1, 500000, 500000);
		addbreaksection(10, 500000, 500000);
		addbreaksection(30, 500000, 500000);
		movecontrol(true);
		animation("zhankuang_shuashuai_01");
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");

		storepos(0);

		settransform(1, "", vector3(0, 0, 0), eular(0, 100, 0), "RelativeWorld", false);

		restorepos(2);

		movecamera(0, true, 0.2, 0, -50, 25, 0, 0, 0);

		movecamera(200, false,  0.1, 10, 0, 0, 0, 0, 0, 499.3, 0, 0, 0, 0, 0, 0, 0.5, 0, 12, 8, 0, 0, 0);
		
		//rotatecamera(0, 200, vector3(-165, 0, 0));

		//rotatecamera(200, 499800, vector3(0, 0, 0));
  };
  oninterrupt()
  {
  };
};