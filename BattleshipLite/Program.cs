using System;
using BattleshipLiteLibrary;
using BattleshipLiteLibrary.Models;

namespace BattleshipLite
{
    static class Program
    {
        public static void Main()
        {
            WelcomeMessage();

            var activePlayer = CreatePlayer("Player 1");
            var opponent = CreatePlayer("Player 2");
            PlayerInfoModel winner = null;

            do
            {
                DisplayShotGrid(activePlayer);
                
                RecordPlayerShot(activePlayer, opponent);
                
                Console.Clear();
                // Determine if the game should continue
                var doesGameContinue = GameLogic.PlayerStillActive(opponent);

                // If over, set activePlayer as the winner 
                // else, swap positions (activePlayer to opponent)
                if (doesGameContinue)
                {
                    // Swap positions (using Tuple)
                    (activePlayer, opponent) = (opponent, activePlayer);
                }
                else
                {
                    winner = activePlayer;
                }

            } while (winner == null);

            IdentifyWinner(winner);

        }

        private static void IdentifyWinner(PlayerInfoModel winner)
        {
            Console.WriteLine($"Congratulations to {winner.UsersName} for winning!");
            Console.WriteLine($"{winner.UsersName} took {GameLogic.GetShotCount(winner)} shots.");
        }

        private static void RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
        {
            // Ask for a shot (we ask for "B2")
            // Determine what row and column that is - split it apart
            // Determine if that is a valid shot
            // Go back to the beginning if not a valid shot
            bool isValidShot;
            var row = string.Empty;
            var column = 0;

            do
            {
                var shot = AskForShot(activePlayer);

                try
                {
                    (row, column) = GameLogic.SplitShotIntoRowAndColumn(shot);
                    isValidShot = GameLogic.ValidateShot(activePlayer, row, column);
                }
                catch (Exception ex)
                {
                    isValidShot = false;
                    Console.WriteLine(ex.Message);
                }

                if (isValidShot == false)
                {
                    Console.WriteLine("Invalid Shot Location. Please try again.");
                }

            } while (!isValidShot);

            // Determine shot results 
            var isAHit = GameLogic.IdentifyShotResult(opponent, row, column);

            // Record results
            GameLogic.MarkShotResult(activePlayer, row, column, isAHit);

        }

        private static string AskForShot(PlayerInfoModel player)
        {
            Console.Write($"It's your turn {player.UsersName}. Please enter your shot selection: ");
            var output = Console.ReadLine();

            return output;
        }

        private static void DisplayShotGrid(PlayerInfoModel activePlayer)
        {
            var currentRow = activePlayer.ShotGrid[0].SpotLetter;

            foreach (var gridSpot in activePlayer.ShotGrid)
            {
                if (gridSpot.SpotLetter != currentRow)
                {
                    Console.WriteLine();
                    currentRow = gridSpot.SpotLetter;
                }

                if (gridSpot.Status == GridSpotStatus.Empty)
                {
                    Console.Write($" {gridSpot.SpotLetter}{gridSpot.SpotNumber} ");
                }
                else if (gridSpot.Status == GridSpotStatus.Hit)
                {
                    Console.Write(" X  ");
                }
                else if (gridSpot.Status == GridSpotStatus.Miss)
                {
                    Console.Write(" O  ");
                }
                else
                {
                    Console.Write(" ?  ");
                }
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        private static void WelcomeMessage()
        {
            Console.WriteLine("Welcome to Battleship Lite");
            Console.WriteLine("Created by: Saeed Adam\n");
        }

        private static PlayerInfoModel CreatePlayer(string playerTitle)
        {
            var output = new PlayerInfoModel();

            Console.WriteLine($"Player information for {playerTitle}");

            // Ask the user for their name
            output.UsersName = AskForUsersName();

            // Load up the shot grid 
            GameLogic.InitializeGridModel(output);

            // Ask the user for their 5 ship placements
            PlaceShips(output);

            // Clear console
            Console.Clear();

            return output;

        }

        private static string AskForUsersName()
        {
            Console.Write("What is your name: ");
            var output = Console.ReadLine();
            

            return output;
        }

        private static void PlaceShips(PlayerInfoModel model)
        {
            do
            {
                Console.Write($"Where do you want to place ship number {model.ShipLocations.Count + 1}: ");
                var location = Console.ReadLine();

                var isValidLocation = GameLogic.PlaceShip(model, location);

                if (!isValidLocation)
                {
                    Console.WriteLine("That was not a valid location. Please try again.");
                }

            } while (model.ShipLocations.Count < 5);
        }
    }
}
