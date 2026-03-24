using System.Collections;
using UnityEngine;

public class EnemiesStatus : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 100;
    int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        //EnemyManager.instance.RegisterEnemy(gameObject); // Đăng ký với EnemyManager
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("Hurt");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy die!");
        animator.SetBool("IsDead", true);
        gameObject.SetActive(false); // Ẩn quái (không Destroy)
    }

    public void ResetEnemy()
    {
        animator.SetBool("IsDead", false);
        currentHealth = maxHealth;
        gameObject.SetActive(true);
    }
}
