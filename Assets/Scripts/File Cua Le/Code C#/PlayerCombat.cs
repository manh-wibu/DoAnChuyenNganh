using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public int attackDamage = 35;
    public Animator animator;
	public Transform attackPoint;
    public float attackTarget = 0.5f;
    public LayerMask enemyLayers;
    public AudioManager audioManager;

    // Start is called before the first frame update
    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
            audioManager.SwordSlash();
		}
    }
    public void Attack()
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackTarget, enemyLayers);
		foreach (Collider2D enemy in hitEnemies)
		{
			enemy.GetComponent<EnemiesStatus>().TakeDamage(attackDamage);
		}
	}
    void OnDrawGizmosSelected()
	{
		if (attackPoint == null)
			return;
		Gizmos.DrawWireSphere(attackPoint.position, attackTarget);
	}
}
