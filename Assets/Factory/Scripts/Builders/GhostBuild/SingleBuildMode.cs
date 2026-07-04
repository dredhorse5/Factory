using Factory.Input;
using UnityEngine;
using VContainer;

namespace Factory.GhostBuild
{
    public class SingleBuildMode : IBuildMode
    {
        private BaseBuildSO selectedBuild;
        public BaseBuildSO SelectedBuild => selectedBuild;

        public int Version => _version;
        private int _version;
        public bool CanBuild => isPlaceFree;
        
        private readonly IInputService input;
        private readonly CursorWorldPositionProvider cursor;
        private readonly BuildSystem buildSystem;
        private readonly GhostBuildSystem ghostBuildSystem;
        
        private bool isPlaceFree;
        private BuildPlacement buildPlacement;

        [Inject]
        public SingleBuildMode(IInputService input, CursorWorldPositionProvider cursor, BuildSystem buildSystem, GhostBuildSystem ghostBuildSystem)
        {
            this.input = input;
            this.cursor = cursor;
            this.buildSystem = buildSystem;
            this.ghostBuildSystem = ghostBuildSystem;
        }


        public void Enter(BaseBuildSO buildSO)
        {
            selectedBuild = buildSO;
            SetCell(cursor.GetLookAtCell());
        }
        
        public void Tick()
        {
            SetCell(cursor.GetCellUnderCursor());
            if (input.RotateBuildingPressed)
                Rotate(true);
            if(input.PointerClick && !input.IsPointerOverUI)
                ghostBuildSystem.Build();
        }
        
        public void Clear() { }

        private void SetCell(Cell cell)
        {
            if (cell != buildPlacement.Cell)
                _version++;
            buildPlacement.Cell = cell;
            isPlaceFree = buildSystem.CanPlaceBuild(buildPlacement.Cell, buildPlacement.Rotation, selectedBuild);
        }

        private void Rotate(bool right = true)
        {
            int i = 0;
            if (right)
            {
                i = ((int)(buildPlacement.Rotation));
                i++;
            }
            else
            {
                i = ((int)(buildPlacement.Rotation));
                i--;
                if (i < 0) i = 3;
            }

            i %= 4;
            SetRotation((BuildRotations)i);
        }

        private void SetRotation(BuildRotations rotation)
        {
            if (rotation != buildPlacement.Rotation)
                _version++;
            buildPlacement.Rotation = rotation;
            isPlaceFree = buildSystem.CanPlaceBuild(buildPlacement.Cell, buildPlacement.Rotation, selectedBuild);
        }


        public BuildPlacement[] GetGhostPlacements() => new BuildPlacement[] { buildPlacement };
        
        public void Dispose()
        {
            selectedBuild = null;
        }

    }
}