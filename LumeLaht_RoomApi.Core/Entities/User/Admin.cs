using LumeLaht_RoomApi.Core_.Interfaces;

namespace LumeLaht_RoomApi.Core_.Entities.User
{
    public class Admin : IUser
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Login { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmed { get; set; }
        public string EmailConfirmed { get; set; }
        public string Phone { get; set; }
    }
}
