namespace Traffic.Interface
{
    public interface IOrbit
    {
        int Craters { get; }
        int DistanceInMM { get; }
        string Name { get; }
    }
}