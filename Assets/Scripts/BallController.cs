using System;
using Assets.Scripts.Configuration;
using UnityEngine;
using Random = System.Random;

public class BallController : MonoBehaviour
{
    private Random _random;
    private float _currentAcceleration;
    /// <summary>
    /// В процессе перестроения движения
    /// </summary>
    private bool _isAvoiding;
    /// <summary>
    /// Пойман курсором
    /// </summary>
    private bool _isCatchedByMouse;

    private bool _canMove;

    public int Speed;
    public int MinAcceleration;
    public int SecureMouseDistance;
    public Vector2 Direction;

    public GameObject Bottom;
    public GameObject BottomCheck;
    public GameObject Top;
    public GameObject TopCheck;

    // Start is called before the first frame update
    void Start()
    {
        _random = new Random();
        Speed = DefaultGameConfiguration.Speed;
        _currentAcceleration = DefaultGameConfiguration.Acceleration;
        MinAcceleration = DefaultGameConfiguration.MinAcceleration;
        Direction = DefaultGameConfiguration.Direction;
        SecureMouseDistance = DefaultGameConfiguration.SecureMouseDistance;
    }

    public void ResetPosition(Vector3 position)
    {
        _canMove = false;
        transform.position = position;
        ResetTranslationParams();
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAvoiding)
        {
            return;
        }

        if (!_canMove)
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

        if (_isCatchedByMouse)
        {
            if (IsMouseDistanceSecure())
            {
                Debug.Log($"Position is secure");
                ResetTranslationParams();
                Reset();
            }
            else
            {
                _currentAcceleration+= DefaultGameConfiguration.AccelerationDelta;
            }
        }

        Move();
        
    }

    void OnMouseOver()
    {
        if (!_canMove)
        {
            return;
        }

        if (_isAvoiding)
        {
            return;
        }
        _isAvoiding = true;
        MoveByPush();
        AvoidEnemyMouse();
        _isCatchedByMouse = true;
        _isAvoiding = false;
    }

    public void Move()
    {
        var translation = Direction * Time.deltaTime * Speed * _currentAcceleration;
        transform.Translate(translation);
    }
    /// <summary>
    /// Проверка безопасности расстояния до курсора
    /// </summary>
    /// <returns></returns>
    public bool IsMouseDistanceSecure()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var distance = Vector2.Distance(transform.position, worldPosition);
        return distance > SecureMouseDistance;

    }

    /// <summary>
    /// Отталкивание от объекта столкновения (курсор, граница)
    /// </summary>
    void MoveByPush()
    {
        if (Speed > 0)
        {
            Speed *= -1;
        }

        _currentAcceleration = DefaultGameConfiguration.PushAcceleration;
        Move();
    }
    /// <summary>
    /// Перерасчет параметров движения от курсора 
    /// </summary>
    public void AvoidEnemyMouse()
    {
        var coefY = _random.Next(0, 101) % 2 == 0 ? 1 : -1;
        var dirY = 2;
        Direction = new Vector2(1, coefY* dirY);
        Speed = Math.Abs(Speed);
        _currentAcceleration = MinAcceleration;
    }
    /// <summary>
    /// Движение от границы
    /// </summary>
    public void AvoidBorder()
    {
        _currentAcceleration = MinAcceleration;
        Move();
        ResetTranslationParams();
        _isAvoiding = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!_canMove)
        {
            return;
        }
        if (other.gameObject.Equals(Bottom))
        {
            _isAvoiding = true;
            Direction = new Vector2(transform.position.x + 1, 0);
            Debug.LogError($"Caught bottom border");
        }
        if (other.gameObject.Equals(Top))
        {
            _isAvoiding = true;
            Direction = new Vector2(transform.position.x + 1, 0);
            Debug.LogError($"Caught top  border");
        }

        if (_isAvoiding)
        {
            AvoidBorder();
        }
    }

    public void UpdateMovingAbility(bool canMove)
    {
        _canMove = canMove;
    }


    public void ResetTranslationParams()
    {
        Direction = DefaultGameConfiguration.Direction;
        _currentAcceleration = DefaultGameConfiguration.Acceleration;
    }

    public void Reset()
    {
        _isCatchedByMouse = false;
    }
}
