using System;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameController : MonoBehaviour
    {
        public Action GameStateChanged;

        public EGameState GameState
        {
            get => _gameState;
            set
            {
                _gameState = value;
                BallController.CanMove = _gameState == EGameState.Process;
                GameStateChanged?.Invoke();
            } 
        }

        public BallController BallController;
        public GameObject Ball;
        public GameObject StartPointObject;
        public GameObject FinishPointObject;
        private EGameState _gameState;

        // Start is called before the first frame update
        void Start()
        {
            GameState = EGameState.None;
            Ball.SetActive(false);
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
            Ball.SetActive(true);
            GameState = EGameState.Process;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.Equals(Ball))
            {
                BallController.CanMove = false;
                GameState = EGameState.Finished;
                BallController.ResetPosition(FinishPointObject.transform.position);
            }
            else
            {
                Debug.Log($"{other.name} object triggered");
            }
        }

        public void Restart()
        {
            GameState = EGameState.None;
            BallController.CanMove = false;
            StartNewGame();
        }

        public void Pause()
        {
            GameState = EGameState.Pause;
        }

        public void Continue()
        {
            GameState = EGameState.Process;
        }

    }
}
