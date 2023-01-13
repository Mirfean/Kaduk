using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] bool _active;

    public Door[] Doors;
    public List<Enemy> Enemies;

    float _timer;

    public bool Active { get => _active; set => _active = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RandomPlaceEnemy(Enemy enemy)
    {
        enemy.gameObject.GetComponent<EnemyMovement>().GoToRandomPoint();
    }

    void ActivateEnemy(Enemy enemy, bool state)
    {
        enemy.gameObject.GetComponent<EnemyMovement>().Active = state;
    }

    void RandomPlacementAllEnemies()
    {
        foreach (Enemy enemy in Enemies)
        {
            RandomPlaceEnemy(enemy);
        }
    }

    public void ActiveAllEnemies(bool state)
    {
        foreach (Enemy enemy in Enemies)
        {
            ActivateEnemy(enemy, state);
        }
    }

}
