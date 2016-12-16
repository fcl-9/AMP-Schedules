namespace System
{
    public interface  TimeTableItem
    {
        DateTime StarTime { get; set; }
        DateTime EndTime { get; set; }
        DateTime Day { get; set; }
    }
}