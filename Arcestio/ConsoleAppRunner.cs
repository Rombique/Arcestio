using System.Threading.Tasks;
using Arcestio.Core.Interfaces;

namespace Arcestio
{
	public class ConsoleAppRunner : IConsoleAppRunner
	{
		private readonly IWorker _worker;

		public ConsoleAppRunner(IWorker worker)
		{
			_worker = worker;
		}
		
		public async Task<int> Main(params string[] args)
		{
			await _worker.DoWorkAsync();
			return 0;
		}
	}
}