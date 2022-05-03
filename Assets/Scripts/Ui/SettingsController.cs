using Assets.Scripts.Configuration;
using UnityEngine.UIElements;

namespace Assets.Scripts.Ui
{
    public class SettingsController
    {
        /// <summary>
        /// Слайдер скорости 
        /// </summary>
        public SliderInt SpeedSlider;
        /// <summary>
        /// Слайдер ускорения
        /// </summary>
        public SliderInt AccelerationSlider;
        /// <summary>
        /// Слайдер безопасного расстояния до курсора
        /// </summary>
        public SliderInt MouseZoneSlider;
        public SettingsController(GameController gameController, VisualElement uiRoot)
        {
            SpeedSlider = uiRoot.Q<SliderInt>("speed-slider");
            AccelerationSlider = uiRoot.Q<SliderInt>("accel-slider");
            MouseZoneSlider = uiRoot.Q<SliderInt>("mouse-zone-slider");

            var speedBtn = uiRoot.Q<Button>("speed-btn");
            var accelerationBtn = uiRoot.Q<Button>("accel-btn");
            var mouseZoneBtn = uiRoot.Q<Button>("mouse-zone-btn");
            var mainContainer = uiRoot.Q<VisualElement>("main-container");

            speedBtn.RegisterCallback<MouseOverEvent>(_ => SpeedSlider.visible = true);
            accelerationBtn.RegisterCallback<MouseOverEvent>(_ => AccelerationSlider.visible = true);
            mouseZoneBtn.RegisterCallback<MouseOverEvent>(_ => MouseZoneSlider.visible = true);

            SpeedSlider.RegisterValueChangedCallback(x => gameController.BallController.Speed = x.newValue);
            AccelerationSlider.RegisterValueChangedCallback(x => gameController.BallController.MinAcceleration = x.newValue);
            MouseZoneSlider.RegisterValueChangedCallback(x => gameController.BallController.SecureMouseDistance = x.newValue);
            SetDefaultSettings();
            mainContainer.RegisterCallback<MouseUpEvent>(_ => HideSettingsSliders());
            HideSettingsSliders();
        }

        void SetDefaultSettings()
        {
            SpeedSlider.value = DefaultGameConfiguration.Speed;
            AccelerationSlider.value = DefaultGameConfiguration.MinAcceleration;
            MouseZoneSlider.value = DefaultGameConfiguration.SecureMouseDistance;
        }
        public void HideSettingsSliders()
        {
            SpeedSlider.visible = false;
            AccelerationSlider.visible = false;
            MouseZoneSlider.visible = false;
        }
    }
}
