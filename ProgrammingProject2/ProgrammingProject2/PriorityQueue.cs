using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingProject2
{
    public class PriorityQueue
    {

        private Node[] heap;
        private int size;

        public PriorityQueue() {
            heap = new Node[255];
            size = 0;
        }

        public PriorityQueue(int s) {
            heap = new Node[s];
            size = 0;
        }

        public int getSize() {
            return size;
        }

        public Node peek() {
            return heap[1];
        }

        public bool isEmpty() {
            return size == 0;
        }

        public void add(Node c) {

            //make sure the heap isnt full
            if (size + 2 > heap.Length) {
                Console.WriteLine("The heap is full");
                return;
            }

            //increase the size
            size++;

            //add new customer to the next open position in the heap
            heap[size] = c;

            //create an index variable to keep track of where our customer is in the heap
            int index = size;

            //continue to compare the node to its parents until it is the a child of the root node
            while (index > 1) {

                //get parent customers index
                int parentIndex = index / 2;

                //compare new nodes F value to its parents to see if it needs to be swapped
                if (heap[index].getF() < heap[parentIndex].getF()) {

                    //swap customers
                    Node temp = heap[index];
                    heap[index] = heap[parentIndex];
                    heap[parentIndex] = temp;

                    //update index to parents index after swap
                    index = parentIndex;
                } else {
                    //parent nodes f value is lower, no swap needed
                    break;
                }
            }
        }

        public Node remove() {

            //make sure the heap isnt empty
            if (isEmpty()) {
                Console.WriteLine("The heap is already empty");
                return null;
            }

            //store temporary reference to root node, so we can we return it at the end
            Node temp = heap[1];

            //move node in the last position to the root
            heap[1] = heap[size];
            heap[size] = null;
            size--;

            //store the index of the node we moved to the root
            int index = 1;

            //continue to compare index nodes f value to its childrens as long as there are children
            while (index <= size / 2) {

                //store index of child nodes
                int leftChildIndex = index * 2;
                int rightChildIndex = leftChildIndex + 1;

                //store f values of child nodes
                int leftChildFValue = heap[leftChildIndex].getF();
                //backup: if there is no right child customer, it will always be a higher f value than the left
                int rightChildFValue = leftChildFValue + 1;

                //if there is a right child, get its actual f value
                if (rightChildIndex <= size) {
                    rightChildFValue = heap[rightChildIndex].getF();
                }

                //determine the lower f value of the two children
                int lowerFValue;
                int lowerIndex;

                if (rightChildFValue < leftChildFValue) {
                    lowerFValue = rightChildFValue;
                    lowerIndex = rightChildIndex;
                } else {
                    lowerFValue = leftChildFValue;
                    lowerIndex = leftChildIndex;
                }

                //determine if a swap should be made with the parent customer and the lower priority child
                if (heap[index].getF() > lowerFValue) {

                    //swap
                    Node swap = heap[index];
                    heap[index] = heap[lowerIndex];
                    heap[lowerIndex] = swap;

                    //update the index since it was moved to a child position
                    index = lowerIndex;
                } else {//parent f value is lower, no need to swap
                    break;
                }
            }

            //return the original root customer
            return temp;
        }

        public void resort(Node n) {

            int index = searchForIndex(n);
            
            //continue to compare the node to its parents until it is the a child of the root node
            while (index > 1)
            {

                //get parent customers index
                int parentIndex = index / 2;

                //compare new nodes F value to its parents to see if it needs to be swapped
                if (heap[index].getF() < heap[parentIndex].getF())
                {

                    //swap customers
                    Node temp = heap[index];
                    heap[index] = heap[parentIndex];
                    heap[parentIndex] = temp;

                    //update index to parents index after swap
                    index = parentIndex;
                }
                else
                {
                    //parent nodes f value is lower, no swap needed
                    break;
                }
            }
        }

        private int searchForIndex(Node nd) {

            int indexFound = 0;

            for (int i = 1; i <= size; i++) {
                if (heap[i] == nd) {
                    indexFound = i;
                } else {
                    indexFound = 0;
                }
            }

            return indexFound;
        }

        public bool search(Node nd) {
            bool wasFound = false;

            for (int i = 1; i <= size; i++) {
                if (heap[i] == nd) {
                    wasFound = true;
                }
            }

            return wasFound;
        }
    }
}
