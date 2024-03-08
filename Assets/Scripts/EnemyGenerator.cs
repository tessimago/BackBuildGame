using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject[] prefabs;
    public Enemy[] enemyTypes;
    void Start(){
        enemyTypes = new Enemy[]{
            new Enemy_Standard(prefabs[0]),
            new Enemy_Fast(prefabs[0]),
            new Enemy_Strong(prefabs[0]),
        };
    }

    public void spawnRandomEnemy(Vector3Int coords){
        Debug.Log("Spawning... " + enemyTypes.Length);
        spawnEnemy(Random.Range(0, enemyTypes.Length), coords);
    }
    public void spawnEnemy(int type, Vector3Int coords){
        Debug.Log("Spawning2...");
        var e = Instantiate(enemyTypes[type].enemy, coords, Quaternion.identity);
        Debug.Log("Spawning3... e: " + e);
        setStats(e, type);
    }

    void setStats(GameObject e, int type){
        e.GetComponent<EnemyStats>().initialize(enemyTypes[type].maxHealth);
        e.GetComponent<EnemyBehaviour>().damage = enemyTypes[type].damage;
        e.GetComponent<EnemyBehaviour>().speed = enemyTypes[type].speed;
        e.GetComponent<SpriteRenderer>().color = enemyTypes[type].color;
    }
}

public abstract class Enemy
{
    public GameObject enemy;
    public int maxHealth;
    public float speed;
    public int damage;
    public Color color;

}

public class Enemy_Standard : Enemy
{
    public Enemy_Standard(GameObject enemyPrefab)
    {
        enemy = enemyPrefab;
        maxHealth = 100;
        speed = 5;
        damage = 10;
        color = Color.white;
    }
}

public class Enemy_Fast : Enemy
{
    public Enemy_Fast(GameObject enemyPrefab)
    {
        enemy = enemyPrefab;
        maxHealth = 50;
        speed = 10;
        damage = 5;
        color = Color.yellow;
    }
}

public class Enemy_Strong : Enemy
{
    public Enemy_Strong(GameObject enemyPrefab)
    {
        enemy = enemyPrefab;
        maxHealth = 250;
        speed = 3;
        damage = 25;
        color = Color.red;
    }
}