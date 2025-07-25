using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChain : MonoBehaviour
{
    public float maxDistance = 3f;  
    public float springForce = 1000f; 
    public float damper = 50f; 

    [Header("Chain Visual")]
    public Color chainColor = Color.black;
    public float chainWidth = 0.1f;

    private Rigidbody rb;
    private Rigidbody otherRb;
    private ConfigurableJoint joint;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null) lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = chainWidth;
        lineRenderer.endWidth = chainWidth;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = chainColor;
        lineRenderer.endColor = chainColor;
        lineRenderer.enabled = false;
    }

    private void Start()
    {
        StartCoroutine(WaitForOtherPlayerAndConnect());
    }

    IEnumerator WaitForOtherPlayerAndConnect()
    {
        while (GameObject.FindGameObjectsWithTag("Player").Length < 2)
            yield return null;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            if (player != gameObject)
            {
                otherRb = player.GetComponent<Rigidbody>();
                break;
            }
        }

        if (otherRb == null)
        {
            Debug.LogError("Other player not found!");
            yield break;
        }

        joint = gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = otherRb;

        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = Vector3.zero;
        joint.connectedAnchor = Vector3.zero;

        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;

        SoftJointLimitSpring limitSpring = new SoftJointLimitSpring
        {
            spring = springForce,
            damper = damper
        };
        joint.linearLimitSpring = limitSpring;

        SoftJointLimit limit = new SoftJointLimit
        {
            limit = maxDistance
        };
        joint.linearLimit = limit;

        joint.angularXMotion = ConfigurableJointMotion.Locked;
        joint.angularYMotion = ConfigurableJointMotion.Locked;
        joint.angularZMotion = ConfigurableJointMotion.Locked;

        lineRenderer.enabled = true;

        Debug.Log($"{gameObject.name} connected to {otherRb.gameObject.name} with clamping.");
    }

    private void Update()
    {
        if (lineRenderer.enabled && otherRb != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, otherRb.position);
        }
    }
}
