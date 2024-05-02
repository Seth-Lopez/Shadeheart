using UnityEngine;

public class Wheels : MonoBehaviour
{
    public float rollDuration = 1f;
    private int direction = -1;
    public bool isSlowingDown = false;
    private float speed = 360f; // Initial speed of wheel rotation

    void Update()
    {
        PlayAnimation(changeDirection(0));
    }

    private void PlayAnimation(int value)
    {
        if (!isSlowingDown)
        {
            speed = 360f; // Reset speed to original value if not slowing down
        }
        else
        {
            if (speed > 0f)
            {
                speed -= Time.deltaTime * speed; // Decelerate smoothly
            }
            else
            {
                speed = 0f; // Ensure speed is exactly 0 when deceleration is complete
            }
        }

        float rotationAmount = speed / rollDuration * Time.deltaTime;
        transform.Rotate(value * Vector3.forward, rotationAmount);

        if (transform.localEulerAngles.z >= 360f)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0f);
        }
    }

    public int changeDirection(int newDirection)
    {
        if (newDirection != 0 && (newDirection == 1 || newDirection == -1))
        {
            direction = newDirection;
            return direction;
        }
        return direction;
    }
}
