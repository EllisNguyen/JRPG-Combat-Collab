using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityCheck : MonoBehaviour
{
    public List<EnemyEntity> enemyEntities;

    // Start is called before the first frame update
    void Start()
    {

        enemyEntities = new List<EnemyEntity>();
        var eEntity = FindObjectsOfType<EnemyEntity>();

        foreach (EnemyEntity enemy in eEntity)
        {
            enemyEntities.Add(enemy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < enemyEntities.Count; i++)
        {
            if (enemyEntities[i] == null) enemyEntities.RemoveAt(i);
        }

        if(enemyEntities.Count == 0)
        {
            print("hello");
        }
    }
}
