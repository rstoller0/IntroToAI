using UnityEngine;
using System.Collections;
using System;
using System.Linq;

/* Byron Park USED FOR REFERENCE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
 * Bomber Belts
 */

public class AI_BPark : MonoBehaviour
{

    public CharacterScript mainScript;

    public float[] bombSpeeds;
    public float[] buttonCooldowns;
    public float playerSpeed;
    public int[] bombDirections;
    public float[] buttonLocations;
    public float characterLocation;
    public float[] distancesFromPlayerToButton = new float[8];
    public float[] bombDistances;
    public float[] scores = new float[8];
    public float score;
    public int indexOfBestAction;

    // Use this for initialization
    void Start()
    {
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
    void Update()
    {

        buttonCooldowns = mainScript.getButtonCooldowns();
        bombDirections = mainScript.getBeltDirections();
        bombSpeeds = mainScript.getBombSpeeds();
        bombDistances = mainScript.getBombDistances();
        characterLocation = mainScript.getCharacterLocation();
        getPlayerDistanceFromButton();  //	gets distance from player to each button
        getScores();    //	assigns a score to each belt

        indexOfBestAction = Array.IndexOf(scores, scores.Max());    //	get the index of the belt with highest priority
        if (buttonLocations[indexOfBestAction] + .1 >= characterLocation)
        {
            if (distancesFromPlayerToButton[indexOfBestAction] <= .8)   //	only push it if it's the button the player wants to push
                mainScript.push();
            mainScript.moveUp();
        }
        else if (buttonLocations[indexOfBestAction] - .1 <= characterLocation)
        {
            if (distancesFromPlayerToButton[indexOfBestAction] <= .8)
                mainScript.push();
            mainScript.moveDown();
        }

    }

    void getPlayerDistanceFromButton()
    {
        for (int i = 0; i < 8; i++)
        {   //	for each belt
            distancesFromPlayerToButton[i] = Math.Abs(buttonLocations[i] - characterLocation) / 2;  //	get distances
        }
    }

    //	score each belt based on predetermined scoring system
    void getScores()
    {
        score = 0;
        for (int i = 0; i < 8; i++)
        {   //	for each button
            if (bombSpeeds[i] >= 4 && bombDirections[i] == -1)  //	the bomb speed is > 4 and it's moving toward your side
                score += 5;
            if (bombDirections[i] == -1)
                score += (12 - bombDistances[i] / 2);   //	closer the bomb to your side, the higher priority the best is (higher score)
            if (bombDirections[i] == 0)
                score += 2;
            if (buttonCooldowns[i] >= .8)
                score -= 3;
            score /= (distancesFromPlayerToButton[i] / 3);  //	take into consideration the distance from the player to the button. Farther distance = bigger number = lower score

            scores[i] = score;
            score = 0;
        }
    }
}