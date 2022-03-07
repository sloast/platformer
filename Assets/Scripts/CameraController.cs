using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 target;
    Vector3 currPos;
    public float cameraSpeed = 5;
    

    float shakeIntensity = 1f;
    float shakeDuration = 0f;

    float decreasingShakeDuration = 0f;
    float shakeMult = 1f;

    void Start()
    {
        target = new Vector3(-.04f, .45f, -10f);
    }

    
    void Update()
    {
        if ((target - currPos).magnitude > .01f){
            Approach(target);
        }
        if (shakeDuration > 0f)
        {
            shakeDuration -= Time.deltaTime;
            transform.position = currPos + shakeIntensity * Random.insideUnitSphere;
        } else if (decreasingShakeDuration > 0f)
        {
            decreasingShakeDuration -= Time.deltaTime;
            transform.position = currPos + decreasingShakeDuration * shakeMult * Random.insideUnitSphere;
        } else {
            transform.position = currPos;
        }
    }

    void Approach(Vector3 t)
    {
        Vector3 pos = Vector3.Lerp(currPos, target, cameraSpeed*Time.deltaTime);
        pos.z = -10f;
        currPos = pos;
    }

    public void SetTarget(Vector3 newTarget)
    {
        target = newTarget;
    }

    public void ShakeScreen(float speed, float duration)
    {
        shakeIntensity = speed;
        shakeDuration = duration;
    }

    public void ShakeScreenDecreasing(float duration, float mult)
    {
        decreasingShakeDuration = duration;
        shakeMult = mult;
    }
}
