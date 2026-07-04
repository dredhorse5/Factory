using Factory.Input;
using UnityEngine;
using VContainer;

namespace Factory
{
    public class CursorWorldPositionProvider
    {
        private readonly MainCamera camera;
        private readonly Map map;
        private readonly IInputService input;

        [Inject]
        public CursorWorldPositionProvider(MainCamera camera, Map map, IInputService input)
        {
            this.camera = camera;
            this.map = map;
            this.input = input;
        }

        public Cell GetCellUnderCursor()
        {
            Ray ray = camera.Camera.ScreenPointToRay(input.Point);

            if (Physics.Raycast(ray, out RaycastHit hit, 5000f))
                return map.GetCellByPosition(hit.point);
        
            return map.GetCellByPosition(ray.origin + ray.direction * 10f);
        }
        
        public Cell GetLookAtCell()
        {
            return map.GetCellByPosition(camera.transform.position);
        }
    }
}