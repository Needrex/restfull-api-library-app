namespace RestApiApp.InterfaceServices
{
    public interface IRazorViewRenderer
    {
        Task<string> RenderViewAsync<T>(string viewName, T model);
    }
}