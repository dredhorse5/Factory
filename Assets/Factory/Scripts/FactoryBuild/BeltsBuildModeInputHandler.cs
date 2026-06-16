using Factory.Input;

namespace Factory.Scripts.FactoryBuild
{
    public class BeltsBuildModeInputHandler
    {
        private readonly IInputService inputService;
        private readonly BeltsBuildMode beltsBuildMode;

        public BeltsBuildModeInputHandler(IInputService inputService, BeltsBuildMode beltsBuildMode)
        {
            this.inputService = inputService;
            this.beltsBuildMode = beltsBuildMode;
        }
    }
}