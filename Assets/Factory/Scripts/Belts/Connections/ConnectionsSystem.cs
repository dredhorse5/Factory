using System;
using VContainer;

namespace Factory
{
    public class ConnectionsSystem : IDisposable
    {
        private readonly BuildSystem _buildSystem;
        private readonly WorldProvider _worldProvider;
        
        [Inject]
        public ConnectionsSystem(BuildSystem buildSystem, WorldProvider worldProvider)
        {
            _buildSystem = buildSystem;
            _worldProvider = worldProvider;

            _buildSystem.OnBuildCreated += OnBuildCreated;
            _buildSystem.OnBuildReconfigured += OnBuildReconfigured;
            _buildSystem.OnBuildWillDestroy += OnBuildDestroyed;
        }

        private void OnBuildDestroyed(uint arg1, BaseBuild arg2)
        {
            if (arg2 is FactoryBuild factoryBuild)
                DisconnectAll(factoryBuild);
        }

        private void OnBuildReconfigured(uint arg1, BaseBuild arg2)
        {
            if (arg2 is FactoryBuild factoryBuild)
            {
                DisconnectAll(factoryBuild);
                ConnectAll(factoryBuild);
            }
        }

        private void OnBuildCreated(uint arg1, BaseBuild arg2)
        {
            if (arg2 is FactoryBuild factoryBuild)
                ConnectAll(factoryBuild);
        }
        
        private void ConnectAll(FactoryBuild build)
        {
            foreach (var connection in build.InputConnections)
                TryConnect(connection);

            foreach (var connection in build.OutputConnections)
                TryConnect(connection);
        }

        private void DisconnectAll(FactoryBuild build)
        {
            foreach (var connection in build.InputConnections)
                Disconnect(connection);

            foreach (var connection in build.OutputConnections)
                Disconnect(connection);
        }
        
        private void Disconnect(Connection connection)
        {
            if (connection.Connected == null)
                return;

            connection.Connected.Connected = null;
            connection.Connected = null;
        }

        private bool TryConnect(Connection connection)
        {
            if(connection.Connected != null)
                return false;
            var nextCell = connection.GetLookAtCell();
            if(_worldProvider.world.TryGetBuild(nextCell, out var build) && build is FactoryBuild factoryBuild)
            {
                var candidates = connection.Type == Connection.Types.Input
                    ? factoryBuild.OutputConnections
                    : factoryBuild.InputConnections;
                
                for (var i = 0; i < candidates.Length; i++)
                {
                    if(candidates[i].Connected != null)
                        continue;
                    
                    if (connection.IsValidConnection(candidates[i]))
                    {
                        connection.Connected = candidates[i];
                        candidates[i].Connected = connection;
                        return true;
                    }
                }
            }
            

            return false;
        }
        

        public void Dispose()
        {
            _buildSystem.OnBuildCreated -= OnBuildCreated;
            _buildSystem.OnBuildReconfigured -= OnBuildReconfigured;
            _buildSystem.OnBuildWillDestroy -= OnBuildDestroyed;
        }
        
    }
}