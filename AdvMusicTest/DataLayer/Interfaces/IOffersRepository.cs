using AdvMusicTest.DataLayer.Types;

namespace AdvMusicTest.DataLayer.Interfaces;

public interface IOffersRepository
{
    public Task InsertRecords(List<LROffer> offers);
    public Task<List<LROffer>> GetOffers(string query);
}