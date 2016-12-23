skill(160000)
{
	section(1233)
	{
		addbreaksection(1, 0, 1233);
		addbreaksection(10, 0, 1233);
		addbreaksection(30, 0, 1233);
		animation("zhankuang_change_01");
		movechild(866, "1_JianShi_w_01", "ef_backweapon01");
  };
  oninterrupt()
  {
		movechild(0, "1_JianShi_w_01", "ef_rightweapon01");
  };
};