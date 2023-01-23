using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlarmer : MonoBehaviour
{
    [SerializeField] Collider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 13)
        {
            collision.gameObject.transform.parent.GetComponent<EnemyMovement>().SetHunt();
        }
    }
}
