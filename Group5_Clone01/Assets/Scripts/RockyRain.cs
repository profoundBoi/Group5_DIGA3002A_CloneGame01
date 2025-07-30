using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockyRain : MonoBehaviour
{
    public string playerTag = "Player";
    public Vector3 triggerRange = new Vector3(3f, 2f, 3f);
    public float fallDelay = 0.5f;
    public float destroyDelay = 1f;

    private Rigidbody rb;
    private bool hasFallen = false;
    private Light warningLight;

    [Header("Break Effect (Parent GameObject)")]
    public GameObject breakEffectObject; // Assign the "effects" GameObject

    private ParticleSystem breakEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        warningLight = GetComponentInChildren<Light>();
        if (warningLight != null)
            warningLight.enabled = false;

        // Get ParticleSystem from child of the breakEffectObject
        if (breakEffectObject != null)
            breakEffect = breakEffectObject.GetComponentInChildren<ParticleSystem>();

        // Disable Play On Awake manually in Inspector
        if (breakEffect != null)
            breakEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
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

    void OnCollisionEnter(Collision other)
    {
        if (!hasFallen) return;

        if (breakEffect != null)
        {
            breakEffectObject.transform.SetParent(null); // Detach so it stays in world space
            breakEffect.Play();
            Destroy(breakEffect.gameObject, breakEffect.main.duration);
        }

        Destroy(gameObject, destroyDelay);
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
