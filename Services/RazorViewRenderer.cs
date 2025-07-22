using RazorLight;
using RestApiApp.InterfaceServices;

namespace RestApiApp.Services
{
    public class RazorViewRenderer : IRazorViewRenderer
    {
        private readonly RazorLightEngine _engine;

        public RazorViewRenderer()
        {
            _engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(Path.Combine(Directory.GetCurrentDirectory(), "Templates"))
                .UseMemoryCachingProvider()
                .Build();
        }

        public Task<string> RenderViewAsync<T>(string fileName, T model)
        {
            return _engine.CompileRenderAsync(fileName, model);
        }
    }
}