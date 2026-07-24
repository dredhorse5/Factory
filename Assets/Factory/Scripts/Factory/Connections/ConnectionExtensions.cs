namespace Factory
{
    public static class ConnectionExtensions
    {
        public static Cell GetLookAtCell(this Connection connection)
        {
            if(connection.Type == Connection.Types.Output)
                return connection.Cell + connection.Direction;
            return connection.Cell - connection.Direction;
        }

        public static bool IsValidConnection(this Connection source, Connection target)
        {
            if (source == null || target == null)
                return false;
            if (source.Type == target.Type)
                return false;
            if (source.GetLookAtCell() != target.Cell || target.GetLookAtCell() != source.Cell)
                return false;

            return true;
        }
    }
}