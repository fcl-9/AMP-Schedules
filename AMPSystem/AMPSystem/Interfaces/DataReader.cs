namespace AMPSystem.Interfaces
{
    public interface DataReader
    {
        /// <summary>
        /// Requests information about the teachers of the university
        /// </summary>
        /// <returns>A JSON in string format</returns>
        string RequestTeachers();
        
        /// <summary>
        /// Requests information about the courses of a given user
        /// </summary>
        /// <returns>A JSON in string format</returns>
        string RequestUserCourses(string username);
        
        /// <summary>
        /// Requests information about the courses of the university
        /// </summary>
        /// <returns>A JSON in string format</returns>
        string RequestCourses();
        
        /// <summary>
        /// Requests information about the rooms of the university
        /// </summary>
        /// <returns>A JSON in string format</returns>
        string RequestRooms();

        /// <summary>
        /// Requests information about the schedule of a given user
        /// </summary>
        /// <returns>A JSON in string format</returns>
        string RequestSchedule(string username);
    }
}