using System;
using UnityEngine;
using Random = System.Random;

public class BallController : MonoBehaviour
{
    private Random _random;
    private float _currentAcceleration;
    public int Speed = 1;
    public int DefaultAcceleration = 1;
    public int MinAcceleration = 1;
    public float AccelerationDelta = 0.1f;
    public int SecureMouseDistance = 2;
    public Vector2 Direction = Vector2.right;
    public GameObject Bottom;
    public GameObject BottomCheck;
    public GameObject Top;
    public GameObject TopCheck;
    
    public bool CanMove;
    /// <summary>
    /// В процессе перестроения движения
    /// </summary>
    public bool IsAvoiding;
    public bool CatchedByMouse;

    // Start is called before the first frame update
    void Start()
    {
        _random = new Random();
    }

    public void ResetPosition(Vector3 position)
    {
        CanMove = false;
        transform.position = position;
        ResetTranslationParams();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsAvoiding)
        {
            return;
        }

        if (!CanMove)
        {
            return;
        }

        if (BottomCheck.transform.position.y < Bottom.transform.position.y)
        {
            Debug.LogWarning("Ball out of bottom");
            transform.position = new Vector3(transform.position.x, Bottom.transform.position.y + 1);
        }
        if (TopCheck.transform.position.y > Top.transform.position.y)
        {
            Debug.LogWarning("Ball out of top");
            transform.position = new Vector3(transform.position.x, Top.transform.position.y - 1);
        }

        if (CatchedByMouse)
        {
            if (IsMouseDistanceSecure())
            {
                Debug.Log($"Position is secure");
                ResetTranslationParams();
                Reset();
            }
            else
            {
                _currentAcceleration+= AccelerationDelta;
            }
        }

        Move();
        
    }

    void OnMouseOver()
    {
        if (!CanMove)
        {
            return;
        }

        if (IsAvoiding)
        {
            return;
        }
        IsAvoiding = true;
        MoveByPush();
        AvoidEnemyMouse();
        CatchedByMouse = true;
        IsAvoiding = false;
    }


    void OnMouseExit()
    {
    }

    public void Move()
    {
        //transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -10f, 10f), transform.position.z);
        var translation = Direction * Time.deltaTime * Speed * _currentAcceleration;
        transform.Translate(translation);
    }

    public bool IsMouseDistanceSecure()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var distance = Vector2.Distance(transform.position, worldPosition);
        return distance > SecureMouseDistance;

    }

    void MoveByPush()
    {
        if (Speed > 0)
        {
            Speed *= -1;
        }

        _currentAcceleration = 2;
        Move();
    }
    public void AvoidEnemyMouse()
    {
        var coefY = _random.Next(0, 101) % 2 == 0 ? 1 : -1;
        var dirY = 2;
        Direction = new Vector2(1, (float) (coefY* dirY));
        Speed = Math.Abs(Speed);
        _currentAcceleration = MinAcceleration;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!CanMove)
        {
            return;
        }
        if (other.gameObject.Equals(Bottom))
        {
            IsAvoiding = true;
            Direction = new Vector2(transform.position.x + 1, 0);
            Debug.LogError($"Caught bottom border");
        }
        if (other.gameObject.Equals(Top))
        {
            IsAvoiding = true;
            Direction = new Vector2(transform.position.x + 1, 0);
            Debug.LogError($"Caught top  border");
        }

        if (IsAvoiding)
        {
            _currentAcceleration = MinAcceleration;
            Move();
            ResetTranslationParams();
            IsAvoiding = false;
        }
    }


    public void ResetTranslationParams()
    {
        Direction = Vector2.right;
        _currentAcceleration = DefaultAcceleration;
    }

    public void Reset()
    {
        CatchedByMouse = false;
    }
}
