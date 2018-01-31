using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingProject1
{
    class EightQueens {

        public static void Main(string[] args)
        {

            //create initial state and additional variables
            State current = new State();
            State neighbor = new State(current);
            State bestNewState = new State(current);
            int numBetterStates = 0;

            //variables for end display
            int numStateChanges = 0;
            int numRestarts = 0;

            while (current.getHeuristic() > 0) {//while not the GOAL STATE

                //print initial state
                Console.WriteLine("Current heuristic: " + current.getHeuristic());
                current.printState();

                //resetting states for testing
                neighbor.newState(current);
                bestNewState.newState(current);
                numBetterStates = 0;

                for (int c = 0; c < 8; c++) {
                    //reset neighbor state to current before next column is checked
                    neighbor.newState(current);

                    for (int r = 0; r < 8; r++) {
                        //start checking heuristics of each column
                        neighbor.setState(r, c);

                        if (neighbor.getHeuristic() < current.getHeuristic()) {
                            //if neighbor state's heuristic is lower than initial state, add to number of better states
                            numBetterStates++;
                        }

                        if (neighbor.getHeuristic() < bestNewState.getHeuristic()) {
                            //if neighbor state's heuristic is lower than the best possible new state found thus far, change best possible new state to neighbor state
                            bestNewState.newState(neighbor);
                        }
                    }
                }

                if (bestNewState.getHeuristic() < current.getHeuristic()) {
                    //if better new state is found, assign to current state
                    current.newState(bestNewState);
                    Console.WriteLine("Neighbors found with lower heuristic: " + numBetterStates);
                    Console.WriteLine("Setting new current state");
                    numStateChanges++;
                } else {
                    //else no better state found, restart
                    current.randomRestart();
                    Console.WriteLine("RESTART");
                    numRestarts++;
                    numStateChanges = 0;
                }

                //blankline
                Console.WriteLine();
            }

            //print information about solution discovery
            current.printState();
            Console.WriteLine("Solution Found!");
            Console.WriteLine("State Changes: " + numStateChanges);
            Console.WriteLine("Restarts: " + numRestarts);

            //pause console
            Console.ReadLine();
        }
    }
}
