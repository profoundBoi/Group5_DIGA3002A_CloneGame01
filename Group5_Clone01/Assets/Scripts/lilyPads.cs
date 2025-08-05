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
    private int playerCount = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        rb.isKinematic = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerCount++;

            if (!isTriggered)
            {
                if (playerCount >= 2)
                {
                    isTriggered = true;
                    StopAllCoroutines();
                    StartCoroutine(FallImmediately());
                }
                else
                {
                    isTriggered = true;
                    StartCoroutine(ShakeAndFall());
                }
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerCount = Mathf.Max(0, playerCount - 1);
        }
    }

    IEnumerator ShakeAndFall()
    {
        float elapsed = 0f;
        Vector3 startPos = originalPosition;

        while (elapsed < delayBeforeFall)
        {
            // If a second player jumps on during shake, fall immediately
            if (playerCount >= 2)
            {
                StartCoroutine(FallImmediately());
                yield break;
            }

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

    IEnumerator FallImmediately()
    {
        transform.position = originalPosition;
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
        playerCount = 0;
    }
}
