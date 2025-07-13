using UnityEngine;

public class Ground : MonoBehaviour
{
    public GameObject LeftJoyStick, RightJoyStick;
    private Rigidbody2D rb;
    public float moveSpeed = 10f;
    public float rotationSpeed = 10f;
    private float initialRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = false;


        float LeftJoyStickY = LeftJoyStick.transform.position.y;
        float LeftJoyStickX = LeftJoyStick.transform.position.x;
        float RightJoyStickY = RightJoyStick.transform.position.y;
        float RightJoyStickX = RightJoyStick.transform.position.x;

        initialRotation = Mathf.Atan2(LeftJoyStickY - RightJoyStickY, LeftJoyStickX - RightJoyStickX) * Mathf.Rad2Deg;

        rb.rotation = initialRotation;
    }

    void FixedUpdate()
    {

        float LeftJoyStickY = LeftJoyStick.transform.position.y;
        float LeftJoyStickX = LeftJoyStick.transform.position.x;
        float RightJoyStickY = RightJoyStick.transform.position.y;
        float RightJoyStickX = RightJoyStick.transform.position.x;


        Vector2 targetPosition = new Vector2(0, (LeftJoyStickY + RightJoyStickY) / 2);


        rb.MovePosition(Vector2.Lerp(rb.position, targetPosition, Time.fixedDeltaTime * moveSpeed));


        float AtanRotateZ = Mathf.Atan2(LeftJoyStickY - RightJoyStickY, LeftJoyStickX - RightJoyStickX) * Mathf.Rad2Deg;


        rb.rotation = Mathf.LerpAngle(rb.rotation, AtanRotateZ, Time.fixedDeltaTime * rotationSpeed);
    }
}

