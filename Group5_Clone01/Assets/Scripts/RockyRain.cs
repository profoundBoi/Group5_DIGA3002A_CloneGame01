using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockyRain : MonoBehaviour
{
    public string playerTag = "Player";     
    public Vector3 triggerRange = new Vector3(3f, 2f, 3f);
    public float fallDelay = 0.5f;

    private Rigidbody rb;
    private bool hasFallen = false;
    private Light warningLight;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        warningLight = GetComponentInChildren<Light>();
        if (warningLight != null)
            warningLight.enabled = false;
    }

    void Update()
    {
        if (hasFallen) return;

        Collider[] hits = Physics.OverlapBox(transform.position, triggerRange * 0.5f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag(playerTag))
            {
                hasFallen = true;
                if (warningLight != null)
                    warningLight.enabled = true;
                Invoke(nameof(Fall), fallDelay);
                break;
            }
        }
    }

    void Fall()
    {
        rb.isKinematic = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, triggerRange);
    }
}
