using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private Vector2 _touchStartPos;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.down, Time.deltaTime * 30f);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.up, Time.deltaTime * 30f);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (transform.localScale.z < 2)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y,
                    transform.localScale.z + (1 * Time.deltaTime));
            }
            else
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 2);
            }
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (transform.localScale.z > 0.1f)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y,
                    transform.localScale.z - (1 * Time.deltaTime));
            }
            else
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 0.1f);
            }
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                float touchDeltaX = touch.deltaPosition.x;

                if (touchDeltaX > 0)
                {
                    transform.Rotate(Vector3.up, Time.deltaTime * 30f);
                }
                else if (touchDeltaX < 0)
                {
                    transform.Rotate(Vector3.down, Time.deltaTime * 30f);
                }
            }

            if (touch.phase == TouchPhase.Began)
            {
                _touchStartPos = touch.position;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                float touchDeltaY = touch.position.y - _touchStartPos.y;

                if (touchDeltaY > 0)
                {
                    if (transform.localScale.z < 2)
                    {
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y,
                            transform.localScale.z + (1 * Time.deltaTime));
                    }
                    else
                    {
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 2);
                    }
                }
                else if (touchDeltaY < 0)
                {
                    if (transform.localScale.z > 0.1f)
                    {
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y,
                            transform.localScale.z - (1 * Time.deltaTime));
                    }
                    else
                    {
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 0.1f);
                    }
                }
            }
        }
    }
}
