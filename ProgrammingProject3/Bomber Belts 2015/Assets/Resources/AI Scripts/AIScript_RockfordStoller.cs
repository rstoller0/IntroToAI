using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIScript_RockfordStoller : MonoBehaviour {

    public CharacterScript mainScript;

    public float[] bombSpeeds;
    public float[] buttonCooldowns;
    public float playerSpeed;
    public int[] beltDirections;
    public float[] buttonLocations;
    //added
    public float[] bombDistances;//for ease of access to bomb distances
    int targetButton = 0;//for index of target bomb/belt
    float playerPressCooldownTime = 0.35f;//for checking if possible to press button on way to most dangerous bomb/belt

    //variables for bomb time heap
    //private float[] bombTimeHeap = new float[255];
    //private int bombTimeHeapSize = 0;

    // Use this for initialization
    void Start () {
        mainScript = GetComponent<CharacterScript>();

        if (mainScript == null)
        {
            print("No CharacterScript found on " + gameObject.name);
            this.enabled = false;
        }

        buttonLocations = mainScript.getButtonLocations();

        playerSpeed = mainScript.getPlayerSpeed();
	}

	// Update is called once per frame
	void Update () {

        buttonCooldowns = mainScript.getButtonCooldowns();
        beltDirections = mainScript.getBeltDirections();



        //Your AI code goes here

        //how fast the bombs move
        bombSpeeds = mainScript.getBombSpeeds();
        //how far the bomb is from exploding
        bombDistances = mainScript.getBombDistances();

        //variables
        int currentIndex = 0;
        float currentDistance = int.MaxValue;
        List<int> PossibleIndexes = new List<int>();
        int enemyBombs = 0;

        //empty bombTimeHeap
        //if(bombTimeHeapIsEmpty() == false) {
        //    for(int i = 1; i < bombTimeHeapSize + 1; i++) {
        //        removeBombTime();
        //    }
        //}

        for (int i = 0; i < beltDirections.Length; i++) {//for loop used to keep the bomb/belt index AI is currently on and to keep track of possible bombs/belts to stop
            if (Mathf.Abs(mainScript.getCharacterLocation() - buttonLocations[i]) < currentDistance) {
                currentIndex = i;
                currentDistance = Mathf.Abs(mainScript.getCharacterLocation() - buttonLocations[i]);
            }

            if (beltDirections[i] == -1 || beltDirections[i] == 0) {
                float bombTime = bombDistances[i] / bombSpeeds[i];
                float playerTime = Mathf.Abs(mainScript.getCharacterLocation() - buttonLocations[i]) / playerSpeed;

                if (playerTime < bombTime && bombTime > buttonCooldowns[i]) {
                    PossibleIndexes.Add(i);
                    //addBombTime(bombTime);
                }

                if(beltDirections[i] == -1) {//get the number of bomb/belt active for enemy
                    enemyBombs++;
                }
            }
        }

        float currentTime = int.MaxValue;//for finding most dangerous bomb/belt
        foreach (int i in PossibleIndexes) {//foreach of the possible bombs
            float bombTime = bombDistances[i] / bombSpeeds[i];

            if (currentTime > bombTime) {//if the bomb/belt being checked is more dangerous than the previous bombs/belts
                currentTime = bombTime;//set new most dangerous bomb/belt time
                targetButton = i;//set new most dangerous bomb/belt index
            }
        }
        #region testing not using push along the way
        /*
        float mostDangerousBombTime = bombDistances[targetButton] / bombSpeeds[targetButton];

        if (buttonLocations[targetButton] < 7) {//if the target is not the top bomb/belt
            if (beltDirections[targetButton + 1] != 1) {//if bomb/belt above target is not pressed by AI character
                if (mostDangerousBombTime > (Mathf.Abs(mainScript.getCharacterLocation() - buttonLocations[targetButton + 1]) / playerSpeed) + playerPressCooldownTime + (Mathf.Abs(buttonLocations[targetButton + 1] - buttonLocations[targetButton]) / playerSpeed)) {
                    //AND if there is enough time to go above the original target bomb/belt and press that button and then return to the original target bomb/belt
                    targetButton += 1;
                }
            }
        }

        if (buttonLocations[targetButton] > 0) {//if the target is not the bottom bomb/belt
            if (beltDirections[targetButton - 1] != 1) {//if bomb/belt below target is not pressed by AI character
                if (mostDangerousBombTime > (Mathf.Abs(mainScript.getCharacterLocation() - buttonLocations[targetButton - 1]) / playerSpeed) + playerPressCooldownTime + (Mathf.Abs(buttonLocations[targetButton - 1] - buttonLocations[targetButton]) / playerSpeed)) {
                    //AND if there is enough time to go below the original target bomb/belt and press that button and then return to the original target bomb/belt
                    targetButton -= 1;
                }
            }
        }
        */
        #endregion

        #region cluster test
        /*
        float bestValue = int.MaxValue;//for finding most dangerous bomb/belt
        foreach (int i in PossibleIndexes) {//foreach of the possible bombs
            float bombValue = (bombDistances[i] / bombSpeeds[i]);
            print("Row " + i + "Value " + bombValue);
            if ((i - 1) < 0 || beltDirections[i] != -1) {//if lower bomb/belt has not been pressed by enemy or does not exist add max value to cluster's heuristic
                bombValue += 10;
            } else {//else add it's value to the cluster's heuristic
                bombValue += (bombDistances[i - 1] / bombSpeeds[i - 1]);
            }

            if ((i + 1) > 7 || beltDirections[i + 1] != -1) {//if upper bomb/belt has not been pressed by enemy or does not exist add max value to cluster's heuristic
                bombValue += 10;
            } else {//else add it's value to the cluster's heuristic
                bombValue += (bombDistances[i + 1] / bombSpeeds[i + 1]);
            }
            
            if (bestValue > bombValue) {//if the bomb/belt being checked is more dangerous than the previous bombs/belts
                bestValue = bombValue;//set new most dangerous bomb/belt time
                targetButton = i;//set new most dangerous bomb/belt index
            }
        }
        */
        #endregion

        if (buttonLocations[targetButton] < mainScript.getCharacterLocation()) {//if target bomb/belt is below AI character, move down
            mainScript.moveDown();
        }
        else if (buttonLocations[targetButton] > mainScript.getCharacterLocation()) {//if target bomb/belt is above AI character, move up
            mainScript.moveUp();
        }

        //boolean used to check if the AI character can stop to press button on the way to current most dangerous bomb/belt
        bool canStopBomb = (Mathf.Abs(mainScript.getCharacterLocation() - buttonLocations[targetButton]) / playerSpeed) + playerPressCooldownTime < bombDistances[targetButton] / bombSpeeds[targetButton];
        bool correctTarget = targetButton == currentIndex;//boolean used check that the AI character is at target

        if (beltDirections[currentIndex] != 1) {//if the belt has not been pressed by AI character
            if (canStopBomb || correctTarget) {//and it can stop but still make it to the most dangerous bomb/belt or is most dangerous bomb/belt
                mainScript.push();//push the button
            }
        }


        #region simple backup code
        /*
        //variables for determining moves
        float minDistance = int.MaxValue;
        int minIndex = 0;
        float currentDistance; 
        
        //first attempt
        for (int i = 0; i < beltDirections.Length; i++) {//find the closest usabe button
            currentDistance = Mathf.Abs(buttonLocations[i] - mainScript.getCharacterLocation());
            if (buttonCooldowns[i] <= 0 && (beltDirections[i] == -1 || beltDirections[i] == 0)) {
                if (currentDistance < minDistance) {
                    minIndex = i;
                    minDistance = currentDistance;
                } else if(beltDirections[i] < beltDirections[minIndex]) {
                    minIndex = i;
                    minDistance = currentDistance;
                }
            }
        }

        //second attempt
        for (int i = 0; i < beltDirections.Length; i++) {//find the closest usabe button
            currentDistance = Mathf.Abs(buttonLocations[i] - mainScript.getCharacterLocation());
            if (buttonCooldowns[i] <= 0 && (beltDirections[i] == -1 || beltDirections[i] == 0)) {
                if (currentDistance < minDistance && beltDirections[i] <= beltDirections[minIndex]) {
                    minIndex = i;
                    minDistance = currentDistance;
                } else if (beltDirections[i] < beltDirections[minIndex]) {
                    minIndex = i;
                    minDistance = currentDistance;
                }
            }
        }

        int targetButtonIndex = minIndex;

        if(buttonLocations[targetButtonIndex] < mainScript.getCharacterLocation()) {
            mainScript.moveDown();
            mainScript.push();
        } else {
            mainScript.moveUp();
            mainScript.push();
        }
        */
        #endregion
    }

    #region heap code
    /*
    public int getBombTimeHeapSize()
    {
        return bombTimeHeapSize;
    }

    public float peekBombTimes()
    {
        return bombTimeHeap[1];
    }

    public bool bombTimeHeapIsEmpty()
    {
        return bombTimeHeapSize == 0;
    }

    public void addBombTime(float c)
    {

        //make sure the heap isnt full
        if (bombTimeHeapSize + 2 > bombTimeHeap.Length)
        {
            return;
        }

        //increase the size
        bombTimeHeapSize++;

        //add new customer to the next open position in the heap
        bombTimeHeap[bombTimeHeapSize] = c;

        //create an index variable to keep track of where our customer is in the heap
        int heapIndex = bombTimeHeapSize;

        //continue to compare the node to its parents until it is the a child of the root node
        while (heapIndex > 1)
        {

            //get parent customers index
            int parentIndex = heapIndex / 2;

            //compare new nodes F value to its parents to see if it needs to be swapped
            if (bombTimeHeap[heapIndex] < bombTimeHeap[parentIndex])
            {

                //swap bomb times
                float temp = bombTimeHeap[heapIndex];
                bombTimeHeap[heapIndex] = bombTimeHeap[parentIndex];
                bombTimeHeap[parentIndex] = temp;

                //update index to parents index after swap
                heapIndex = parentIndex;
            }
            else
            {
                //parent nodes f value is lower, no swap needed
                break;
            }
        }
    }

    public float removeBombTime()
    {

        //make sure the heap isnt empty
        if (bombTimeHeapIsEmpty())
        {
            return int.MaxValue;
        }

        //store temporary reference to root node, so we can we return it at the end
        float temp = bombTimeHeap[1];

        //move node in the last position to the root
        bombTimeHeap[1] = bombTimeHeap[bombTimeHeapSize];
        bombTimeHeap[bombTimeHeapSize] = int.MaxValue;
        bombTimeHeapSize--;

        //store the index of the node we moved to the root
        int heapIndex = 1;

        //continue to compare index nodes f value to its childrens as long as there are children
        while (heapIndex <= bombTimeHeapSize / 2)
        {

            //store index of child nodes
            int leftChildIndex = heapIndex * 2;
            int rightChildIndex = leftChildIndex + 1;

            //store f values of child nodes
            float leftChildFValue = bombTimeHeap[leftChildIndex];
            //backup: if there is no right child customer, it will always be a higher f value than the left
            float rightChildFValue = leftChildFValue + 1;

            //if there is a right child, get its actual f value
            if (rightChildIndex <= bombTimeHeapSize)
            {
                rightChildFValue = bombTimeHeap[rightChildIndex];
            }

            //determine the lower f value of the two children
            float lowerFValue;
            int lowerIndex;

            if (rightChildFValue < leftChildFValue)
            {
                lowerFValue = rightChildFValue;
                lowerIndex = rightChildIndex;
            }
            else
            {
                lowerFValue = leftChildFValue;
                lowerIndex = leftChildIndex;
            }

            //determine if a swap should be made with the parent customer and the lower priority child
            if (bombTimeHeap[heapIndex] > lowerFValue)
            {

                //swap
                float swap = bombTimeHeap[heapIndex];
                bombTimeHeap[heapIndex] = bombTimeHeap[lowerIndex];
                bombTimeHeap[lowerIndex] = swap;

                //update the index since it was moved to a child position
                heapIndex = lowerIndex;
            }
            else
            {//parent f value is lower, no need to swap
                break;
            }
        }

        //return the original root customer
        return temp;
    }

    public void resortBombTimeHeap(float n)
    {

        int heapIndex = searchForBombTimeHeapIndex(n);

        //continue to compare the node to its parents until it is the a child of the root node
        while (heapIndex > 1)
        {

            //get parent customers index
            int parentIndex = heapIndex / 2;

            //compare new nodes F value to its parents to see if it needs to be swapped
            if (bombTimeHeap[heapIndex] < bombTimeHeap[parentIndex])
            {

                //swap customers
                float temp = bombTimeHeap[heapIndex];
                bombTimeHeap[heapIndex] = bombTimeHeap[parentIndex];
                bombTimeHeap[parentIndex] = temp;

                //update index to parents index after swap
                heapIndex = parentIndex;
            }
            else
            {
                //parent nodes f value is lower, no swap needed
                break;
            }
        }
    }

    private int searchForBombTimeHeapIndex(float nd)
    {

        int indexFound = 0;

        for (int i = 1; i <= bombTimeHeapSize; i++)
        {
            if (bombTimeHeap[i] == nd)
            {
                indexFound = i;
            }
            else
            {
                indexFound = 0;
            }
        }

        return indexFound;
    }

    public bool search(float nd)
    {
        bool wasFound = false;

        for (int i = 1; i <= bombTimeHeapSize; i++)
        {
            if (bombTimeHeap[i] == nd)
            {
                wasFound = true;
            }
        }

        return wasFound;
    }
    */
    #endregion
}
