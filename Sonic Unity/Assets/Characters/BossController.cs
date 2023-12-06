using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    Vector3 target;
    float speed = 7.0f;
    // Start is called before the first frame update
    void Start()
    {
        SetNewTarget(new Vector3(
            transform.position.x + 800,
            transform.position.y,
            transform.position.z
        ));
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        
    }

    void SetNewTarget(Vector3 newTarget)
    {
        target = newTarget;
        transform.LookAt(target);
    }
}
