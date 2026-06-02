using System;
using UnityEngine;
using VContainer.Unity;

namespace Factory
{
    public class TickSystem : ITickable
    {
        public static event Action<float> OnTick;

        [SerializeField] private int targetTPS = 20;
        private float accumulator;

        public static uint CurrentTick { get; private set; }
        public static float TickInterval { get; private set; }

        public TickSystem(int targetTPS = 20)
        {
            this.targetTPS = targetTPS;
            CalculateTickInterval();
        }

        private void CalculateTickInterval()
        {
            TickInterval = 1f / targetTPS;
        }
        public void Tick()
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