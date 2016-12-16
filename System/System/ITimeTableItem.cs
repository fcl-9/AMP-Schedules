namespace System
{
    public interface  ITimeTableItem
    {
        DateTime StarTime { get; set; }
        DateTime EndTime { get; set; }
        DateTime Day { get; set; }
    }
}