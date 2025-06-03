using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject door;
    public float moveSpeed = 2f; 

    public bool isPressed = false;

    private bool porte=true;

    void Update()
    {

        if (isPressed)
        {
            door.transform.position = Vector2.Lerp(door.transform.position, new Vector2(door.transform.position.x, door.transform.position.y + door.transform.localScale.y), moveSpeed * Time.deltaTime);
            if (door.transform.position.y >= door.transform.localScale.y)
                isPressed = false;
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Box") && porte)
        {
            isPressed = true;
            porte = false;
        }
    }
}
