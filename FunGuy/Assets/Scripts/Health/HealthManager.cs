using System.Collections.Generic;
using System.Linq;
using Event;
using UnityEngine;
using Util;

namespace Health
{
    public class HealthManager : MonoBehaviour, IEventReceiver<HealthLostEvent>

    {
        [SerializeField] private string PID;

        [SerializeField] private GameObject _heartObject;

        private readonly List<GameObject> _hearts = new List<GameObject>();

        private void Start()
        {
            EventBus<HealthLostEvent>.Register(this);
            for (var i = 0; i < GlobalConst.MaxHealth; i++)
            {
                var direction = PID == GlobalConst.PlayerOne ? Vector3.right : Vector3.left;
                var heart = Instantiate(_heartObject, transform.position + direction * i, Quaternion.identity);
                _hearts.Add(heart);
            }
        }

        private void OnDestroy()
        {
            EventBus<HealthLostEvent>.UnRegister(this);
        }

        public void OnEvent(HealthLostEvent e)
        {
            if (PID != e.PID) return;
            if (_hearts.Count <= 1) return;

            var heart = _hearts.LastOrDefault();
            _hearts.Remove(heart);
            Destroy(heart);
        }
    }
}