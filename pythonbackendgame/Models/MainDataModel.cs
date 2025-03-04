using Microsoft.AspNetCore.Components.Web;

namespace pythonbackendgame.Models
{
    public class MainDataModel
    {
        public int MyPlayerNumber { get; set; }
        public string MyLobby { get; set; }
        public string MyCursorStyle { get; set; }
        public int PlayersInLobby { get; set; }
        public int gamestate { get; set; }

        public int BrowserHeight { get; set; }
        public int BrowserWidth { get; set; }
        public double mycursx { get; set; }
        public double mycursy { get; set; }
        public void MouseMoved(MouseEventArgs e)
        {
            //this sets curs to the coords around the center bascially
            mycursx = e.ClientX - (BrowserWidth / 2);
            mycursy = e.ClientY - (BrowserHeight / 2);
        }
        public void RegularUpdateFromHost(MainDataModel fromhost, MainDataModel MDM,int localplayernumber)
        {
            MDM.P1Xcords = fromhost.P1Xcords;
            MDM.P1Ycords = fromhost.P1Ycords;
            MDM.P1GameVar1 = fromhost.P1GameVar1;
            MDM.P1GameVar2 = fromhost.P1GameVar2;
            MDM.P1GameVar3 = fromhost.P1GameVar3;
            MDM.P1GameVar4 = fromhost.P1GameVar4;
            MDM.P1Game = fromhost.P1Game;
            MDM.P1State = fromhost.P1State;
            MDM.P1Health = fromhost.P1Health;
            MDM.gamestate = fromhost.gamestate;
            MDM.PlayersInLobby = fromhost.PlayersInLobby;
            MDM.Circles = fromhost.Circles;
            MDM.currentteamgamenumber = fromhost.currentteamgamenumber;
            if (localplayernumber != 2)
            {
                MDM.P2Xcords = fromhost.P2Xcords;
                MDM.P2Ycords = fromhost.P2Ycords;
                MDM.P2Game = fromhost.P2Game;
                MDM.P2GameVar1 = fromhost.P2GameVar1;
                MDM.P2GameVar2 = fromhost.P2GameVar2;
                MDM.P2GameVar3 = fromhost.P2GameVar3;
                MDM.P2GameVar4 = fromhost.P2GameVar4;
                MDM.P2State = fromhost.P2State;
                MDM.P2Health = fromhost.P2Health;

            }
            else if (localplayernumber != 3)
            {
                MDM.P3Xcords = fromhost.P3Xcords;
                MDM.P3Ycords = fromhost.P3Ycords;
                MDM.P3Game = fromhost.P3Game;
                MDM.P3GameVar1 = fromhost.P3GameVar1;
                MDM.P3GameVar2 = fromhost.P3GameVar2;
                MDM.P3GameVar3 = fromhost.P3GameVar3;
                MDM.P3GameVar4 = fromhost.P3GameVar4;
                MDM.P3State = fromhost.P3State;
                MDM.P3Health = fromhost.P3Health;
            }
            else if (localplayernumber != 4)
            {
                MDM.P4Xcords = fromhost.P4Xcords;
                MDM.P4Ycords = fromhost.P4Ycords;
                MDM.P4Game = fromhost.P4Game;
                MDM.P4GameVar1 = fromhost.P4GameVar1;
                MDM.P4GameVar2 = fromhost.P4GameVar2;
                MDM.P4GameVar3 = fromhost.P4GameVar3;
                MDM.P4GameVar4 = fromhost.P4GameVar4;
                MDM.P4State = fromhost.P4State;
                MDM.P4Health = fromhost.P4Health;
            }

        }
        public void RegularUpdateFromLeech(LeechSendModel lsm, MainDataModel MDM)
        {
            if (lsm.lastnumbericlicked > MDM.currentteamgamenumber) { MDM.currentteamgamenumber = lsm.lastnumbericlicked; }

            switch (lsm.PlayerLobbyNumber)
            {
                case 2:
                    MDM.P2Xcords = lsm.PlayerXcords;
                    MDM.P2Ycords = lsm.PlayerYcords;
                    MDM.P2GameVar1 = lsm.GameVar1;
                    MDM.P2GameVar2 = lsm.GameVar2;
                    MDM.P2GameVar3 = lsm.GameVar3;
                    MDM.P2GameVar4 = lsm.GameVar4;
                    MDM.P2Game = lsm.PlayerGame;
                    MDM.P2State = lsm.MyState;
                    MDM.P2Health = lsm.MyHealth;
                    MDM.Circlesp2clicked = lsm.CirclesIClicked;
                    break;
                case 3:
                    MDM.P3Xcords = lsm.PlayerXcords;
                    MDM.P3Ycords = lsm.PlayerYcords;
                    MDM.P3GameVar1 = lsm.GameVar1;
                    MDM.P3GameVar2 = lsm.GameVar2;
                    MDM.P3GameVar3 = lsm.GameVar3;
                    MDM.P3GameVar4 = lsm.GameVar4;
                    MDM.P3Game = lsm.PlayerGame;
                    MDM.P3State = lsm.MyState;
                    MDM.P3Health = lsm.MyHealth;
                    MDM.Circlesp3clicked = lsm.CirclesIClicked;
                    break;
                case 4:
                    MDM.P4Xcords = lsm.PlayerXcords;
                    MDM.P4Ycords = lsm.PlayerYcords;
                    MDM.P4GameVar1 = lsm.GameVar1;
                    MDM.P4GameVar2 = lsm.GameVar2;
                    MDM.P4GameVar3 = lsm.GameVar3;
                    MDM.P4GameVar4 = lsm.GameVar4;
                    MDM.P4Game = lsm.PlayerGame;
                    MDM.P4State = lsm.MyState;
                    MDM.P4Health = lsm.MyHealth;
                    MDM.Circlesp4clicked = lsm.CirclesIClicked;
                    break;
            }
        }
        public void UpdateCircles(List<int> clickedCircles, MainDataModel MDM)
        {
            MDM.Circles.RemoveAll(circle => clickedCircles.Contains(circle.Number));
        }
        public void FinishedGame(bool won, int playeriam, MainDataModel mainDataModel, LeechSendModel lsm)
        {
            if (playeriam == 1) { mainDataModel.P1State = 0; }
            else
            { 
                lsm.MyState = 0; 
                switch (playeriam) 
                { 
                    case 2: mainDataModel.P2State = 0; break; 
                    case 3: mainDataModel.P3State = 0; break; 
                    case 4: mainDataModel.P4State = 0; break; 
                } 
            }
            if (this.gamestate!=0)
            {
                if (won)
                {
                    switch (playeriam)
                    {
                        case 1:
                            mainDataModel.P1Health += 1;
                            break;
                        case 2:
                            lsm.MyHealth += 1;
                            mainDataModel.P2Health += 1;
                            break;
                        case 3:
                            lsm.MyHealth += 1;
                            mainDataModel.P3Health += 1;
                            break;
                        case 4:
                            lsm.MyHealth += 1;
                            mainDataModel.P4Health += 1;
                            break;
                    }
                }
                else
                {
                    switch (playeriam)
                    {
                        case 1:
                            mainDataModel.P1Health -= 1;
                            break;
                        case 2:
                            lsm.MyHealth -= 1;
                            mainDataModel.P2Health -= 1;
                            break;
                        case 3:
                            lsm.MyHealth -= 1;
                            mainDataModel.P3Health -= 1;
                            break;
                        case 4:
                            lsm.MyHealth -= 1;
                            mainDataModel.P4Health -= 1;
                            break;
                    }
                }
            }
            
            
        }
        public class Circle
        {
            public int Number { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
        }
        public List<Circle> Circles { get; set; }
        public string ReasonForUpdate { get; set; }
        public int SpecifyPlayerInit { get; set; }
        public string SpecifyPlayerID { get; set; }
        public double P1Xcords { get; set; } = -99999;
        public double P1Ycords { get; set; } = -99999;
        public double P2Xcords { get; set; } = -99990;
        public double P2Ycords { get; set; } = -099999;
        public double P3Xcords { get; set; } = -099999;
        public double P3Ycords { get; set; } = -09999;
        public double P4Xcords { get; set; } = -0999999;
        public double P4Ycords { get; set; } = -0999;

