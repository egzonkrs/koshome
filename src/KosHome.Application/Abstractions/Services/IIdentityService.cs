using System.Threading.Tasks;
using FluentResults;

namespace KosHome.Application.Abstractions.Services;

/// <summary>
/// 
/// </summary>
public interface IIdentityService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<Result> RegisterUserAsync(string email, string password);
}