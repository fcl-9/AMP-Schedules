namespace System
{
    public interface IViewHandler:IObserver
    {
        void RenderView(int viewType);
    }
}