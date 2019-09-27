using ActioBP.Identity.ModelsJS;
using ActioBP.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ActioBP.Identity.Interfaces;
using ActioBP.General.HttpModels;
using ActioBP.Linq.FilterLinq;

namespace ActioBP.Identity.EF
{
    public class AuthRepository : IAuthRespository
    {
        private MyUserManager uM;
        private MySignInManager sM;
        protected MyDbContext db = null;
        
        public AuthRepository(MyDbContext dbBase, MySignInManager MySignInManager, MyUserManager userManager)
        {
            db = dbBase;
            uM = userManager;
            sM = MySignInManager;
        }

        #region SignInManager
        #region CheckLogin
        /// <summary>
        /// Comprueba el inicio de sesión de un usuario y devuelve un estado que indica si ha sido exitoso o no. De no serlo, la respuesta indica la razón.
        /// </summary>
        /// <param name="username">Nombre de usuario.</param>
        /// <param name="password">Contraseña.</param>
        /// <param name="skipEmailConfirmed">¿Debe validarse que el email haya sido confirmado?</param>
        /// <returns></returns>
        public async Task<LogInResult> CheckCredentialsAsync(string username, string password, bool skipEmailConfirmed = false)
        {
            LogInResult result = new LogInResult(LoginStatusTypes.UnknownError);

            try
            {
                var user = await uM.FindByNameAsync(username);
                result = new LogInResult(LoginStatusTypes.UnknownError, user);
                if (user == null)
                {
                    //No existe
                    result.Status = LoginStatusTypes.DoesNotExist;
                    return result;
                }

                if (user.LockoutEnabled)
                {
                    bool haltAndReturn = true;
                    //Usuario bloqueado
                    if (user.LockoutEnd.HasValue)
                    {
                        //El bloqueo es temporal.
                        if (user.LockoutEnd.Value <= DateTimeOffset.Now)
                        {
                            //El bloqueo ha expirado: desbloquear & continuar.
                            await uM.SetLockoutEnabledAsync(user, false);
                            haltAndReturn = false;
                        }
                        else
                        {
                            ///El bloqueo sigue activo.
                            result.Status = LoginStatusTypes.UserLocked_Temporal;
                        }
                    }
                    else
                    {
                        //El bloqueo es permanente
                        result.Status = LoginStatusTypes.UserLocked_Permanent;
                    }
                    if (haltAndReturn) return result;
                }

                if (!skipEmailConfirmed)
                {
                    if (!user.EmailConfirmed) result.Status = LoginStatusTypes.EmailNotConfirmed;
                    return result;
                }

                bool isPasswordValid = await uM.CheckPasswordAsync(user, password);
                if (!isPasswordValid)
                {
                    result.Status = LoginStatusTypes.PasswordNotValid;
                    return result;
                }
                else
                {
                    result.Status = LoginStatusTypes.Success;
                }
            }
            catch (Exception)
            {

            }
            
            return result;
        }


        #region LogIn con Cookie
        /// <summary>
        /// Realiza el inicio de sesión para un usuario con una password.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="isPersistent">Habilitar o no cookie para sesión persistente, tras cerrar navegador</param>
        /// <returns></returns>
        public async Task<SignInResult> LogInCookieAsync(MyUser user, string password, bool isPersistent = true)
        {
            var result = await sM.PasswordSignInAsync(user, password, isPersistent, false);
            return result;
        }
        /// <summary>
        /// Realiza el inicio de sesión para un usuario con una password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="isPersistent">Habilitar o no cookie para sesión persistente, tras cerrar navegador</param>
        /// <returns></returns>
        public async Task<SignInResult> LogInCookieAsync(string username, string password, bool isPersistent = true)
        {
            var result = await sM.PasswordSignInAsync(username, password, isPersistent, false);
            return result;
        }
        public async Task<SignInResult> LogInAsync(MyUser user, string password)
        {
            var result = await sM.CheckPasswordSignInAsync(user, password, false);
            return result;
        }
        public async Task<SignInResult> LogInAsync(string username, string password)
        {
            var user = await uM.FindByNameAsync(username);
            if (user == null)return new SignInResult();

            var result = await sM.CheckPasswordSignInAsync(user, password, false);
            return result;
        }
        #endregion
        #region LogIn sin Cookie
        //https://github.com/aspnet/Identity/issues/1376#issuecomment-331116870

