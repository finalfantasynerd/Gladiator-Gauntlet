using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public HealthBar healthBar; //health bar ui
    

    public AudioSource swordHitPrefab; // Sound clip for sword hit
    private AudioSource swordHit; // Instance of sword hit audio source

    public static int maxHealth = 100; //Health
    public int currentHealth;
    
    public float attackRange = 0.5f;
    public static int attackDMG = 10; //Damage

    public static float attackRate = 2f; //Rate of attack
    float nextAttackTime = 0f;

    public string badEndingScene = "Bad Ending";

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth); //Sets max health

        // Instantiate the AudioSource from the prefab
        swordHit = Instantiate(swordHitPrefab, transform.position, Quaternion.identity);
        swordHit.transform.parent = transform; // Make the audio source a child of the player
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) //testing health bar
        {
            TakeDamage(20);
        }

        if(Time.time >= nextAttackTime){
            if (Input.GetKeyDown(KeyCode.Mouse0)){
                Attack();
                nextAttackTime = Time.time + 1f / attackRate; //adds delay
            }
        }
    }

    void Attack(){
        // Play an attack animation
        animator.SetTrigger("Attack");
        
        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage them
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDMG);
            // Play sword hit sound
            swordHit.PlayOneShot(swordHit.clip);
        }
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
        Debug.Log("Player died");

        animator.SetBool("isDead", true);

        // Disable this script
        this.enabled = false;

        // Load bad ending scene
        SceneManager.LoadScene(badEndingScene);
    }
    
    void OnDrawGizmosSelected(){ // helps verify attack range
        
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void increaseHealth()
    {
        maxHealth += 10;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void increaseDMG()
    {
        attackDMG += 10;
    }
    
    public void increaseRate()
    {
        attackRate += 0.25f;
        
    }

    public void decreaseHealth()
    {
        maxHealth -= 10;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void decreaseDMG()
    {
        attackDMG -= 5;
    }
    
    public void decreaseRate()
    {
        attackRate -= 0.25f;
        
    }
}
