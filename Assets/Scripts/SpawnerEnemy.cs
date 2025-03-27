using System.Collections;
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour, IDamage
{
    [SerializeField] int hp;
    [SerializeField] GameObject deathEffect;
    [SerializeField] GameObject enemyToSpawn;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float spawnTime;
    float currentSpawnTime;
    Vector3 point;
    bool spawn;

    void Start()
    {
        GameManager.instance.UpdateGameGoal(1);
        currentSpawnTime = spawnTime;
    }

    public void HealDamage(int amount)
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            spawn = false;
            StartCoroutine(SpawnEffect());
            GameManager.instance.UpdateGameGoal(-1);
            Destroy(gameObject, 8 * 0.26f);
        }
    }

    IEnumerator SpawnEffect()
    {
        for(int i = 0;i < 9;i++)
        {
            var x = transform.position.x;
            var y = transform.position.y;  
            var z = transform.position.z;
            point = new Vector3(
            Random.Range(-1.5f, 1.5f) + x,
            Random.Range(0f, 1.5f) + y,
            Random.Range(-1.5f, 1.5f) + z);
            Instantiate(deathEffect, point, Quaternion.identity);
            yield return new WaitForSeconds(0.25f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(spawn)
        {
            currentSpawnTime += Time.deltaTime;
            if(currentSpawnTime >= spawnTime)
            {
                Instantiate(enemyToSpawn, spawnPoint.position, spawnPoint.rotation);
                currentSpawnTime = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            spawn = true;
    }
}
