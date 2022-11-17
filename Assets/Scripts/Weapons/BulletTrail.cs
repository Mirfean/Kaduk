using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private float _progress;

    [SerializeField]
    private float _speed;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = new Vector3(transform.position.x, transform.position.y, -1);

    }

    // Update is called once per frame
    void Update()
    {
        _progress += Time.deltaTime * _speed;
        transform.position = Vector3.Lerp(_startPosition, _targetPosition, _progress);
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        targetPosition = new Vector3(targetPosition.x, targetPosition.y, -1);
    }
}
