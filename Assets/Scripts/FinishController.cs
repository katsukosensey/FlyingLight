using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts
{
    public class FinishController : MonoBehaviour
    {
        public GameObject Ball;
        public delegate void FinishControllerDelegate();
        public event FinishControllerDelegate OnFinishedEvent;

        void Start()
        {
            if (Ball == null)
            {
                Debug.LogError("Reference of Ball gameobject not added to FinishController");
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.Equals(Ball))
            {
                OnFinishedEvent?.Invoke();
            }
            else
            {
                Debug.Log($"{other.name} object triggered");
            }
        }
    }
}
