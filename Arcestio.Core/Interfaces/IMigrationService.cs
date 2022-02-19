using System.Threading.Tasks;

namespace Arcestio.Core.Interfaces
{
	public interface IMigrationService
	{
		public Task TryExecuteScript(string sql);
	}
}