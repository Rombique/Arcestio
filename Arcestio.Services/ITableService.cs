using System.Threading.Tasks;

namespace Arcestio.Services
{
	public interface ITableService
	{
		public Task<bool> CreateTableAsync() 
	}
}