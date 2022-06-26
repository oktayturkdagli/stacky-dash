using System;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private Transform playerStacks;
    [SerializeField] private float altitudeMultiplier = 0.075f;
    private Animator animator;
    private BoxCollider boxCollider;
    private bool canMove = true;
    Vector3 directionVector = Vector3.zero;
    private float currentAltitude;
    private int playerStacksCounter = 1;
    
    void Start()
    {
        EventManager.current.onSwipe += OnSwipe;
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        currentAltitude = transform.position.y;
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
        
        bool isThereRoad = SendRaycastToFindRoad(directionVector);
        if (isThereRoad)
        {
            HandleMovement();
        }
    }
    
    void HandleMovement()
    {
        AdjustTheColliderDirection();
        StartMovement();
    }

    void StartMovement()
    {
        if (canMove)
        {
            canMove = false;
            if (directionVector.x != 0)
                transform.DOMoveX(directionVector.x * 100, 100/speed).SetId("Movement").SetUpdate(UpdateType.Fixed);
            else if (directionVector.z != 0)
                transform.DOMoveZ(directionVector.z * 100, 100/speed).SetId("Movement").SetUpdate(UpdateType.Fixed);
        }
    }
    
    void StopMovement()
    {
        DOTween.Kill("Movement");
        Vector3 stopVector = new Vector3((float)Math.Round(transform.position.x), transform.position.y, (float)Math.Round(transform.position.z));
        transform.DOMove(stopVector, 0.5f).OnComplete(()=>canMove = true);
    }
    
    void AdjustTheColliderDirection()
    {
        Vector3 boxColliderLocationVector = Vector3.zero;
        Vector3 boxColliderSizeVector = new Vector3(0.1f, 0.1f, 0.1f);
        float location = 0.25f, size = 1f;
        boxColliderLocationVector.y = location;
        if (directionVector.x > 0)
        {
            boxColliderLocationVector.x = location;
            boxColliderSizeVector.x = size;
        }
        else if (directionVector.x < 0)
        {
            boxColliderLocationVector.x = -location;
            boxColliderSizeVector.x = size;
        }
        else if (directionVector.z > 0)
        {
            boxColliderLocationVector.z = location;
            boxColliderSizeVector.z = size;
        }
        else if (directionVector.z < 0)
        {
            boxColliderLocationVector.z = -location;
            boxColliderSizeVector.z = size;
        }

        boxCollider.center = boxColliderLocationVector;
        boxCollider.size = boxColliderSizeVector;
    }
    
    bool SendRaycastToFindRoad(Vector3 directionVector)
    {
        Vector3 origin = transform.position + Vector3.up + directionVector; //From the player's body
        Vector3 direction = Vector3.down; // Downwards
        bool isThereRoad = false;
        RaycastHit hit;
        
        Debug.DrawRay(origin, direction * 500, Color.red, 3f);
        if (Physics.Raycast(origin, direction, out hit,500))
        {
            GameObject obj = hit.transform.gameObject;
            if (obj.tag.Equals("Road") || obj.tag.Equals("Stack"))
            {
                isThereRoad = true;
            }
        }

        return isThereRoad;
    }
    
    void Redirect(string whereDidItComeFrom, string routerName)
    {
        if (whereDidItComeFrom.Equals("Left")) 
        {
            if (routerName.Equals("LDDL")) // Left to Down, Down to Left
            {
                directionVector = Vector3.back;
            }
            else if (routerName.Equals("LUUL")) // Left to Up, Up to Left
            {
                directionVector = Vector3.forward;
            }
        } else if (whereDidItComeFrom.Equals("Right")) 
        {
            if (routerName.Equals("RDDR")) // Right to Down, Down to Right
            {
                directionVector = Vector3.back;
            }
            else if (routerName.Equals("RUUR")) // Right to Up, Up to Right
            {
                directionVector = Vector3.forward;
            }
        }
        else if (whereDidItComeFrom.Equals("Down")) 
        {
            if (routerName.Equals("RDDR")) // Right to Down, Down to Right
            {
                directionVector = Vector3.right;
            }
            else if (routerName.Equals("LDDL")) // Left to Down, Down to Left
            {
                directionVector = Vector3.left;
            }
        }
        else if (whereDidItComeFrom.Equals("Up")) 
        {
            if (routerName.Equals("RUUR")) // Right to Up, Up to Right
            {
                directionVector = Vector3.right;
            }
            else if (routerName.Equals("LUUL")) // Left to Up, Up to Left
            {
                directionVector = Vector3.left;
            }
        }
        
        DOTween.Kill("Movement");
        Vector3 stopVector = new Vector3((float)Math.Round(transform.position.x), transform.position.y, (float)Math.Round(transform.position.z));
        transform.DOMove(stopVector, 0.1f).OnComplete(()=>
        {
            canMove = true;
            HandleMovement();
        });
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Stack"))
        {
            currentAltitude += altitudeMultiplier;
            playerStacksCounter += 1;
            DOTween.Kill("Rising");
            transform.DOMoveY(currentAltitude, 0.2f).SetRelative(false).SetId("Rising");
            animator.SetBool("Jump", true);
            
            Transform stack = other.transform;
            stack.gameObject.GetComponent<BoxCollider>().enabled = false;
            stack.parent = playerStacks;
            stack.localEulerAngles = Vector3.zero;
            stack.transform.localPosition = Vector3.zero;
            stack.transform.localPosition = new Vector3(stack.localPosition.x, (playerStacksCounter - 1) * -altitudeMultiplier ,stack.localPosition.z);
        }
        
        if (other.gameObject.tag.Equals("Gate"))
        {
            animator.SetBool("Jump", false);
            StopMovement();
        }
        
        if (other.gameObject.tag.Equals("Pole"))
        {
            if (playerStacks.childCount > 1)
            {
                currentAltitude -= altitudeMultiplier;
                playerStacksCounter -= 1;
                DOTween.Kill("Rising");
                transform.DOMoveY(currentAltitude, 0.2f).SetRelative(false).SetId("Rising");
                animator.SetBool("Jump", false);
                Transform pole = other.transform;
                Transform stack = playerStacks.GetChild(playerStacks.childCount - 1);
                pole.gameObject.GetComponent<BoxCollider>().enabled = false;
                stack.parent = null;
                stack.localEulerAngles = Vector3.zero;
                stack.transform.position = new Vector3(pole.position.x, pole.position.y + 0.5f, pole.position.z);
            }
            else
            {
                StopMovement();
            }
        }
        
        if (other.gameObject.tag.Equals("Router"))
        {
            animator.SetBool("Jump", false);
            string routerName = other.gameObject.name;
            string whereDidItComeFrom = "Left";
            if (directionVector == Vector3.right)
            {
                whereDidItComeFrom = "Left";
            }
            else if (directionVector == Vector3.left)
            {
                whereDidItComeFrom = "Right";
            }
            else if (directionVector == Vector3.back)
            {
                whereDidItComeFrom = "Up";
            }
            else if (directionVector == Vector3.forward)
            {
                whereDidItComeFrom = "Down";
            }
            
            Redirect(whereDidItComeFrom, routerName);
        }
        
    }

    void OnDestroy()
    {
        EventManager.current.onSwipe -= OnSwipe;
    }
    
}
