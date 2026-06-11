using UnityEngine;

namespace Factory.Input
{
    public interface IInputService
    {
        Vector2 CameraMove { get; }
        float CameraZoom { get; }
        float CameraRotate { get; }
        Vector2 Point { get; }
        bool IsPointerOverUI { get; }

        bool RotateBuildingPressed { get; }
        bool BuildPressed { get; }
        bool CancelPressed { get; }
    }
}