namespace LumaCove_Api.Domain.Interfaces
{
    public interface IUser
    {
        int UserId { get; set; }
        string Name {  get; set; }
        string Surname {  get; set; }
        string Login {  get; set; }
        int Age {  get; set; }
        string Email { get; set; }
        string Password { get; set; }
        string PasswordConfirmed { get; set; }
        string EmailConfirmed { get; set; }
        string Phone { get; set; }

    }
}
