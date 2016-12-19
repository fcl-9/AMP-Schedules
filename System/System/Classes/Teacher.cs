namespace System
{
    public class Teacher : User
    {
        public string Email { get; set; }

        public Room Room { get; set; }

        public Teacher(string aUsername, string aPassword,string aEmail, Room aRoom)
        {
            Username = aUsername;
            Password = aPassword;
            Email = aEmail;
            Room = aRoom;
        }
    }
}