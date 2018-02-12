using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingProject2
{
    public class Node {
        private int row, col, f, g, h, type;
        private Node parent;
        private bool isPath;

        public Node(int r, int c, int t) {
            row = r;
            col = c;
            type = t;
            parent = null;
            isPath = false;
            //type 0 is traverseable, 1 is not
        }

        //mutator methods to set values
        public void setF() {
            f = g + h;
        }
        public void setG(int value) {
            g = value;
        }
        public void setH(int value) {
            h = value;
        }
        public void setParent(Node n) {
            parent = n;
        }
        public void setIsPath(bool p) {
            isPath = p;
        }

        //accessor methods to get values
        public int getF() {
            return f;
        }
        public int getG() {
            return g;
        }
        public int getH() {
            return h;
        }
        public Node getParent() {
            return parent;
        }
        public int getRow() {
            return row;
        }
        public int getCol() {
            return col;
        }
        public int getType() {
            return type;
        }

        public bool equals(Object inn) {
            //typecast to Node
            Node n = (Node) inn;

            return row == n.getRow() && col == n.getCol();
        }

        public void displayTile() {
            String message = row + "," + col;

            if (type == 1) {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(message.PadLeft(6));//to keep a square board
                Console.ResetColor();
            } else {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(message.PadLeft(6));//to keep a square board
                Console.ResetColor();
            }
        }

        public void displayTileWithPathway()
        {
            String message = row + "," + col;

            if(isPath == true) {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(message.PadLeft(6));//to keep a square board
                Console.ResetColor();
            } else if (type == 1) {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(message.PadLeft(6));//to keep a square board
                Console.ResetColor();
            } else {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(message.PadLeft(6));//to keep a square board
                Console.ResetColor();
            }
        }

        public String toString() {
            string toStr = "Node: ".PadRight(6) + (row + "_" + col).PadRight(5) + " F: ".PadRight(4) + ("" + f).PadRight(4)
                + " G: ".PadRight(4) + ("" + g).PadRight(4) + " H: ".PadRight(4) + ("" + h).PadRight(4) + " Parent: ".PadRight(9);
            if (parent == null) {
                toStr += "Null".PadRight(4);
            } else {
                toStr += ("" + parent.getRow() + "_" + parent.getCol()).PadRight(5);
            }

            return toStr;
        }

    }
}
