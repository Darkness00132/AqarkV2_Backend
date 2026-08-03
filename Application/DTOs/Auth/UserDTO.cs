

namespace Application.DTOs.User
{
    public class UserDTO
    {
        public required Guid Id { get; set; }

        public required string Name { get; set; }

        public required string Email { get; set; }

        public string? profilePhoto { get; set; }

        public required int Credits { get; set; }
    }
}
