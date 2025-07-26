using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lilyPads : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    [Header("Timing Settings")]
    public float delayBeforeFall = 2f; 
    public float resetTime = 10f;     

    [Header("Shake Settings")]
    public float shakeMagnitude = 0.1f;
    public float shakeSpeed = 20f;

    private bool isTriggered = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        rb.isKinematic = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isTriggered && collision.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
            StartCoroutine(ShakeAndFall());
        }
    }

    IEnumerator ShakeAndFall()
    {
        float elapsed = 0f;

        Vector3 startPos = originalPosition;

        while (elapsed < delayBeforeFall)
        {
            float x = Mathf.Sin(Time.time * shakeSpeed) * shakeMagnitude;
            float z = Mathf.Cos(Time.time * shakeSpeed) * shakeMagnitude;

            transform.position = startPos + new Vector3(x, 0f, z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = startPos;
        rb.isKinematic = false;

        yield return new WaitForSeconds(resetTime);
        ResetPlatform();
    }

    void ResetPlatform()
    {
        rb.isKinematic = true;
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        isTriggered = false;
    }
}