        public string ponezone { get; set; }
        public string ptwozone { get; set; }
        public string pthreezone { get; set; }
        public string pfourzone { get; set; }
        public int MyCurrentGame = 1;
        public int[] MyGameVars;
        
        
        public int P1Game { get; set; }
        public int P1GameVar1 { get; set; }
        public int P1GameVar2 { get; set; }
        public int P1GameVar3 { get; set; }
        public int P1GameVar4 { get; set; }
        public int P2Game { get; set; }
        public int P2GameVar1 { get; set; }
        public int P2GameVar2 { get; set; }
        public int P2GameVar3 { get; set; }
        public int P2GameVar4 { get; set; }
        public int P3Game { get; set; }
        public int P3GameVar1 { get; set; }
        public int P3GameVar2 { get; set; }
        public int P3GameVar3 { get; set; }
        public int P3GameVar4 { get; set; }
        public int P4Game { get; set; }
        public int P4GameVar1 { get; set; }
        public int P4GameVar2 { get; set; }
        public int P4GameVar3 { get; set; }
        public int P4GameVar4 { get; set; }
        public int P1State { get; set; }
        public int P2State { get; set; }
        public int P3State { get; set; }
        public int P4State { get; set; }
        public int P1Health { get; set; }
        public int P2Health { get; set; }
        public int P3Health { get; set; }
        public int P4Health { get; set; }
        public string StartTime { get; set; }
        public bool P1Present { get; set; }
        public bool P2Present { get; set; }
        public bool P3Present { get; set; }
        public bool P4Present { get; set; }
        public string P2ID { get; set; }
        public string P3ID { get; set; }
        public string P4ID { get; set; }

        public List<int> Circlesp1clicked { get; set; }
        public List<int> Circlesp2clicked { get; set; }
        public List<int> Circlesp3clicked { get; set; }
        public List<int> Circlesp4clicked { get; set; }
        public int currentteamgamenumber { get; set; }

    }
}
