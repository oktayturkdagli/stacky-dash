using UnityEngine;

public class StackController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            //Destroy(gameObject);
        }
    }
}
