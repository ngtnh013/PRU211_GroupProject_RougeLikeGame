using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicBehaviour : MeleeWeaponBehaviour
{
    List<GameObject> markedEnemies;

    protected override void Start()
    {
        base.Start();
        markedEnemies = new List<GameObject>();
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        GameObject enemyEnity = col.gameObject;
        if(col.CompareTag("Enemy") && !markedEnemies.Contains(enemyEnity))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage(), transform.position);

            markedEnemies.Add(enemyEnity);
        }
        else if (col.CompareTag("Prop"))
        {
            if (col.gameObject.TryGetComponent(out BreakableProps breakble) && !markedEnemies.Contains(enemyEnity))
            {
                breakble.TakeDamage(GetCurrentDamage());
                markedEnemies.Add(col.gameObject);
            }
        }
    }
}