        #endregion

        #endregion
        #endregion

        #region Users

        #region GET
        public MyUser GetUser(Guid id)
        {
            return this.db.Users
                .SingleOrDefault(o => o.Id == id);
        }
        public MyUser GetUser(string username, string password)
        {
            return db.Users
                .SingleOrDefault(o => o.UserName == username && o.PasswordHash == password);
        }
        public MyUser GetUser(string username)
        {
            return db.Users
                .SingleOrDefault(o => o.UserName.Equals(username));
        }

        public CollectionList<MyUser> GetUserCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pageSize = 10, string sortName = "", bool sortDescending = false)
        {
            var total = GetUserListCount(filter, filterArr);

            if (total == 0) return new CollectionList<MyUser>();

            var items = GetUserList(filter, filterArr, pageIndex, pageSize, "username", sortDescending);

            return new CollectionList<MyUser>()
            {
                Items = items,
                Total = total
            };
        }

        public int GetUserListCount(string filter = "", List<FilterCriteria> filterArr = null)
        {
            IQueryable<MyUser> lista = this.db.Users.
                                        WhereAct(filterArr, filter, fieldFilter: "Username", opFilter: FilterOperator.Cn);

            return lista.Count();
        }
        public IEnumerable<MyUser> GetUserList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pageSize = 10, string sortName = "Username", bool sortDescending = false)
        {
            IQueryable<MyUser> lista = this.db.Users.
                                        OrderByAct(sortName, sortDescending).
                                        WhereAct(filterArr, filter, fieldFilter: "username", opFilter: FilterOperator.Cn);

            return lista.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }

        #endregion

        #region ADD UPDATE DELETE

        public MyUser AddUser(MyUser m)
        {
            return AddUser(m, commit: true);
        }
        public MyUser AddUser(MyUser m, bool commit)
        {

            this.db.Users.Add(m);
            if (commit) db.SaveChanges();
            return m;

        }
        public MyUser AddUser(string username, string password, string email,  ref IdentityResult result, string phoneNumber = null)
        {
            var myUser = new MyUser
            {
                Id = Guid.NewGuid(),
                UserName = username,
                Email = email,
                PhoneNumber = phoneNumber
            };

            return AddUser(myUser, password, ref result);
        }
        public MyUser AddUser(MyUser user,string password, ref IdentityResult result)
        {
            user.LockoutEnabled = false;
            var order = uM.CreateAsync(user, password);
            order.Wait();

            result= order.Result;
            if (result.Succeeded) return user;
            else
                return null;
        }
        public async Task<IdentityResult> UpdateUserAsync(Guid userId, string username = null, string password = null, string email = null, string phoneNumber = null)
        {
            var result = new IdentityResult();
            var user = GetUser(userId);
            
            if (!string.IsNullOrWhiteSpace(username))
            {
                result = await uM.SetUserNameAsync(user, username);
                if (!result.Succeeded) return result;
            }

            if (!string.IsNullOrWhiteSpace(password))
            {
                result = await uM.RemovePasswordAsync(user);
                if (!result.Succeeded) return result;
                result = await uM.AddPasswordAsync(user, password);
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                result = await uM.SetEmailAsync(user, email);
                if (!result.Succeeded) return result;
            }

            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                result = await uM.SetPhoneNumberAsync(user, phoneNumber);
                if (!result.Succeeded) return result;
            }

            return result;
        }
        public async Task<IdentityResult> UpdateUserAsync(MyUser m)
        {
            return await uM.UpdateAsync(m);
        }

        public MyUser UpdateUserPassword(Guid id_user, string password)
        {
            return UpdateUserPassword(id_user, password, commit: true);
        }
        public MyUser UpdateUserPassword(Guid id_user,string password, bool commit)
        {
            var mDB = GetUser(id_user);

            mDB.PasswordHash = uM.PasswordHasher.HashPassword(mDB,password);

            if (commit) db.SaveChanges();
            return mDB;
        }

        //public bool DeleteUser(Guid id)
        //{
        //    return DeleteUser(id, commit: true);
        //}
        //private bool DeleteUser(Guid id, bool commit)
        //{
        //    var mDB = GetUser(id);
        //    this.db.Users.Remove(mDB);

        //    if (commit) db.SaveChanges();
        //    return true;
        //}
        public async Task<IdentityResult> DeleteUserAsync(Guid id)
        {
            var user = GetUser(id);
            return await DeleteUserAsync(user);
        }
        public async Task<IdentityResult> DeleteUserAsync(MyUser user)
        {
            return await uM.DeleteAsync(user);
        }

        #endregion



        #endregion


        #region Roles

        #region Is user in Role
        public bool IsUserInRole(Guid id_user, string rolename)
        {
            return (from r in db.Roles
                    join u in db.UserRoles on r.Id equals u.RoleId
                    where u.UserId == id_user && r.Name == rolename
                    select r).Count()> 0;

        }
        public bool IsUserInRole(string username, string rolename)
        {
            return (from r in db.Roles
                    join ur in db.UserRoles on r.Id equals ur.RoleId
                    join u in db.Users on ur.UserId equals u.Id
                    where u.NormalizedUserName == username && r.Name == rolename
                    select r).Count() > 0;
        }

        #endregion
        #region GET
        public async Task<List<string>> GetRoleNamesByUserAsync(string username)
        {
            var user = await uM.FindByNameAsync(username);
            var roles = await GetRoleNamesByUserAsync(user);
            return roles;
        }
        public async Task<List<string>> GetRoleNamesByUserAsync(MyUser user)
        {
            if (user == null) return new List<string>();
            var roles = await uM.GetRolesAsync(user);
            return roles.ToList();
        }
        public MyRole GetRol(Guid id)
        {
            return this.db.Roles.
                Single(o => o.Id == id);
        }
        public MyRole GetRol(string username)
        {
            return db.Roles.SingleOrDefault(o => o.Name == username);
        }

        public CollectionList<MyRole> GetRolCollectionList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pageSize = 10, string sortName = "Name", bool sortDescending = false)
        {
            var total = GetRolListCount(filter, filterArr);

            if (total == 0) return new CollectionList<MyRole>();

            var items = GetRolList(filter, filterArr, pageIndex, pageSize, sortName, sortDescending);

            return new CollectionList<MyRole>()
            {
                Items = items,
                Total = total
            };
        }

        public int GetRolListCount(string filter = "", List<FilterCriteria> filterArr = null)
        {
            IQueryable<MyRole> lista = this.db.Roles.
                                        WhereAct(filterArr, filter, fieldFilter: "name", opFilter: FilterOperator.Cn);

            return lista.Count();
        }
        public IEnumerable<MyRole> GetRolList(string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pageSize = 10, string sortName = "Name", bool sortDescending = false)
        {
            IQueryable<MyRole> lista = this.db.Roles.
                                        OrderByAct(sortName, sortDescending).
                                        WhereAct(filterArr, filter, fieldFilter: "name", opFilter: FilterOperator.Cn);

            return lista.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }

        public CollectionList<MyRole> GetRolCollectionListByUser(Guid id_user,string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pageSize = 10, string sortName = "", bool sortDescending = false)
        {
            var total = GetRolListCountByUser(id_user,filter, filterArr);

            if (total == 0) return new CollectionList<MyRole>();

            var items = GetRolListByUser(id_user,filter, filterArr, pageIndex, pageSize, sortName, sortDescending);

            return new CollectionList<MyRole>()
            {
                Items = items,
                Total = total
            };
        }

        public int GetRolListCountByUser(Guid id_user, string filter = "", List<FilterCriteria> filterArr = null)
        {
            IQueryable<MyRole> lista = (from r in this.db.Roles
                                        join u in db.UserRoles on r.Id equals u.RoleId
                                        where u.UserId == id_user
                                        select r).
                                        WhereAct(filterArr, filter, fieldFilter: "name", opFilter: FilterOperator.Cn);

            return lista.Count();
        }
        public IEnumerable<MyRole> GetRolListByUser(Guid id_user,string filter = "", List<FilterCriteria> filterArr = null, int pageIndex = 0, int pageSize = 10, string sortName = "Name", bool sortDescending = false)
        {
            IQueryable<MyRole> lista = (from r in this.db.Roles
                                       join u in db.UserRoles on r.Id equals u.RoleId
                                       where u.UserId ==id_user
                                       select r ).
                                        OrderByAct(sortName, sortDescending).
                                        WhereAct(filterArr, filter, fieldFilter: "name", opFilter: FilterOperator.Cn);

            return lista.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }



        #endregion

        #region SET USER-ROLE
        /// <summary>
        /// Añade un usuario a un rol y devuelve la lista de roles a los que pertenece.
        /// </summary>
        /// <param name="user">Usuario con el que operar</param>
        /// <param name="roleId">ID del rol que debe ser asignado</param>
        /// <returns></returns>
        public async Task<IEnumerable<MyRole>> AddToRoleAsync(MyUser user, Guid roleId)
        {
            var role = GetRol(roleId);
            if (role != null)
            {
                try
                {
                    var result = await uM.AddToRoleAsync(user, role.Name);
                } catch(Exception e)
                {
                    throw e;
                }
            }
            return GetRolListByUser(user.Id);
        }
        /// <summary>
        /// Elimina un usuario de un rol y devuelve la lista de roles a los que pertenece.
        /// </summary>
        /// <param name="user">Usuario con el que operar</param>
        /// <param name="roleId">ID del rol que debe ser desasignado</param>
        /// <returns></returns>
        public async Task<IEnumerable<MyRole>> RemoveFromRoleAsync(MyUser user, Guid roleId)
        {
            var role = GetRol(roleId);
            if (role != null)
            {
                try
                {
                    var result = await uM.RemoveFromRoleAsync(user, role.Name);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return GetRolListByUser(user.Id);
        }
        #endregion

        #endregion


        #region Client

        public AppClient FindClient(string clientId)
        {
            var client = db.AppClient.Find(clientId);

            return client;
        }

        #endregion
        

        #region refresh token
        public async Task<bool> AddRefreshTokenAsync(AppRefreshToken token)
        {

            var existingToken = db.AppRefreshToken.Where(r => r.Subject == token.Subject && r.AppClientId == token.AppClientId).SingleOrDefault();

            if (existingToken != null)
            {
                var result = await RemoveRefreshTokenAsync(existingToken);
            }

            db.AppRefreshToken.Add(token);

            return await db.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshTokenAsync(string refreshTokenId)
        {
            var refreshToken = await db.AppRefreshToken.FindAsync(refreshTokenId);

            if (refreshToken != null) {
                db.AppRefreshToken.Remove(refreshToken);
                return await db.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshTokenAsync(AppRefreshToken refreshToken)
        {
            db.AppRefreshToken.Remove(refreshToken);
            return await db.SaveChangesAsync() > 0;
        }

        public AppRefreshToken GetRefreshToken(string refreshTokenId)
        {
            var refreshToken = db.AppRefreshToken.Find(refreshTokenId);

            return refreshToken;
        }
        public async Task<AppRefreshToken> FindRefreshTokenAsync(string refreshTokenId)
        {
            var refreshToken = await db.AppRefreshToken.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public List<AppRefreshToken> GetAllRefreshTokens()
        {
            return db.AppRefreshToken.ToList();
        }

        #endregion

        #region claims        

        #endregion


        #region extra features

        public async Task<IdentityResult> SetUserAsLockedAsync(Guid userId, DateTimeOffset? endDate = null)
        {
            return await SetUserLockStatusAsync(userId, true, endDate);
        }
        public async Task<IdentityResult> SetUserAsLockedAsync(MyUser user, DateTimeOffset? endDate = null)
        {
            return await SetUserLockStatusAsync(user, true, endDate);
        }
        public async Task<IdentityResult> SetUserAsUnLockedAsync(Guid userId, DateTimeOffset? endDate = null)
        {
            return await SetUserLockStatusAsync(userId, false, endDate);
        }
        public async Task<IdentityResult> SetUserAsUnLockedAsync(MyUser user, DateTimeOffset? endDate = null)
        {
            return await SetUserLockStatusAsync(user, false, endDate);
        }
        public async Task<IdentityResult> SetUserLockStatusAsync(Guid userId, bool asLocked, DateTimeOffset? endDate = null)
        {
            var user = GetUser(userId);
            return await SetUserLockStatusAsync(user, asLocked, endDate);
        }
        public async Task<IdentityResult> SetUserLockStatusAsync(MyUser user, bool asLocked, DateTimeOffset? endDate = null)
        {
            if (asLocked)
            {
                if(endDate.HasValue) return await uM.SetLockoutEndDateAsync(user, endDate);
            }

            return await uM.SetLockoutEnabledAsync(user, asLocked);
        }
        #endregion

        public void Dispose()
        {
            db.Dispose();
            uM.Dispose();

        }
    }
}