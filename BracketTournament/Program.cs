using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BracketTournament
{
    class Program
    {
        static void Main(string[] args)
        {
            //  ODO: Psuedo code map for assignment
            //  Read Tournaments bracket data from file
            //  Validate number of participants
            //  Assign each line/player a random unique bracket id
            //  Place each bowler into an array by unique bracked id
            //
            //  Compare corresponding groups of 8 bowlers for round one, ie 0-1, 2-3, 4-5, & 6-7
            //  Winner of 0/1 goes to round two and becomes 0
            //  Winner of 2/3 goes to round two and becomes 1
            //  Winner of 4/5 goes to round two and becomes 2
            //  Winner of 6/7 goes to round two and becomes 4

            //  Compare corresponding groups of 4 bowlers for round two
            //  Winner of 0/1 goes to round three and becomes 0
            //  Winner of 2/3 goes to round three and becomes 1
            //
            //  Compare finalist
            //  Winner of 0/1 gets first & $25, loser gets 2nd & $10

            // Random Number Generator
            var rand = new Random();

            // Declare variables
            int numParticipants;
            int participantsNeeded;

            // File Path & Name
            String filePath =  @"D:\Documents\CPW205_OODA\HW01\BracketTournament\BracketTournament\ReferenceMaterials\";
            String fileName = "tournament.txt";

            // Create an array of participants from the text file
            string[] txtArray = File.ReadAllLines(filePath+fileName);
            
            // Determine Number of Participants
            numParticipants = txtArray.Length;

            // Display list of participants, in the order they signed up.
            Console.WriteLine("Bracket Entry Form: " + fileName);
            foreach (string line in txtArray)
            {
                // Use a tab to indent each line of the file.
                Console.WriteLine("  " + line);
            }
            Console.WriteLine("");

            // Calculate remainder of participants from 8 bowler brackets
            participantsNeeded = 8 - numParticipants % 8;
            int numberOfBrackets = numParticipants / 8;

            // Display message of how many brackets we have
            // or how many participants/entries are needed to have brackets of 8 bowlers
            string singularPlural = "brackets";
            if (numParticipants % 8 == 0)
            {
                if (numberOfBrackets == 1)
                {
                    singularPlural = "bracket";
                }
                Console.WriteLine("We have " + numberOfBrackets + " " + singularPlural + " in the brackets tournament");
            }
            else if (numParticipants % 8 != 0)
            {
                Console.WriteLine("We need " + participantsNeeded + " additional entries to create a brackets tournament");
            }

            // Initialize array for randomized bracket entries
            string[] bracketArray = new string[numParticipants];

            /*
             * Available Bracket Positions
             * This will be filled with unique random numbers
             * These random numbers will be used to place each line of participants
             * into the the bracket array in a random order
             */
            int[] bracketPositions = { -1, -1, -1, -1, -1, -1, -1, -1 };
            for (int i = 0; i < bracketPositions.Length; i++)
            {
                int tempNum = rand.Next(8);
                while (bracketPositions.Contains(tempNum))
                {
                    tempNum = rand.Next(8);
                }
                bracketPositions[i] = tempNum;
            }

            /*
             * Copies participants from txtArray, ie from text file
             * into a bracket array, in a random order
             */
            int iterator = 0;
            foreach (string line in txtArray)
            {
                bracketArray[bracketPositions[iterator]] = line;
                iterator++;
            }

            // Displays the Randomized Tournament Bracket
            Console.WriteLine("\nRandom Tournament Bracket");
            for (int i = 0; i < bracketArray.Length; i++)
            {
                Console.WriteLine("  " + bracketArray[i]);
            }
            Console.WriteLine();

            /*
             * Create 2D array
             * This will allow easier comparisons
             */

            // create a list to store temp string data from file
            List<string> tempList = new List<string>();

            // places bracket array into a list
            foreach(string line in bracketArray)
            {
                string[] rowAsArray = line.Split(' ');
                foreach(string item in rowAsArray)
                {
                    tempList.Add(item);
                }
            }

            int numRows = bracketArray.Length;
            int numCols = bracketArray[0].Split(' ').Length;

            // creating 2D array to compare head to head matches for eliminations
            string[,] bracket2DArray = new string[numRows, numCols];
            int arrI = 0;

            for (int r = 0; r < numRows; r++)
            {
                for (int c = 0; c < numCols; c++)
                {
                    bracket2DArray[r, c] = tempList[arrI];
                    arrI++;
                }
            }

/*
 * ------ Determine Round Winners ----------------------------------------------
 *        Determining Winners should be refactored out as it's own method
 */

            /*
             * Determine Round One Winners
---->        * Needs to be refactored <----
             */
            Console.WriteLine("Round One Winners");
            string[,] roundTwo = new string[numRows / 2, numCols];
            int k = 0;

            for (int i = 0; i < numRows - 1; i += 2)
            {
                if (Convert.ToInt32(bracket2DArray[i, 1]) > Convert.ToInt32(bracket2DArray[i + 1, 1]))
                {
                    for (int j = 0; j < 4; j++)
                    {
                        roundTwo[k, j] = bracket2DArray[i, j];
                    }
                }
                else
                {
                    for (int j = 0; j < 4; j++)
                    {
                        roundTwo[k, j] = bracket2DArray[i+1, j];
                    }
                }
                k++;
            }

            // Display Round One Winners
            for (int r = 0; r < 4; r++)
            {
                Console.WriteLine("  " + roundTwo[r, 0]);
            }
            Console.WriteLine();

            /*
             * Determine Round Two Winners
             */
            Console.WriteLine("Round Two Winners");
            string[,] roundThree = new string[numRows / 4, numCols];
            k = 0;

            for (int i = 0; i < 4 - 1; i += 2)
            {
                if (Convert.ToInt32(roundTwo[i, 2]) > Convert.ToInt32(roundTwo[i+1, 2]))
                {
                    for (int j = 0; j < 4; j++)
                    {
                        roundThree[k, j] = roundTwo[i, j];
                    }
                }
                else
                {
                    for (int j = 0; j < 4; j++)
                    {
                        roundThree[k, j] = roundTwo[i+1, j];
                    }
                }
                k++;
            }

            // Display Round Two Winners
            for (int r = 0; r < 2; r++)
            {
                Console.WriteLine("  " + roundThree[r, 0]);
            }
            Console.WriteLine();

            /*
             * Determine Winner & Runner Up
             */
            Console.WriteLine("Final Results");
            if (Convert.ToInt32(roundThree[0, 3]) > Convert.ToInt32(roundThree[1, 3]))
            {
                Console.WriteLine("  " + roundThree[0, 0] + " Wins $25");
                Console.WriteLine("  " + roundThree[1, 0] + " Wins $10");
            }
            else
            {
                Console.WriteLine("  " + roundThree[1, 0] + " Wins $25");
                Console.WriteLine("  " + roundThree[0, 0] + " Wins $10");
            }
        }
    }
}
