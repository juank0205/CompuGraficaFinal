using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapling : MonoBehaviour {
    private PlayerMovement pm;
    public Camera cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    private Animator animator;
    public LineRenderer lr;

    public float maxGrappleDistance;
    public float grappleDelayTime;
    private Vector3 grapplePoint;

    public float grapplingCd;
    private float grapplingCdTimer;

    private bool grappling;
    public float overshootYAxis;

    private void Start() {
        GetReferences();
    }

    private void GetReferences() {
        pm = GetComponent<PlayerMovement>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update() {
        if (Input.GetButton("Fire2")) StartGrapple();
        if (grapplingCdTimer > 0) {
            grapplingCdTimer -= Time.deltaTime;
        }
    }

    private void LateUpdate() {
        if (grappling)
            lr.SetPosition(0, gunTip.position);
    }

    private void StartGrapple() {
        if (grapplingCdTimer > 0) return;
        animator.SetInteger("Grapple", 1);
        grappling = true;
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxGrappleDistance, whatIsGrappleable)) {
            grapplePoint = hit.point;
            ExecuteGrapple();
        } else {
            grapplePoint = cam.transform.position + cam.transform.forward * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }

    private void ExecuteGrapple() {
        pm.Grappling = true;
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;
        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        pm.JumpToPosition(grapplePoint, highestPointOnArc);
        Invoke(nameof(StopGrapple), grappleDelayTime);
    }

    private void StopGrapple() {
        grappling = false;
        pm.Grappling = false;
        grapplingCdTimer = grapplingCd;
        lr.enabled = false;
        animator.SetInteger("Grapple", 0);
    }
}
