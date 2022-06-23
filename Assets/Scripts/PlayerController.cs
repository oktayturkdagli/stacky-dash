using System;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private Transform playerStacks;
    [SerializeField] private float altitudeMultiplier = 0.075f;
    private Animator animator;
    private bool canMove = true;
    Vector3 directionVector = Vector3.zero;
    private float currentAltitude = 0f;
    private int playerStacksCounter = 0;
    
    void Start()
    {
        EventManager.current.onSwipe += OnSwipe;
        animator = GetComponent<Animator>();
        currentAltitude = transform.position.y;
    }

    private void Update()
    {
        if (canMove)
        {
            transform.Translate(directionVector * speed * Time.deltaTime, Space.World);
        }
    }

    void OnSwipe(string direction)
    {
        directionVector = Vector3.zero;
        switch (direction)
        {
            case "Left":
                directionVector = Vector3.left;
                break;
            case "Right":
                directionVector = Vector3.right;
                break;
            case "Up":
                directionVector = Vector3.forward;
                break;
            case "Down":
                directionVector = Vector3.back;
                break;
        }
        HandleMovement(directionVector);
    }
    
    void HandleMovement(Vector3 directionVector)
    {
        canMove = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Border"))
        {
            canMove = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Stack"))
        {
            currentAltitude += altitudeMultiplier;
            playerStacksCounter += 1;
            transform.DOKill(false);
            transform.DOMoveY(currentAltitude, 0.2f).SetRelative(false);
            animator.SetTrigger("Jump");

            Transform stack = other.transform;
            stack.gameObject.GetComponent<BoxCollider>().enabled = false;
            stack.parent = playerStacks;
            stack.localEulerAngles = Vector3.zero;
            stack.transform.localPosition = Vector3.zero;
            stack.transform.localPosition = new Vector3(stack.localPosition.x, (playerStacksCounter - 1) * -altitudeMultiplier ,stack.localPosition.z);
        }
    }

    void OnDestroy()
    {
        EventManager.current.onSwipe -= OnSwipe;
    }
    
}