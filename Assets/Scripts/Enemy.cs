using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{    
    public Animator animator;
    public EnemyBar healthBar;

    public crowdScript crowd; //call in crowd script and player
    public playerCombat player;

    private int previousPlayerHealth;
    
    public int maxHealth = 100; // Health
    public int currentHealth;   

    public float attackRange = 0.5f;
    public int attackDMG = 10; //Damage

    public float attackRate = 2f; //Rate of attack
    float nextAttackTime = 0f;

    public string nextSceneName; // Name of next scene

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth); //Sets max health
    }



    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth); //Updates health bar upon damage

        animator.SetTrigger("Hurt");

        // Play hurt animation

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Play death animation
        Debug.Log("Enemy died");

        animator.SetBool("isDead", true);
        this.enabled = false;

        // Trigger the voting system and pass in player's previous health
        crowd.StartVotingSystem(previousPlayerHealth); //Triggers to early for UI

        int nextSceneIndex = GetCurrentSceneIndex() + 1;

        switch(nextSceneIndex)
    {
        case 1:
        case 2:
        case 3:
            SceneManager.LoadScene("Level " + nextSceneIndex);
            break;
        case 4:
            SceneManager.LoadScene("Maximus");
            break;
        case 5:
            Debug.Log("Loading Good Ending scene");
            SceneManager.LoadScene("Good Ending");
            break;
        default:
            Debug.LogError("Invalid scene index: " + nextSceneIndex);
            break;
    }
    }

    int GetCurrentSceneIndex()
{
    string sceneName = SceneManager.GetActiveScene().name;
    int sceneIndex = -1;
    if (sceneName.StartsWith("Level"))
    {
        string indexString = sceneName.Substring(6); // Extract the index part after "Level "
        if (int.TryParse(indexString, out sceneIndex))
        {
            return sceneIndex;
        }
    }
    else if (sceneName == "MAXIMUS")
    {
        sceneIndex = 4; // Assuming MAXIMUS scene is considered as scene 4
    }
    else if (sceneName == "Good Ending")
    {
        sceneIndex = 5;
    }
    return sceneIndex;
}
}
