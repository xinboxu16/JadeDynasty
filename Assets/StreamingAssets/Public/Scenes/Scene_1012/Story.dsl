story(1)
{
};
story(2)
{
    local
    {
        @npcbatch(0);
        @reconnectCount(0);
    };
	onmessage("start")
	{
		showui(false);
		wait(10);
		cameraenable("Main Camera",false);
		wait(10);
		cameraenable("Camera01",false);
		wait(10);
		cameraenable("Camera02",false);
		wait(10);
		cameraenable("Camera03",false);
		wait(10);
		sendgfxmessage("StoryObj","SetPlayerselfScale",1.3,1.3,1.3); 
		wait(10);
		sendgfxmessage("GfxGameRoot","EnableBloom");
		publishlogicevent("ge_change_player_movemode","game",1);
		wait(10);
		cameraenable("Camera01",true);
		wait(10);
		sendgfxmessage("HuLun_02","SetVisible",0);
		sendgfxmessage("Camera01","DimScreen",10);
		wait(10);
		sendgfxmessage("Main Camera","PlaySound",1); 
		sendgfxmessage("Camera01","LightScreen",1500);
		sendgfxmessage("HuLun_01","PlayAnimation","Cut_01",0.9);
		wait(5000);
		cameraenable("Camera02",true);
		//wait(10);
		//sendgfxmessage("Camera02","LightScreen",100);
		wait(10);
		sendgfxmessage("HuLun_01","SetVisible",0);
		sendgfxmessage("HuLun_02","SetVisible",1);
		sendgfxmessage("HuLun_02","PlayAnimation","Cut_02",1.0);
		cameraenable("Camera01",false);
		wait(10);
		playerselfmovewithwaypoints(vector2list("23.24348 21.10176 23.24348 21.10176 23.38383 20.52621"));
		wait(10);
		lockframe(0.2);
		sendgfxmessage("Door_Prison","PlayAnimation","Cut_01",1.0);
		//wait(680);
		sendgfxmessage("7_Scene_Menposui_01","PlayParticle");
		wait(2600);
		lockframe(0.9);
		playerselfmovewithwaypoints(vector2list("23.24348 21.10176 23.24348 21.10176 23.38383 20.52621"));
		wait(10);
		wait(345);
		sendgfxmessage("7_Scene_Menposui_02","PlayParticle");
		wait(10);
		//objmovewithwaypoints(playerselfid(),vector2list("23.38 20.52"));
		//sendgfxmessage("StoryObj","SetPlayerselfScale",1.3,1.3,1.3); 
		//wait(10);
		//publishlogicevent("ge_change_player_movemode","game",1);
		//wait(10);
		//playerselfmovewithwaypoints(vector2list("25.33961 24.04214 24.87665 23.21136 24.40665 22.36794 23.64957 21.00937 23.38383 20.52621"));
		//playerselfmovewithwaypoints(vector2list("23.15736 21.51246 23.15736 21.51246 23.38383 20.52621"));
		wait(280);
		lockframe(1.0);
		wait(1150);
		showdlg(101201);
		//cameraenable("Camera03",true);
		//wait(10);
		//cameraenable("Camera02",false);
		//wait(10);
	//};
	//onmessage("playerselfarrived")
	//{
		//wait(500);
		//playerselfface(2.9157471);
		//wait(300);
		sendgfxmessage("NewbeGuide", "OnCloseController");
		wait(1000);
		sendgfxmessage("Camera02","DimScreen",2500);
		sendgfxmessage("Main Camera","DimScreen",10);
		wait(2500);
		sendgfxmessage("StoryObj","SetPlayerselfScale",1.0,1.0,1.0); 
		wait(10);
		sendgfxmessage("StoryObj","SetPlayerselfPosition",26.58272,150.0811,27.31203); 
		wait(1000);
		sendgfxmessage("Main Camera","PlaySound",0); 
		sendgfxmessage("HuLun_02","SetVisible",0);
		cameraenable("Main Camera",true);
		wait(10);
		cameraenable("Camera02",false);
		wait(10);
		publishlogicevent("ge_change_player_movemode","game",2);
		wait(10);
		wait(10);
		sendgfxmessage("Main Camera","LightScreen",1500);
		wait(1500);
		showwall("StoryWall",true);
    sendgfxmessage("GfxGameRoot","DisableBloom");
    wait(200);
		showui(true);
		sendgfxmessage("NewbeGuide", "OnStartGuide");
        restartareamonitor(2);
  };
    onmessage("anyuserenterarea", 2)
    {
        loop(3)
        {
            createnpc(1001+$$);
        };
		sendgfxmessage("NewbeGuide", "OnCommonAttack",1);
        restartareamonitor(3);
    };
    onmessage("anyuserenterarea", 3)
    {
        loop(5)
        {
            createnpc(1004+$$);
        };
		sendgfxmessage("NewbeGuide", "OnSkillAAttack",1);
        restartareamonitor(4);
    };
    onmessage("anyuserenterarea", 4)
    {
        loop(5)
        {
            createnpc(1009+$$);
        };
        sendgfxmessage("NewbeGuide", "OnCommonAttack",2);
    };
    onmessage("missionfailed")
    {
        missionfailed(1010);
        terminate();
    };
    onmessage("allnpckilled")
    {
        if(0==@npcbatch)
        {
            showwall("GuideWall1", false);
        };
        if(1==@npcbatch)
        {
            showwall("GuideWall2", false);
        };
		if(2==@npcbatch)
		{
			wait(3000);
			sendgfxmessage("NewbeGuide","OnShowReturnCity");
			wait(10000);
            missioncompleted(1010);
		};
		inc(@npcbatch);
    };
    onmessage("returntomaincity")
    {
        while(!islobbyconnected() && @reconnectCount<10)
        {
          reconnectlobby();
          wait(3000);
          inc(@reconnectCount);
        };
        if(islobbyconnected())
        {
          missioncompleted(1010);
          wait(20000);
          missionfailed(1010);
          terminate();
        } else {
          missionfailed(1010);
          terminate();
        };
    }
};
