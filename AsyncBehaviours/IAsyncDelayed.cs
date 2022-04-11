using System.Threading;
using System.Threading.Tasks;

namespace AsyncBehaviours
{
    public interface IAsyncDelayed
    {
        bool isWaiting { get; }
        Task Fire(CancellationToken cancellationToken=default);
    }
}