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
    public float[] bombDistances;
    int targetButton = 0;

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

        for (int i = 0; i < beltDirections.Length; i++) {
            if (Mathf.Abs(mainScript.getCharacterLocation() - buttonLocations[i]) < currentDistance) {
                currentIndex = i;
                currentDistance = Mathf.Abs(mainScript.getCharacterLocation() - buttonLocations[i]);
            }

            if (beltDirections[i] == -1 || beltDirections[i] == 0) {
                float bombTime = bombDistances[i] / bombSpeeds[i];
                float playerTime = Mathf.Abs(mainScript.getCharacterLocation() - buttonLocations[i]) / playerSpeed;

                if (playerTime < bombTime && bombTime > buttonCooldowns[i]) {
                    PossibleIndexes.Add(i);
                }
            }
        }

        float currentTime = Mathf.Infinity;
        foreach (int i in PossibleIndexes)
        {
            float bombTime = bombDistances[i] / bombSpeeds[i];

            if (currentTime > bombTime)
            {
                currentTime = bombTime;
                targetButton = i;
            }
        }

        if (buttonLocations[targetButton] < mainScript.getCharacterLocation()) {
            mainScript.moveDown();
        }
        else if (buttonLocations[targetButton] > mainScript.getCharacterLocation()) {
            mainScript.moveUp();
        }

        bool canMakeIt = (Mathf.Abs(mainScript.getCharacterLocation() - buttonLocations[targetButton]) / playerSpeed) + 0.35f < bombDistances[targetButton] / bombSpeeds[targetButton];
        bool onTarget = targetButton == currentIndex;

        if (beltDirections[currentIndex] != 1) {
            if (canMakeIt || onTarget) {
                mainScript.push();
            }
        }


        //simple backup code
        #region
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
}
