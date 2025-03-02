using pythonbackendgame.Models;
using static pythonbackendgame.Pages.GamePage.GameComponents.TeamGameComponents.ActiveTeamGameComponent;
using System;
using static pythonbackendgame.Models.MainDataModel;

namespace pythonbackendgame.Pages.GamePage.GameComponents
{
    public class MinigameGeneration
    {
        //0-sixnine
        //1-makeasquare
        //2-memorizecolornumber
        //3-memorizecolorshape
        //4-matchcolorword
        //5-dragcircle
        static int[] tempgames = { 0,1,2,3,4,5 };
        public static void StartNewGameLeech(LeechSendModel lsm)
        {
            Random rndm = new Random();

            int whatgame = tempgames[rndm.Next(0, tempgames.Length)];

            int[] tempvars = new int[4];
            tempvars = GenerateVariables(whatgame);
            lsm.GameVar1 = tempvars[0];
            lsm.GameVar2 = tempvars[1];
            lsm.GameVar3 = tempvars[2];
            lsm.GameVar4 = tempvars[3];
            lsm.PlayerGame = whatgame;
        }
        public static void StartNewGameHost(MainDataModel mdm)
        {
            //mdm.P1State = 0;
            Random rndm = new Random();

            int whatgame = tempgames[rndm.Next(0, tempgames.Length)];

            int[] tempvars = new int[4];
            tempvars = GenerateVariables(whatgame);
            mdm.P1GameVar1 = tempvars[0];
            mdm.P1GameVar2 = tempvars[1];
            mdm.P1GameVar3 = tempvars[2];
            mdm.P1GameVar4 = tempvars[3];
            mdm.P1Game = whatgame;
        }
        public static int[] InitialHostGeneration()
        {
            Random rndm = new Random();
            int whatgame = tempgames[rndm.Next(0, tempgames.Length)];

            int[] tempvars = new int[4];
            tempvars = GenerateVariables(whatgame);
            return new[] { tempvars[0], tempvars[1], tempvars[2], tempvars[3], whatgame };
        }

        private static Random random = new Random();
        private static double CircleRadius = 20;  // Half the width/height of the circle
        private static double MinDistance = 40;   // Minimum distance between circle centers

        public static List<Circle> GenerateCircles()
        {
            var circles = new List<Circle>();

            for (int i = 1; i <= 12; i++)
            {
                Circle newCircle;

                // Try generating a new position until no overlap is detected
                do
                {
                    newCircle = new Circle
                    {
                        Number = i,
                        X = random.Next(0, 400),  // Random X position
                        Y = random.Next(0, 400)   // Random Y position
                    };
                }
                while (IsOverlapping(newCircle, circles));

                // Add the non-overlapping circle to the list
                circles.Add(newCircle);
            }

            return circles;
        }

        // Check if the new circle overlaps with any existing circle
        private static bool IsOverlapping(Circle newCircle, List<Circle> existingCircles)
        {
            foreach (var circle in existingCircles)
            {
                double distance = Math.Sqrt(Math.Pow(newCircle.X - circle.X, 2) + Math.Pow(newCircle.Y - circle.Y, 2));

                // If the distance between centers is less than the minimum distance, they overlap
                if (distance < MinDistance)
                {
                    return true;
                }
            }
            return false;
        }
        //this will take in what game it is and return an array of the proper variables for the game
        //will return in the form [var1,var2,var3,var4] depending on the game
        public static int[] GenerateVariables(int whatgame)
        {
            Random rndm = new Random();
            if (whatgame == 0)
            {
                //sixninegame
                //just needs to generate 4 random numbers between 0-133 which will be where the 6s or 9s are
                int first, second, third, fourth;
                do
                {
                    first = rndm.Next(0, 133);
                    second = rndm.Next(0, 133);
                    third = rndm.Next(0, 133);
                    fourth = rndm.Next(0, 133);
                } while (first == second || first == third || first == fourth || second == third || second == fourth || third == fourth);
                return new[] { first, second, third, fourth };
            }
            if (whatgame == 1)
            {
                //make a squre
                //only returns 1-3 so no square starts not needing to be rotated
                return new[] { rndm.Next(1, 4), rndm.Next(1, 4), rndm.Next(1, 4), rndm.Next(1, 4) };
            }
            if (whatgame == 2)
            {
                //memorizecolornumber
                //[colors,numbers,questoin1or2,answer]
                return new[] { rndm.Next(1, 10), rndm.Next(1, 10), rndm.Next(0, 2), rndm.Next(0, 3) };
            }
            if (whatgame == 3)
            {
                //memorizecolorshape
                //[colors,shapes,questoin1or2,answer]
                return new[] { rndm.Next(1, 25), rndm.Next(1, 25), rndm.Next(0, 2), rndm.Next(0, 4) };
            }
            if (whatgame == 4)
            {
                //matchcolorword
                return new[] { rndm.Next(0, 4), rndm.Next(0, 4), rndm.Next(0, 4), 0 };
            }
            if (whatgame == 5)
            {
                //drag circle
                return new[] { 50 , 50 , rndm.Next(0, 4), 0 };
            }
            //else if (whatgame == 1)
            else
            {
                return new[] { rndm.Next(0, 32), rndm.Next(0, 32), rndm.Next(0, 32), rndm.Next(0, 32) };
            }
        }

        public static async Task PlayerFinishedGame(int player, bool won, int lobby)
        {
            //need to start new game here
            //await hubConnection.SendAsync("UpdateInGame", lobby, player, 0);
            //below is to change player health
            if (won)
            {
                switch (player)
                {
                    //case 1: await hubConnection.SendAsync("HealthChanged", lobby, 2, 3, true); break;
                    //case 2: await hubConnection.SendAsync("HealthChanged", lobby, 4, 3, true); break;
                   //case 3: await hubConnection.SendAsync("HealthChanged", lobby, 1, 3, true); break;
                   // case 4: await hubConnection.SendAsync("HealthChanged", lobby, 3, 3, true); break;
                    default: break;
                }
                await Task.Delay(600);
            }
            else { }
            await MinigameGeneration.GenerateNewGame(player, lobby);
            //await Task.Delay(2000);
            //await hubConnection.SendAsync("UpdateInGame", lobby, player, 1);
        }
        public static async Task GenerateNewGame(int player, int lobby)
        {
            Random rndm = new Random();
            int whatgame = rndm.Next(0, 6);
            int[] tempvars = new int[4];
            tempvars = GenerateVariables(whatgame);
            //await hubConnection.SendAsync("UpdateGameVars", lobby, player, tempvars[0], tempvars[1], tempvars[2], tempvars[3], whatgame);
        }
    }
}
