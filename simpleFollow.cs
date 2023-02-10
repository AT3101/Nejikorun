using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleFollow : MonoBehaviour
{
    Vector3 diff;
    
    public GameObject target;
    public float followSpeed;

    void Start()
    {
        diff = target.transform.position - transform.position;
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            target.transform.position - diff,
            Time.deltaTime * followSpeed
        );
    }
}
