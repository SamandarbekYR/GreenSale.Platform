namespace GreenSale.DataAccess.Common
{
    public interface ISearchable<TViewModel>
    {
        public Task<(int ItemsCount, List<TViewModel>)> SearchAsync(string search);
    }
}