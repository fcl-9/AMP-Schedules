namespace AMPSystem.Interfaces
{
    public interface IViewHandler:IObserver
    {
        void RenderView(int viewType);
    }
}