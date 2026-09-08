using System;
using Factory;
using VContainer;

public class SimulationSystem : IDisposable
{
    private readonly BeltSimulation beltSimulation;
    private readonly TransferSimulation transferSimulation;

    [Inject]
    public SimulationSystem(BeltSimulation beltSimulation, TransferSimulation transferSimulation)
    {
        this.beltSimulation = beltSimulation;
        this.transferSimulation = transferSimulation;
        TickSystem.OnFixedTick += Tick;
    }

    private void Tick(float obj)
    {
        beltSimulation.Tick(obj);
        transferSimulation.Tick(obj);
    }

    public void Dispose()
    {
        TickSystem.OnFixedTick -= Tick;
    }
}
