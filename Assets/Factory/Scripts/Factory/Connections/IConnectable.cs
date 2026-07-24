namespace Factory
{
    public interface IConnectable
    {
        public Connection[] OutputConnections { get; set; }
        public Connection[] InputConnections { get; set; }
    }
}