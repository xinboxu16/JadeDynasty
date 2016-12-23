story(1)
{ 
	onmessage("start")
	{
		wait(100);
		//createnpc(10001);
	  //wait(10);
	  sendgfxmessage("GfxGameRoot","EnableBloom");
		showui(false);
		sendgfxmessage("Main Camera","DimScreen",10);
		wait(10);
		sendgfxmessage("Main Camera","LightScreen",3000);
		wait(1000);
	  showdlg(101301);
	};
	onmessage("dialogover",101301)
	{
		wait(10);
		sendgfxmessage("GfxGameRoot","DisableBloom");
		showui(true);
		wait(50);
	  loop(4)
	  {
	    createnpc(1001+$$);
	  };
	  wait(500);
	  npcpatrol(10001,vector2list("47.20232 9.301882"),isnotloop);
	  wait(1000);
	  setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
	};
	onmessage("npckilled",1001)
	{
	  npcpatrol(10001,vector2list("47.20232 9.301882"),isnotloop);
	};
	onmessage("npckilled",1002)
	{
	  npcpatrol(10001,vector2list("47.20232 9.301882"),isnotloop);
	};
	onmessage("npckilled",1003)
	{
	  npcpatrol(10001,vector2list("47.20232 9.301882"),isnotloop);
	};
	onmessage("npckilled",1004)
	{
	  npcpatrol(10001,vector2list("47.20232 9.301882"),isnotloop);
	};
	onmessage("allnpckilled")
	{
		wait(100);
		npcpatrol(10001,vector2list("51.17324 28.24513"),isnotloop);
		wait(2000);
		restartareamonitor(2);
		wait(100);
		showwall("AtoB",false);
	};
	onmessage("anyuserenterarea",2)
	{
		//showwall("BDoor",true);
		startstory(2);
		terminate();	  
	};
  onmessage("missionfailed")
  {
    missionfailed(1010);
    terminate();
  };
};
story(2)
{
	onmessage("start")
	{
		wait(10);
	  loop(10)
	  {
	    createnpc(2001+$$);
	  };
	  wait(1000);
	  setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
	};
	onmessage("npckilled",2001)
	{
	  npcpatrol(10001,vector2list("47.03958 38.17242"),isnotloop);
	};
	onmessage("npckilled",2002)
	{
	  npcpatrol(10001,vector2list("47.03958 38.17242"),isnotloop);
	};
	onmessage("npckilled",2005)
	{
	  npcpatrol(10001,vector2list("38.67462 44.81231"),isnotloop);
	};
	onmessage("npckilled",2006)
	{
	  npcpatrol(10001,vector2list("38.67462 44.81231"),isnotloop);
	};
	onmessage("npckilled",2009)
	{
	  npcpatrol(10001,vector2list("38.67462 44.81231"),isnotloop);
	};
	onmessage("npckilled",2003)
	{
	  npcpatrol(10001,vector2list("42.77573 55.78115"),isnotloop);
	};
	onmessage("npckilled",2004)
	{
	  npcpatrol(10001,vector2list("42.77573 55.78115"),isnotloop);
	};
	onmessage("npckilled",2007)
	{
	  npcpatrol(10001,vector2list("42.77573 55.78115"),isnotloop);
	};
	onmessage("npckilled",2008)
	{
	  npcpatrol(10001,vector2list("42.77573 55.78115"),isnotloop);
	};
	onmessage("npckilled",2010)
	{
	  npcpatrol(10001,vector2list("42.77573 55.78115"),isnotloop);
	};
	onmessage("allnpckilled")
	{
		wait(100);
		npcpatrol(10001,vector2list("47.16977 70.23268"),isnotloop);
		wait(2500);
		restartareamonitor(3);
		wait(100);
		showwall("BtoC",false);
	};
	onmessage("anyuserenterarea",3)
	{
		wait(500);
		npcpatrol(10001,vector2list("55.46964 84.35872"),isnotloop);
		//showwall("CDoor",true);
		startstory(3);
		terminate();
	};
  onmessage("missionfailed")
  {
    missionfailed(1010);
    terminate();
  };
};
story(3)
{
  local
  {
    @reconnectCount(0);
    @rnd(0);
  };
	onmessage("start")
	{	
		wait(100);
		@rnd=rndfloat();
	  loop(9)
	  {
  	  createnpc(3001+$$,@rnd);
  	};  	
  	wait(1000);
	  setblockedshader(0x0000ff90,0.5,0,0xff000090,0.5,0);
	};
	onmessage("npckilled",3002)
	{
	  npcpatrol(10001,vector2list("55.46964 84.35872"),isnotloop);
	};
	onmessage("npckilled",3003)
	{
	  npcpatrol(10001,vector2list("55.46964 84.35872"),isnotloop);
	};
	onmessage("npckilled",3004)
	{
	  npcpatrol(10001,vector2list("55.46964 84.35872"),isnotloop);
	};
	onmessage("npckilled",3005)
	{
	  npcpatrol(10001,vector2list("55.46964 84.35872"),isnotloop);
	};
	onmessage("npckilled",3006)
	{
	  npcpatrol(10001,vector2list("55.46964 84.35872"),isnotloop);
	};
	onmessage("npckilled",3007)
	{
	  npcpatrol(10001,vector2list("55.46964 84.35872"),isnotloop);
	};
	onmessage("npckilled",3008)
	{
	  npcpatrol(10001,vector2list("55.46964 84.35872"),isnotloop);
	};
	onmessage("npckilled",3009)
	{
	  npcpatrol(10001,vector2list("55.46964 84.35872"),isnotloop);
	};
	onmessage("finalblow")
	{
    lockframe(0.2);
	};
	onmessage("allnpckilled")
	{
    //camerayaw(-80,3100);
    //wait(500);
    //cameraheight(2.3,10);
	  //cameradistance(7.6,10);
	  lockframe(0.01);
    wait(500);
    lockframe(0.05);
    wait(1800);
    lockframe(0.08);
    wait(300);
    lockframe(0.2);
    wait(500);
    lockframe(1.0);
		wait(300);
		//camerayaw(0,100);
	  //cameraheight(-1,100);
	  //cameradistance(-1,100);
	  wait(3000);
		//检测网络状态
		while(!islobbyconnected() && @reconnectCount<10)
		{
		  reconnectlobby();
		  wait(3000);
		  inc(@reconnectCount);
		};
		if(islobbyconnected())
		{
		  missioncompleted(1010);
		  terminate();
		} else {
		  missionfailed(1010);
		};
	};
  onmessage("missionfailed")
  {
    missionfailed(1010);
    terminate();
  };
};
