namespace GreenSale.Domain.Entites.Storages
{
    public class Storage : Auditable
    {
        public long UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double AddressLatitude { get; set; }
        public double AddressLongitude { get; set; }
        public string Info { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
    }
}
