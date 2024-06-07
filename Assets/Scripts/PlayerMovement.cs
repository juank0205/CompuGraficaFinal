using Palmmedia.ReportGenerator.Core.CodeAnalysis;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerMovement : MonoBehaviour {
    public Transform playerCam;
    public Transform orientation;
    public GameObject hands;

    private Rigidbody rb;
    private PlayerStats stats;

    //Rotation and look 
    private float xRotation;
    public float sensitivity = 50f;
    public float sensMultiplier = 1f;

    //Movement 
    public float moveSpeed = 4500;
    public float maxSpeed = 20;
    private bool grounded;
    private float threshold = 0.01f;
    public LayerMask whatIsGround;
    public float heightLimit = -50f;

    public float counterMovement = 0.175f;
    public float maxSlopeAngle = 35f;

    //Crouch & Slide
    public float crouchHeightChange = 0.5f;
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;
    private Vector3 handsScale;
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;
    private bool sliding;
    private float slidingCooldown = 0.75f;
    private float crouchMultiplier = 0.4f;

    //Jumping
    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;
    public float jumpForce = 550f;

    //Grapple
    private bool grappling = false;

    //Input
    float x, y;
    bool jumping, sprinting, crouching;

    //Sliding
    private Vector3 normalVector = Vector3.up;
    private Vector3 wallNormalVector;

    //Animations
    private Animator animator;

    private void Start() {
        GetReferences();
        playerScale = transform.localScale;
        handsScale = new Vector3(crouchScale.x, crouchScale.y * 4, crouchScale.z);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void GetReferences()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        stats = GetComponent<PlayerStats>();
    }

    private void FixedUpdate() {
        Movement();
        HandleAnimations();
    }

    private void Update() {
        if (stats.IsDead()) return;
        getInput();
        Look();
        CheckHeight();
    }

    private void CheckHeight() {
        if (transform.position.y <= heightLimit)
            stats.Die();
    }

    private void Movement() {
        GroundPull();

        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;
        CounterMovement(x, y, mag);
        if (readyToJump && jumping) Jump();

        if (crouching && sliding) {
            rb.AddForce(Vector3.down * Time.deltaTime * 3000);
            return;
        }

        float multiplier = 1f, multiplierV = 1f;
        float maxSpeed = this.maxSpeed;
        if (!grounded) multiplier = 0.8f;
        if (!sliding && crouching) {
            multiplierV = 0.5f;
            maxSpeed *= crouchMultiplier;
        }

        ControlSpeed(xMag, yMag, maxSpeed);

        Vector3 forward = orientation.transform.forward;
        Vector3 right = orientation.transform.right;
        forward.y = 0;
        right.y = 0;

        rb.AddForce(forward.normalized * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(right.normalized * x * moveSpeed * Time.deltaTime * multiplier * multiplierV);
    }

    private void GroundPull() {
        if (Grappling) return;
        rb.AddForce(Vector3.down * Time.deltaTime * 10);
    }

    private void ControlSpeed(float xMag, float yMag, float maxSpeed) {
        if (Grappling) return;
        if (Mathf.Abs(x) > 0 && Mathf.Abs(xMag) > maxSpeed) x = 0;
        if (Mathf.Abs(y) > 0 && Mathf.Abs(yMag) > maxSpeed) y = 0;
    }

    public Vector2 FindVelRelativeToLook() {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private void Jump() {
        if (grounded && readyToJump) {
            readyToJump = false;

            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 1f);

            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            if (rb.velocity.y > 0)
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump() {
        readyToJump = true;
    }

    private void getInput() {
        if (Grappling) return;
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");
        crouching = Input.GetButton("Crouch");

        if (Input.GetButtonDown("Crouch"))
            StartCrouchAndSlide();
        if (Input.GetButtonUp("Crouch"))
            StopCrouch();
    }

    private void StartCrouchAndSlide() {
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - crouchHeightChange, transform.position.z);

        hands.transform.localScale = handsScale;

        if (rb.velocity.magnitude >= maxSpeed * 0.9f && grounded && !sliding) {
            Vector3 vel = rb.velocity;
            vel = vel.normalized;
            sliding = true;
            rb.AddForce(new Vector3(vel.x * slideForce, 0, vel.z * slideForce));
            Invoke(nameof(ResetSliding), slidingCooldown);
        }
    }

    private void ResetSliding() {
        sliding = false;
    }

    private void StopCrouch() {
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + crouchHeightChange, transform.position.z);

        hands.transform.localScale = playerScale;
    }

    private float desiredX;
    private void Look() {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        Vector3 rot = playerCam.transform.rotation.eulerAngles;
        desiredX = mouseX + rot.y;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
    }
    private bool IsFloor(Vector3 v) {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private void CounterMovement(float x, float y, Vector2 mag) {
        if (!grounded || jumping) return;

        if (crouching) {
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
            return;
        }

        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0)) {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0)) {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed) {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    private bool cancellingGrounded;

    public bool Grappling { get => grappling; set => grappling = value; }

    private void OnCollisionStay(Collision other) {
        //Make sure we are only checking for walkable layers
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        //Iterate through every collision in a physics update
        for (int i = 0; i < other.contactCount; i++) {
            Vector3 normal = other.contacts[i].normal;
            grounded = true;
            cancellingGrounded = false;
            normalVector = normal;
            CancelInvoke(nameof(StopGrounded));
        }

        //Invoke ground/wall cancel, since we can't check normals with CollisionExit
        float delay = 3f;
        if (!cancellingGrounded) {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }
    private void OnCollisionEnter(Collision collision) {
        if (Grappling) Grappling = false;
    }

    private void StopGrounded() {
        grounded = false;
    }

    private void HandleAnimations()
    {
        if (rb.velocity.magnitude <= 0.5f)
            animator.SetFloat("Speed", 0, 0.2f, Time.deltaTime);
        else
            animator.SetFloat("Speed", 0.6f, 0.2f, Time.deltaTime);
    }

    private Vector3 velocityToSet;
    private void SetVelocity() {
        rb.velocity = velocityToSet;
    }

    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight) {
        velocityToSet = CalculateVelocityNeeded(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);
    }

    private Vector3 CalculateVelocityNeeded(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight) {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }
}
