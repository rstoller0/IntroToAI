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

            //create initial state
            State current = new State();
            State neighbor = new State(current);
            State bestNewState = new State(current);
            int numBetterStates = 0;

            //variables for end display
            int numStateChanges = 0;
            int numRestarts = 0;

            //print initial state
            current.printState();

            //print heuristic value
            Console.WriteLine("Heuristic Value of State: " + current.getHeuristic());

            while (current.getHeuristic() > 0) {

                //print initial state
                Console.WriteLine("Current heuristic: " + current.getHeuristic());
                current.printState();

                //resetting states for testing
                neighbor.newState(current);
                bestNewState.newState(current);
                numBetterStates = 0;

                for (int c = 0; c < 8; c++) {

                    for (int r = 0; r < 8; r++) {
                        neighbor.setState(r, c);

                        if (neighbor.getHeuristic() < current.getHeuristic())
                        {
                            numBetterStates++;
                        }

                        if (neighbor.getHeuristic() < bestNewState.getHeuristic())
                        {
                            bestNewState.setState(r, c);
                            numStateChanges++;
                        }
                    }
                }

                if (bestNewState.getHeuristic() < current.getHeuristic()) {
                    current.newState(bestNewState);
                    Console.WriteLine("Neighbors found with lower heuristic: " + numBetterStates);
                    Console.WriteLine("Setting new current state");
                } else {
                    current.randomRestart();
                    Console.WriteLine("RESTART");
                    numRestarts++;
                    numStateChanges = 0;
                }

                //blankline
                Console.WriteLine();
            }

            current.printState();
            Console.WriteLine("Solution Found!");
            Console.WriteLine("State Changes: " + numStateChanges);
            Console.WriteLine("Restarts: " + numRestarts);

            //pause console
            Console.ReadLine();
        }
    }
}
