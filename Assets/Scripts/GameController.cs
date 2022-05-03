using System;
using Assets.Scripts.Configuration;
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
                BallController.UpdateMovingAbility(_gameState == EGameState.Process);
                GameStateChanged?.Invoke();
            } 
        }

        public BallController BallController
        {
            get;
            private set;
        }
        public GameObject Ball;
        public GameObject StartPointObject;
        public GameObject FinishPointObject;
        public FinishController FinishController;
        private EGameState _gameState;

        // Start is called before the first frame update
        void Start()
        {
            if (Ball == null)
            {
                Debug.LogError("Reference of Ball gameobject not added to GameController");
                return;
            }
            BallController = Ball.GetComponent<BallController>();
            if (Ball == null)
            {
                Debug.LogError("Reference of BallController not added to Ball gameobject");
                return;
            }
            if (StartPointObject == null)
            {
                Debug.LogWarning("Reference of Start gameobject not added to GameController");
                return;
            }
            if (FinishPointObject == null)
            {
                Debug.LogWarning("Reference of Finish gameobject not added to GameController");
                return;
            }

            FinishController.OnFinishedEvent += Finish;

            GameState = EGameState.None;
            Ball.SetActive(false);
        }

        public void StartNewGame()
        {
            StartPointObject.transform.position = DefaultGameConfiguration.StartPoint;
            FinishPointObject.transform.position = DefaultGameConfiguration.FinishPoint;
            BallController.ResetPosition(StartPointObject.transform.position);
            Ball.SetActive(true);
            GameState = EGameState.Process;
        }

        public void Restart()
        {
            Reset();
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

        public void Finish()
        {
            GameState = EGameState.Finished;
            BallController.ResetPosition(FinishPointObject.transform.position);
        }

        public void Reset()
        {
            GameState = EGameState.None;
        }

    }
}
