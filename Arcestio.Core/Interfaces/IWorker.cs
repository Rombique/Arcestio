using System.Threading.Tasks;

namespace Arcestio.Core.Interfaces
{
	public interface IWorker
	{
		public Task DoWorkAsync();
	}
}