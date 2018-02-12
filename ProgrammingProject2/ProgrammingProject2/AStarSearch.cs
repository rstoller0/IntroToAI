using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingProject2
{
    class AStarSearch {

        static void Main(string[] args) {
            //Min Heap for Open List!!!
            PriorityQueue openList = new PriorityQueue();
            //Dictionary for Closed List [Dictionary<Key, Value>] Key = what we search for, Value = what is stored ((using string as key, for the board space[r, c]))
            Dictionary<string, Node> closedList = new Dictionary<string, Node>(255);
            Node[,] boardSpace = new Node[15, 15];//the 2D array to hold the tile board
            Random rng = new Random();//random number generator
            String userInput = "";//variable to capture user input
            int startingRow = -1, startingColumn = -1, goalRow = -1, goalColumn = -1;//starting and goal positions
            Stack<Node> pathway = new Stack<Node>();//stack used to hold and calculate list version of path

            //board creation
            #region
            //create board and each tile
            for (int r = 0; r < 15; r++) {//each row
                for (int c = 0; c < 15; c++) {//each column
                    int chance = rng.Next(0, 9);

                    if (chance == 1) {//if number generated is 1 out of 0-9 (10 numbers)
                        boardSpace[r, c] = new Node(r, c, 1);//blocked
                    } else {
                        boardSpace[r, c] = new Node(r, c, 0);//not blocked
                    }
                }
            }
            #endregion

            while (string.Equals(userInput, "No", StringComparison.OrdinalIgnoreCase) != true) {//while user is still inputing new starting/goal positions
                //node reseter
                #region
                for (int r = 0; r < 15; r++) {//each row
                    for (int c = 0; c < 15; c++) {//each column
                        if (boardSpace[r, c].getType() == 1) {//if number generated is 1 out of 0-9 (10 numbers)
                            boardSpace[r, c] = new Node(r, c, 1);//blocked
                        } else {
                            boardSpace[r, c] = new Node(r, c, 0);//not blocked
                        }
                    }
                }
                #endregion

                //display board
                #region
                for (int r = 0; r < 15; r++) {//row
                    for (int c = 0; c < 15; c++) {//all columns on each row

                        boardSpace[r, c].displayTile();
                    }

                    //move to next line
                    Console.WriteLine();
                }
                #endregion

                startingRow = startingColumn = goalRow = goalColumn = -1;//reset all positions to null (-1)

                //collecting user input
                #region
                Console.Write("Enter the Row of the starting position: ");
                userInput = Console.ReadLine();

                if (Int32.TryParse(userInput, out int sRow)) {//capture starting row number
                    startingRow = sRow;
                } else {//assign default row value if not Parsable
                    startingRow = 0;
                }

                Console.Write("Enter the Column of the starting position: ");
                userInput = Console.ReadLine();

                if (Int32.TryParse(userInput, out int sColumn)) {//capture starting column number
                    startingColumn = sColumn;
                } else {//assign default column value if not Parsable
                    startingColumn = 0;
                }

                Console.Write("Enter the Row of the goal position: ");
                userInput = Console.ReadLine();

                if (Int32.TryParse(userInput, out int gRow)) {//capmture goal row number 
                    goalRow = gRow;
                } else {//assign default row value if not Parsable
                    goalRow = 0;
                }

                Console.Write("Enter the Column of the goal position: ");
                userInput = Console.ReadLine();

                if (Int32.TryParse(userInput, out int gColumn)) {//capture goal column number
                    goalColumn = gColumn;
                } else {//assign default column value if not Parsable
                    goalColumn = 0;
                }
                #endregion

                //Attempt to use A*
                #region
                Node currentSpace = null;//assign null to allow enterance to while loop
                openList.add(boardSpace[startingRow, startingColumn]);//add starting tile to openList for first movement turn
                while (currentSpace != boardSpace[goalRow, goalColumn] && openList.getSize() != 0)//While not at goal or not completely searched
                {
                    currentSpace = openList.remove();//pop off best move
                    //Console.WriteLine("Current Node: " + currentSpace.toString());//for debugging
                    string addKey = "" + (currentSpace.getRow()) + "_" + (currentSpace.getCol());
                    closedList.Add(addKey, currentSpace);//add new tile to closedList

                    for (int r = 1; r < 4; r++) {//calculating moves [rows]
                        for (int c = 1; c < 4; c++) {//calculating moves [columns]
                            
                            //temp string key for assuring the space to be checked is not already in the closedList
                            string key = "" + (currentSpace.getRow() - 2 + r) + "_" + (currentSpace.getCol() - 2 + c);
                            
                            //if space on board is in bounds
                            if ((currentSpace.getRow() - 2 + r) > -1 && (currentSpace.getRow() - 2 + r) < 15 && (currentSpace.getCol() - 2 + c) > -1 && (currentSpace.getCol() - 2 + c) < 15) {
                                
                                //node variable to shorten code
                                Node potentialNode = boardSpace[currentSpace.getRow() - 2 + r, currentSpace.getCol() - 2 + c];
                                
                                //if space is not in the closedList
                                if (potentialNode.getType() == 0 && !closedList.ContainsKey(key)) {

                                    //set heuristic value with manhattan method
                                    int tempH = (Math.Abs(potentialNode.getRow() - goalRow) + Math.Abs(potentialNode.getCol() - goalColumn)) * 10;
                                    potentialNode.setH(tempH);

                                    //calculate cost to get to space from start through this space
                                    int tempG = currentSpace.getG();
                                    if ((r == 1 && c == 1) || (r == 1 && c == 3) || (r == 3 && c == 1) || (r == 3 && c == 3)) {//if a diagonal move
                                        tempG += 14;
                                    } else {//else adjacent move
                                        tempG += 10;
                                    }
                                    
                                    //if moving through this space is better than previous or has not been assigned, reassign/assign cost and parent
                                    if (potentialNode.getG() > tempG || potentialNode.getG() == 0) {
                                        potentialNode.setG(tempG);
                                        potentialNode.setParent(currentSpace);
                                    }

                                    //set the F value of the space being checked
                                    potentialNode.setF();

                                    if (openList.search(potentialNode) == false)
                                    {
                                        openList.add(potentialNode);
                                    }
                                    else
                                    {
                                        openList.resort(potentialNode);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                //Clear the screen
                Console.Clear();

                //if unachievable goal
                #region
                if (openList.getSize() == 0) {
                    Console.WriteLine("Goal is unreachable.");
                } else {//calculate and display pathway
                    Node pathwayNode = boardSpace[goalRow, goalColumn];
                    while (pathwayNode.getParent() != null)
                    {
                        //set as part of path
                        pathwayNode.setIsPath(true);

                        //assign new pathwayNode
                        pathwayNode = pathwayNode.getParent();
                    }
                    //get starting tile as part of pathway
                    pathwayNode.setIsPath(true);
                }
                #endregion

                //display board with pathway
                #region
                Console.Write("Start: ".PadRight(7) + (startingRow + "_" + startingColumn).PadLeft(5) + "\n");
                Console.Write("Goal: ".PadRight(7) + (goalRow + "_" + goalColumn).PadLeft(5) + "\n");
                for (int r = 0; r < 15; r++) {//row
                    for (int c = 0; c < 15; c++) {//all columns on each row
                        boardSpace[r, c].displayTileWithPathway();
                    }

                    //move to next line
                    Console.WriteLine();
                }
                #endregion

                //check if user wants to find another path
                #region
                while (string.Equals(userInput, "No", StringComparison.OrdinalIgnoreCase) != true && string.Equals(userInput, "Yes", StringComparison.OrdinalIgnoreCase) != true) {//while user hasn't input a valid command

                    Console.Write("Would you like to input another starting and goal position? (Yes or No) ");
                    userInput = Console.ReadLine();

                    if (string.Equals(userInput, "Yes", StringComparison.OrdinalIgnoreCase) == true) {//if user wants to create another path, remove all open list nodes and closed list nodes

                        int tempSize = openList.getSize();
                        for (int i = 0; i < tempSize; i++) {
                            openList.remove();
                        }
                        closedList.Clear();

                        //Clear the screen
                        Console.Clear();
                    }
                }
                #endregion
            }

            //store list of pathway in stack for easy display
            #region
            Node patNod = boardSpace[goalRow, goalColumn];
            while (patNod.getParent() != null)
            {
                //add to stack for listed display of path
                pathway.Push(patNod);

                //assign new patNod
                patNod = patNod.getParent();
            }
            //get starting tile as part of pathway
            pathway.Push(patNod);
            #endregion

            //display pathway in list form
            #region
            int pathCount = pathway.Count;
            for (int s = 0; s < pathCount; s++) {
                Console.WriteLine(("Path Step " + (s + 1) + ": ").PadRight(14) + pathway.Pop().toString());
            }
            #endregion

            //pause program at the end
            Console.ReadLine();
        }
    }
}