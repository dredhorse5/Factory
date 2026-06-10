using System.Numerics;

namespace Factory.Scripts.Input
{
    public interface IInputService
    {
        Vector2 CameraMove { get; }
        Vector2 CameraRotate { get; }

        bool RotateBuildingPressed { get; }
        bool BuildPressed { get; }
        bool CancelPressed { get; }
    }
}