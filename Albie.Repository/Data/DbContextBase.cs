using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Albie.Models;
using System.Web;

namespace Albie.Repository.Data
{
    public class DBContextBase<T> : DbContext where T : DbContext, IDisposable
    {
        public override int SaveChanges()
        {
            AddBasicInfo();
            return base.SaveChanges();
        }

        public DBContextBase(DbContextOptions options)
            : base(options)
        {
        }

        public DBContextBase()
        {
        }
        #region methods
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        #endregion


        private void AddBasicInfo()
        {
            var entries = ChangeTracker.Entries();
            var entities = entries.Where(x => x.Entity is EntityBase && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                //if (entity.State == EntityState.Added)
                //{
                //    var thisEntity = (EntityBase)entity.Entity;
                //    if (thisEntity.Id == Guid.Empty || thisEntity.F_Creacion.Ticks == 0)
                //    {
                //        thisEntity.Id = Guid.NewGuid();
                //        thisEntity.UsuarioCreacion = GetUserId();
                //        thisEntity.F_Creacion = DateTime.UtcNow;
                //        thisEntity.F_Modificacion = null;
                //        thisEntity.UsuarioModificacion = null;
                //        //thisEntity.F_Created = new DateTimeOffset(DateTime.Now, TimeSpan.FromMinutes(s.TzOffset * -1));
                //    }
                //}
                //else
                //{
                //    var thisEntity = (EntityBase)entity.Entity;
                //    thisEntity.UsuarioModificacion = GetUserId();
                //    thisEntity.F_Modificacion = DateTime.UtcNow;
                //}
            }
        }
        /// <summary>
        /// Recupera el ID del usuario que guarda los datos en ese momento en la base de datos,
        /// usando el Helper de HttpContextCore
        /// </summary>
        /// <returns>The user identifier.</returns>
        private Guid GetUserId()
        {
            var currentUser = GetCurrentUser();
            var identity = (System.Security.Claims.ClaimsIdentity)currentUser.Identity;
            IEnumerable<System.Security.Claims.Claim> claims = identity.Claims;

            var id = identity.Claims.SingleOrDefault(o => o.Type == "id");
            if (id == null) return Guid.Empty;
            return Guid.Parse(id.Value);
        }
        private string GetUserName()
        {
            var currentUser = GetCurrentUser();
            return currentUser.Identity?.Name;
        }
        private System.Security.Claims.ClaimsPrincipal GetCurrentUser()
        {
            return HttpContextCore.Current.User;
        }
    }
}
