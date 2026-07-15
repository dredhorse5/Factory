using System;
using Factory;
using VContainer;

public class SimulationSystem : IDisposable
{
    private readonly BeltSimulation _beltSimulation;

    [Inject]
    public SimulationSystem(BeltSimulation beltSimulation)
    {
        _beltSimulation = beltSimulation;
        TickSystem.OnFixedTick += Tick;
    }

    private void Tick(float obj)
    {
        _beltSimulation.Tick(obj);
    }

    public void Dispose()
    {
        TickSystem.OnFixedTick -= Tick;
    }
}
