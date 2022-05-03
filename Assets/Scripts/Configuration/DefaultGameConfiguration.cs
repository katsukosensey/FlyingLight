using UnityEngine;

namespace Assets.Scripts.Configuration
{
    public static class DefaultGameConfiguration
    {
        public static Vector3 StartPoint = new(-10, 0);
        public static Vector3 FinishPoint = new(10, 0);
        /// <summary>
        /// Скорость
        /// </summary>
        public static int Speed = 1;
        /// <summary>
        /// Ускорение при движении к цели
        /// </summary>
        public static int Acceleration = 1;
        /// <summary>
        /// Стартовое ускорение движения от курсора
        /// </summary>
        public static int MinAcceleration = 1;
        /// <summary>
        /// Дельта увеличения ускорения движения от курсора
        /// </summary>
        public static float AccelerationDelta = 0.1f;
        /// <summary>
        /// Ускорение при отталкивании от объекта столкновения
        /// </summary>
        public static int PushAcceleration = 2;
        /// <summary>
        /// Безопасное расстояние до курсора
        /// </summary>
        public static int SecureMouseDistance = 2;
        /// <summary>
        /// Направление движения к цели
        /// </summary>
        public static Vector2 Direction = Vector2.right;
    }
}
