using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameController : MonoBehaviour
    {
        public bool IsGameInProcess;
        public BallController BallController;
        public GameObject Ball;
        public GameObject StartPointObject;
        public GameObject FinishPointObject;

        // Start is called before the first frame update
        void Start()
        {
            StartNewGame();
        }

        public void StartNewGame()
        {
            if (StartPointObject == null)
            {
                Debug.LogWarning("Отсутствует ссылка на объект старта");
                return;
            }
            if (FinishPointObject == null)
            {
                Debug.LogWarning("Отсутствует ссылка на объект финиша");
                return;
            }

            StartPointObject.transform.position = new Vector3(-10, 0);
            FinishPointObject.transform.position = new Vector3(10,0);
            BallController.ResetPosition(StartPointObject.transform.position);
            BallController.Reset();
            BallController.CanRun = true;
            IsGameInProcess = true;
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Restart();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.Equals(Ball))
            {
                BallController.CanRun = false;
                IsGameInProcess = false;
                Debug.LogError($"Game was finished");
            }
            else
            {
                Debug.Log($"{other.name} object triggered");
            }
        }

        public void Restart()
        {
            IsGameInProcess = false;
            BallController.CanRun = false;
            StartNewGame();
        }

    }
}
