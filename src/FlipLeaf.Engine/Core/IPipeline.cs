using System.Threading.Tasks;

namespace FlipLeaf.Core
{
    public interface IPipeline
    {
        bool Accept(IWebSite site, IInput input);

        Task<InputItems> PrepareAsync(IWebSite site, IInput input);

        Task TransformAsync(IWebSite site, IInput input);
    }
}
