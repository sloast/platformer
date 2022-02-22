using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 target;
    public float cameraSpeed = 5;

    void Start()
    {
        target = new Vector3(-.04f, .45f, -10f);
    }

    
    void Update()
    {
        if ((target - transform.position).magnitude > .01f){
            Approach(target);
        }
    }

    void Approach(Vector3 t)
    {
        Vector3 pos = Vector3.Lerp(transform.position, target, cameraSpeed*Time.deltaTime);
        pos.z = -10f;
        transform.position = pos;
    }

    public void SetTarget(Vector3 newTarget)
    {
        target = newTarget;
    }
}
