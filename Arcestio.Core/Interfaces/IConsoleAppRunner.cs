using System.Threading.Tasks;

namespace Arcestio.Core.Interfaces
{
	public interface IConsoleAppRunner
	{
		Task<int> Main(params string[] args);
	}
}