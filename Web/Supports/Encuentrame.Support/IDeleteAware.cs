namespace Encuentrame.Support
{
    public interface IDeleteAware : IDeleteable
    {
        void OnPreDelete();
    }
}