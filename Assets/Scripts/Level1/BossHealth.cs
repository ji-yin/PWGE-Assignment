using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
	public int health = 100;

	//public GameObject deathEffect;

	//public bool isInvulnerable = false;
	void Update()
	{
		if (health <= 200)
		{
			GetComponent<Animator>().SetInteger("IsAngry", 1);
		}

		if (health <= 0)
		{
			Die();
		}
	}
	public void TakeDamage(int damage)
	{
		//if (isInvulnerable)
		//	return;

		health -= damage;
	}

	void Die()
	{
		GetComponent<Animator>().SetBool("IsDying", true);
		//Destroy(gameObject);
	}

}
