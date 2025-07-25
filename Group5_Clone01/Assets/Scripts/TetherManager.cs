using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class TetherManager : MonoBehaviour
{
    public float maxTetherDistance = 5f;
    public float springForce = 10f;

    [HideInInspector] public Rigidbody rb;
    public static List<TetherManager> AllPlayers = new List<TetherManager>();

    private LineRenderer lineRenderer;
    private TetherManager nearestOther;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();

        if (!AllPlayers.Contains(this))
            AllPlayers.Add(this);
    }

    private void OnDisable()
    {
        AllPlayers.Remove(this);
    }

    private void FixedUpdate()
    {
        if (rb == null || AllPlayers.Count < 2) return;

        TetherManager closest = null;
        float closestDist = float.MaxValue;

        foreach (var other in AllPlayers)
        {
            if (other == this || other.rb == null) continue;

            float dist = Vector3.Distance(transform.position, other.transform.position);
            if (dist < closestDist)
            {
                closest = other;
                closestDist = dist;
            }

            // Tether logic
            if (dist > maxTetherDistance)
            {
                Vector3 pullDir = (other.transform.position - transform.position).normalized;
                float stretch = dist - maxTetherDistance;

                Vector3 force = pullDir * (stretch * springForce);
                rb.AddForce(force * 0.5f, ForceMode.Force);
                other.rb.AddForce(-force * 0.5f, ForceMode.Force);
            }
        }

        // Only draw line to the closest other player (1 tether per player)
        if (closest != null)
        {
            nearestOther = closest;
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, closest.transform.position);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}
