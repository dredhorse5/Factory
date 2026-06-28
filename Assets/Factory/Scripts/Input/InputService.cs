using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Factory.Input
{
    public class InputService : IInputService
    {
        private readonly GameInput input;
        public InputService()
        {
            input = new GameInput();
            input.Enable();
        }

        public Vector2 CameraMove => input.General.CameraMove.ReadValue<Vector2>();
        public float CameraZoom => input.General.CameraZoom.ReadValue<float>();
        public float CameraRotate => input.General.CameraRotate.ReadValue<float>();
        public Vector2 Point => input.General.Point.ReadValue<Vector2>();
        public bool IsPointerOverUI =>
            EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject();

        public bool PointerDown => input.General.PointerDown.WasPressedThisFrame();
        public bool PointerClick =>  input.General.PointerClick.WasPressedThisFrame();
        public bool RotateBuildingPressed => input.General.RotateBuilding.WasPressedThisFrame();
        public bool BuildPressed => input.General.PlaceBuilding.WasPressedThisFrame();
        public bool DestroyBuildPressed => input.General.RightPointerDown.WasPressedThisFrame();
        public bool CancelPressed => input.General.Cancel.WasPressedThisFrame();
    }
}