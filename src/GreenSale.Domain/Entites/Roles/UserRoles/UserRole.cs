namespace GreenSale.Domain.Entites.Roles.UserRoles
{
    public class UserRole : Auditable
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
    }
}
