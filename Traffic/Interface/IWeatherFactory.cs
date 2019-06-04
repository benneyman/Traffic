using Traffic.Enum;

namespace Traffic.Interface
{
    public interface IWeatherFactory
    {
        int GetCraterChangePercentage(WeatherConditions weatherCondition);
    }
}