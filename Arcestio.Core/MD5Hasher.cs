using Arcestio.Core.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Arcestio.Core
{
	public class MD5Hasher : IHasher
	{
		public string Hash(string content)
		{
			using var md5 = MD5.Create();
			var inputBytes = Encoding.ASCII.GetBytes(content);
			var hashBytes = md5.ComputeHash(inputBytes);

			var sb = new StringBuilder();
			foreach (var t in hashBytes)
				sb.Append(t.ToString("X2"));

			return sb.ToString();
		}
	}
}