using GreenSale.Application.Utils;

namespace GreenSale.Service.Interfaces.Common;

public interface IPaginator
{
    public void Paginate(long itemsCount, PaginationParams @params);
}
