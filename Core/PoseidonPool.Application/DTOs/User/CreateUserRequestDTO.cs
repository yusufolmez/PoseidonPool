namespace PoseidonPool.Application.DTOs.User
{
    public class CreateUserRequestDTO
    {
        public string NameSurname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
