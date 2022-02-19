using System.Threading.Tasks;

namespace Arcestio.Core.Interfaces
{
	public interface IPerformer
	{
		public Task Perform(string sql);
	}
}