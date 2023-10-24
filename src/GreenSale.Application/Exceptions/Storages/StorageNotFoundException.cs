namespace GreenSale.Application.Exceptions.Storages;

public class StorageNotFoundException : NotFoundException
{
    public StorageNotFoundException()
    {
        this.TitleMessage = "Storage not found!";
    }
}