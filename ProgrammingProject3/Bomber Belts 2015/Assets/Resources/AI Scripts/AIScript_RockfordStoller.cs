using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AIScript_RockfordStoller : MonoBehaviour {

    public CharacterScript mainScript;

    public float[] bombSpeeds;
    public float[] buttonCooldowns;
    public float playerSpeed;
    public int[] beltDirections;
    public float[] buttonLocations;
    //added
    public float[] bombDistances;//for ease of access to bomb distances
    public int targetButton = 0;//for index of target bomb/belt
    public float playerPressCooldownTime = 0.35f;//for checking if possible to press button on way to most dangerous bomb/belt
    public float[] beltValues;//array for heuristics
    public float[] buttonToAIDistances;//array for holding the distances from button to AI character

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

        bombSpeeds = mainScript.getBombSpeeds();//how fast the bombs move
        bombDistances = mainScript.getBombDistances();//how far the bomb is from exploding
        beltValues = new float[beltDirections.Length];//set length of bombValue array
        buttonToAIDistances = new float[beltDirections.Length];//set length of buttonToAIDistances array

        //variables
        for (int i = 0; i < buttonToAIDistances.Length; i++) {//for each belt get distances between buttons and AI character (for calculations)
            buttonToAIDistances[i] = Mathf.Abs(buttonLocations[i] - mainScript.getCharacterLocation()) / 2;
        }

        int currentIndex = 0;
        float currentDistance = int.MaxValue;
        for (int i = 0; i < beltValues.Length; i++) {
            if (buttonToAIDistances[i] < currentDistance) {//calculate what the index of AI character location is
                currentIndex = i;
                currentDistance = buttonToAIDistances[i];
            }
        }

        calculateBeltValues();//calculate heuristic values for each belt

        float topBeltValue = -int.MaxValue;
        for (int i = 0; i < beltValues.Length; i++) {
            if (topBeltValue < beltValues[i]) {
                topBeltValue = beltValues[i];
                targetButton = i;
            }
        }

        bool onTargetButton = targetButton == currentIndex;//boolean used check that the AI character is at target

        if (buttonLocations[targetButton] <= mainScript.getCharacterLocation()) {//if target bomb/belt is below AI character, move down
            if (onTargetButton) {
                mainScript.push();
            }
            mainScript.moveDown();
        }
        else if (buttonLocations[targetButton] >= mainScript.getCharacterLocation()) {//if target bomb/belt is above AI character, move up
            if (onTargetButton) {
                mainScript.push();
            }
            mainScript.moveUp();
        }

        //print(targetButton);
    }

    void calculateBeltValues()
    {
        float value = 0;
        for (int i = 0; i < beltValues.Length; i++) {//for each bomb/belt
            float bombTime = bombDistances[i] / bombSpeeds[i];

            if (bombTime < buttonCooldowns[i]) {//if bomb will explode before button's cooldown is finished
                value -= 50;//give up on that bomb
            }

            if (bombSpeeds[i] >= 4 && beltDirections[i] == -1) {//if the bomb speed is > 4 and it's moving toward AI character's side
                value += 5;
            }

            if (beltDirections[i] == -1) {//if the bomb is moving to AI character's side
                value += (12 - (bombDistances[i] / 2));//the closer the bomb to AI character's side, the higher priority (higher value)
            }

            if (beltDirections[i] == 0) {//if the bomb is sitting still
                value += 2;
            }

            if (buttonCooldowns[i] >= 0.8f) {//if the button is on cooldown
                value -= 3;
            }

            value /= (buttonToAIDistances[i] / 3);//use the distance between the button and the AI character, farther distance = bigger number = lower score

            beltValues[i] = value;
            value = 0;
        }
    }
}
