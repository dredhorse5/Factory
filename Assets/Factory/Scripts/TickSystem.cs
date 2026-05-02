using System;
using UnityEngine;

namespace Factory
{
    public class TickSystem : MonoBehaviour
    {
        public static event Action<float> OnTick;

        [SerializeField] private int targetTPS = 20;
        private float accumulator;

        public static uint CurrentTick { get; private set; }
        public static float TickInterval { get; private set; }

        private void Awake()
        {
            TickInterval = 1f / targetTPS;
        }

        private void Update()
        {
            accumulator += Time.deltaTime;

            while (accumulator >= TickInterval)
            {
                accumulator -= TickInterval;

                CurrentTick++;

                OnTick?.Invoke(TickInterval);
            }
        }
    }
}