namespace Domain.Enums
{
    public enum UserRoles
    {
        User = 1, // access to see ads only
        Broker = 2, // access to make his own ads
        Admin = 3, // access to manage users and ads
        SuperAdmin = 4 // Full control, can create/remove admins, brokers, etc
    }
}
