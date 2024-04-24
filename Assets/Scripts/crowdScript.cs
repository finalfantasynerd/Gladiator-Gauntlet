using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class crowdScript : MonoBehaviour
{
    public float moveSpeed = 1f; // Speed of oscillation
    public float amplitude = 0.25f; // Maximum distance from the starting position

    public playerCombat player;
    public Enemy enemy;

    public Text voteText; // Reference to the UI Text element for displaying crowd vote
    public Text buffText; // Reference to the UI Text element for displaying applied buff

    public void StartVotingSystem(int playerHealth)
    {
        // Generate votes with a bias based on remaining player health
        int totalVotes = 25000;
        float healthBias = Mathf.Clamp01((float)playerHealth / 100f) * 0.35f; // Maximum 35% contribution from player health
        float randomBias = Random.Range(0.0f, 1.0f); // Random value between 0 and 1 for additional randomness
        float yesBias = healthBias + randomBias * 0.65f; // Randomness contributes up to 65%
        int yesVotes = Mathf.RoundToInt(totalVotes * yesBias);
        int noVotes = totalVotes - yesVotes;

        // Output the number of "yes" and "no" votes along with the bias
        Debug.Log("Total votes: " + totalVotes);
        Debug.Log("Yes votes: " + yesVotes + " (with bias: " + yesBias.ToString("P2") + ")");
        Debug.Log("No votes: " + noVotes + " (with bias: " + (1 - yesBias).ToString("P2") + ")");

        // Update UI Text element to display crowd vote
        voteText.text = "Crowd Vote: " + (yesVotes > noVotes ? "Yes" : "No");

        // Determine the outcome based on votes
        if (yesVotes > noVotes)
        {
            // Randomly choose between the options
            int choice = Random.Range(0, 3); // 0: Increase health, 1: Increase attack, 2: Decrease attack rate

            // Apply the chosen option
            switch (choice)
            {
                case 0:
                    // Increase Player health by 10
                    Debug.Log("Crowd voted to increase player health by 10.");
                    player.increaseHealth();
                    buffText.text = "Applied Buff: +10 Health";
                    break;
                case 1:
                    // Increase Attack by 10
                    Debug.Log("Crowd voted to increase player attack by 10.");
                    player.increaseDMG();
                    buffText.text = "Applied Buff: +10 Attack";
                    break;
                case 2:
                    // Decrease player attack rate by 0.25f
                    Debug.Log("Crowd voted to decrease player attack rate by 0.25f.");
                    player.increaseRate();
                    buffText.text = "Applied Buff: -0.25 Attack Rate";
                    break;
                default:
                    Debug.LogError("Invalid choice.");
                    break;
            }
        }
        else
        {
            // Randomly choose between the options
            int choice = Random.Range(0, 3); // 0: Decrease health, 1: Decrease attack, 2: Increase attack rate

            // Apply the chosen option
            switch (choice)
            {
                case 0:
                    // Decrease Player health by 10
                    Debug.Log("Crowd voted to decrease player health by 10.");
                    player.decreaseHealth();
                    buffText.text = "Applied Buff: -10 Health";
                    break;
                case 1:
                    // Decrease Attack by 5
                    Debug.Log("Crowd voted to decrease player attack by 5.");
                    player.decreaseDMG();
                    buffText.text = "Applied Buff: -5 Attack";
                    break;
                case 2:
                    // Increase player attack rate by 0.25f
                    Debug.Log("Crowd voted to increase player attack rate by 0.25f.");
                    player.decreaseRate();
                    buffText.text = "Applied Buff: +0.25 Attack Rate";
                    break;
                default:
                    Debug.LogError("Invalid choice.");
                    break;
            }
        }
    }



//-----------------------------------------------------------------------------------
    private Vector3 startPosition; // for moving the crowd

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the vertical offset based on a sine wave
        float offset = Mathf.Sin(Time.time * moveSpeed) * amplitude;

        // Apply the offset to the transform's position
        transform.position = startPosition + Vector3.up * offset;
    }
}
