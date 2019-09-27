using ActioBP.General.HttpModels;
using ActioBP.Identity.Models;
using ActioBP.Identity.ModelsJS;
using ActioBP.Linq.FilterLinq;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActioBP.Identity.Interfaces
{
    public interface IAuthRespository
    {
        MyUser GetUser(Guid id);

        MyUser GetUser(string username, string password);

        MyUser GetUser(string username);

        CollectionList<MyUser> GetUserCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pageSize = 10, string sortName = "", bool sortDescending = false);

        int GetUserListCount(string filter = "", List<FilterCriteria> filterArr = null);
        IEnumerable<MyUser> GetUserList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pageSize = 10, string sortName = "Username", bool sortDescending = false);      
        MyUser AddUser(MyUser m);


        MyUser AddUser(string username, string password, string email, ref IdentityResult result, string phoneNumber = null);

        MyUser AddUser(MyUser user, string password, ref IdentityResult result);

        Task<IdentityResult> UpdateUserAsync(Guid userId, string username = null, string password = null, string email = null, string phoneNumber = null);
        Task<IdentityResult> UpdateUserAsync(MyUser m);


        MyUser UpdateUserPassword(Guid id_user, string password);


        Task<IdentityResult> DeleteUserAsync(Guid id);
        Task<IdentityResult> DeleteUserAsync(MyUser user);
        bool IsUserInRole(Guid id_user, string rolename);
        bool IsUserInRole(string username, string rolename);
        Task<List<string>> GetRoleNamesByUserAsync(string user);
        Task<List<string>> GetRoleNamesByUserAsync(MyUser user);
        MyRole GetRol(Guid id);
      
        MyRole GetRol(string username);

        Task<IEnumerable<MyRole>> AddToRoleAsync(MyUser user, Guid roleId);
        Task<IEnumerable<MyRole>> RemoveFromRoleAsync(MyUser user, Guid roleId);

        CollectionList<MyRole> GetRolCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pageSize = 10, string sortName = "Name", bool sortDescending = false);
      

        int GetRolListCount(string filter = "", List<FilterCriteria> filterArr = null);
   
        IEnumerable<MyRole> GetRolList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pageSize = 10, string sortName = "Name", bool sortDescending = false);
      

        CollectionList<MyRole> GetRolCollectionListByUser(Guid id_user, string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pageSize = 10, string sortName = "", bool sortDescending = false);
      
        int GetRolListCountByUser(Guid id_user, string filter = "", List<FilterCriteria> filterArr = null);
      
        IEnumerable<MyRole> GetRolListByUser(Guid id_user, string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pageSize = 10, string sortName = "Name", bool sortDescending = false);
   
      

        AppClient FindClient(string clientId);
        Task<bool> AddRefreshTokenAsync(AppRefreshToken token);

        Task<bool> RemoveRefreshTokenAsync(string refreshTokenId);

        Task<bool> RemoveRefreshTokenAsync(AppRefreshToken refreshToken);
        AppRefreshToken GetRefreshToken(string refreshTokenId);
        Task<AppRefreshToken> FindRefreshTokenAsync(string refreshTokenId);
        List<AppRefreshToken> GetAllRefreshTokens();

        Task<LogInResult> CheckCredentialsAsync(string username, string password, bool skipEmailConfirmed = false);
        Task<SignInResult> LogInAsync(MyUser user, string password);
        Task<SignInResult> LogInAsync(string username, string password);
        Task<SignInResult> LogInCookieAsync(string username, string password, bool isPersistent = true);
        Task<SignInResult> LogInCookieAsync(MyUser user, string password, bool isPersistent = true);


        Task<IdentityResult> SetUserAsLockedAsync(Guid user, DateTimeOffset? endDate = null);
        Task<IdentityResult> SetUserAsLockedAsync(MyUser user, DateTimeOffset? endDate = null);
        Task<IdentityResult> SetUserAsUnLockedAsync(Guid user, DateTimeOffset? endDate = null);
        Task<IdentityResult> SetUserAsUnLockedAsync(MyUser user, DateTimeOffset? endDate = null);
        Task<IdentityResult> SetUserLockStatusAsync(Guid userId, bool asLocked, DateTimeOffset? endDate = null);
        Task<IdentityResult> SetUserLockStatusAsync(MyUser user, bool asLocked, DateTimeOffset? endDate = null);
    }
}
