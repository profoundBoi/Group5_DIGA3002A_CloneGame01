using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TetherManager : MonoBehaviour
{
    [Header("Tether Settings")]
    public float maxTetherDistance = 5f;
    public float springForce = 10f;

    [Header("Struggle Settings")]
    public float struggleDuration = 5f;
    public float strongPullForce = 50f;
    private float struggleTimer;
    private bool isStruggling;

    [Header("Struggle UI")]
    public RawImage[] countdownImages;
    private Coroutine countdownCoroutine;

    [HideInInspector] public Rigidbody rb;
    public static List<TetherManager> AllPlayers = new List<TetherManager>();

    private LineRenderer lineRenderer;
    private TetherManager nearestOther;

    private void Start()
    {
        ResetCountdownUI();
    }
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

            if (dist > maxTetherDistance)
            {
                if (!isStruggling)
                {
                    isStruggling = true;
                    struggleTimer = struggleDuration;

                    if (countdownCoroutine != null)
                        StopCoroutine(countdownCoroutine);

                    countdownCoroutine = StartCoroutine(StartCountdown());
                }

                Vector3 pullDir = (other.transform.position - transform.position).normalized;
                float stretch = dist - maxTetherDistance;
                Vector3 force = pullDir * (stretch * springForce);

                rb.AddForce(force * 0.5f, ForceMode.Force);
                other.rb.AddForce(-force * 0.5f, ForceMode.Force);
            }
            else
            {
                if (isStruggling)
                {
                    isStruggling = false;

                    if (countdownCoroutine != null)
                        StopCoroutine(countdownCoroutine);

                    ResetCountdownUI();
                }
            }
        }

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

    private void Update()
    {
        if (isStruggling)
        {
            struggleTimer -= Time.deltaTime;

            if (struggleTimer <= 0f)
            {
                isStruggling = false;
                ApplyStrongPull();
                ResetCountdownUI();
            }
        }
    }

    private void ApplyStrongPull()
    {
        if (nearestOther == null) return;

        Vector3 pullDir = (nearestOther.transform.position - transform.position).normalized;
        Vector3 force = pullDir * strongPullForce;

        rb.AddForce(force, ForceMode.Impulse);
        nearestOther.rb.AddForce(-force, ForceMode.Impulse);
    }

    private IEnumerator StartCountdown()
    {
        ResetCountdownUI();

        int steps = countdownImages.Length;
        float interval = struggleDuration / steps;

        for (int i = steps - 1; i >= 0; i--)
        {
            countdownImages[i].enabled = true;
            yield return new WaitForSeconds(interval);
            countdownImages[i].enabled = false;
        }
    }

    private void ResetCountdownUI()
    {
        foreach (var img in countdownImages)
        {
            if (img != null)
                img.enabled = false;
        }
    }
}
