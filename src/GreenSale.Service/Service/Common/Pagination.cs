using GreenSale.Application.Utils;
using GreenSale.Service.Interfaces.Common;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace GreenSale.Service.Service.Common;

public class Pagination : IPaginator
{
    private IHttpContextAccessor _accessor;

    public Pagination(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public void Paginate(long itemsCount, PaginationParams @params)
    {
        PaginationMetaData metaData = new PaginationMetaData();
        metaData.CurrentPage = @params.PageNumber;
        metaData.TotalItems = int.Parse(itemsCount.ToString());
        metaData.PageSize = @params.PageSize;

        metaData.TotalPages = (int)Math.Ceiling((double)itemsCount) / (@params.PageSize);
        if ((int)Math.Ceiling((double)itemsCount) % (@params.PageSize) > 0)
        {
            metaData.TotalPages += 1;
        }
        metaData.HasPrevious = metaData.CurrentPage > 1;
        metaData.HasNext = metaData.CurrentPage < metaData.TotalPages;

        string Convert = JsonConvert.SerializeObject(metaData);
        _accessor.HttpContext!.Response.Headers.Add("x-pagination", Convert);
    }
}
