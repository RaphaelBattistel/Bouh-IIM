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
            isPressed = false; 
        }
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Box") && porte)
        {
            isPressed = true;
            porte = false;
        }

    }
}
