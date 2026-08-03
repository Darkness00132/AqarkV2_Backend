namespace Application.Interfaces.Credits
{
    public interface ICreditsRepo
    {
        Task<bool> DeductAsync(Guid userId, int cost, CancellationToken ct = default);
    }
}
