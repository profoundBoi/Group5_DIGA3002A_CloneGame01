using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChain : MonoBehaviour
{
    public Rigidbody otherPlayer;
    public float maxDistance = 3f;
    public float springForce = 50f;
    public float damper = 5f;

    [Header("Chain Visual")]
    public Color chainColor = Color.black;
    public float chainWidth = 0.1f;

    private ConfigurableJoint joint;
    private LineRenderer lineRenderer;

    void Start()
    {
        
        joint = gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = otherPlayer;

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

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = chainWidth;
        lineRenderer.endWidth = chainWidth;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = chainColor;
        lineRenderer.endColor = chainColor;
    }

    void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, otherPlayer.position);
    }
}
