using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    private Rigidbody2D landerRigidbody2D;

    private void Awake()
    {
        landerRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Keyboard.current.upArrowKey.isPressed)
        {
            float force = 700f; 
            landerRigidbody2D.AddForce(force * transform.up * Time.deltaTime);
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            float turnSpeed = +100f;
            landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
        }
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            float turnSpeed = -100f;
            landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision2d)
    {
        float softLandingVelocityMagnitude = 4f;
        if (collision2d.relativeVelocity.magnitude > softLandingVelocityMagnitude)
        {
            // Landed hard
            Debug.Log("Landed hard");
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDot =.9f;

        if (dotVector < minDot)
        {
            // too steep angle
            Debug.Log("too steep angle");
            return;
        }

        Debug.Log("succesful landing");
    }
}
