namespace Application.Interfaces.ThirdParty
{
    public interface ILocationService
    {
        string GetGovernorateName(int id);

        string GetCityName(int governorateId, int cityId);

        bool CityBelongsToGovernorate(int governorateId, int cityId);
    }
}
