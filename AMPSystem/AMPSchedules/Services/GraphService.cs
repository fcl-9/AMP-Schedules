namespace AMPSchedules.Services
{
    public static class GraphService 
    {
        public static IGraphService Instance { get; } = new GraphServiceGraph();
    }
}