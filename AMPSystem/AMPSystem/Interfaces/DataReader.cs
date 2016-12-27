namespace AMPSystem.Interfaces
{
    public interface DataReader
    {
        string RequestTeachers();
        string RequestUserCourses(string username);
        string RequestCourses();
        string RequestRooms();
        string RequestSchedule(string username);
    }
}