using DNCS.Config;
using DNCS.Data.Entity.Content;
using DNCS.Data.Entity.Extensions;
using DNCS.Data.Entity.System;
using DNCS.Data.Entity.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DNCS.Data.Entity
{
    public class CustumDbContext : DbContext
    {
        public CustumDbContext() : base()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConfigManager.Now.ConnectionStrings.NpgsqlDbConnectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //FamilyDiary user's mood
            //modelBuilder.Entity<FamilyDiary>()
            //    .HasMany(p => p.UserMood)
            //    .WithMany(p => p.FamilyDiary)
            //    .Map(p=> {
            //        p.MapLeftKey("Mid");
            //        p.MapRightKey("Id");
            //        p.ToTable("Fam_FamilyDiaryUserMoods");
            //    });

            modelBuilder.HasDefaultSchema("public");
            modelBuilder.UseSerialColumns();

            base.OnModelCreating(modelBuilder);

            #region 全部表名转小写

            // 设置转大写为false
            DbTableViewExtensions.UseUpperCase = false;

            // 将名为 YourContext 中的所有 DbSet 和 DbQuery 的表名、视图名、列名转换为小写
            // 参数位 true,则 自动跳过处理 DbContext 程序集中已实现的 IEntityTypeConfiguration 和 IQueryTypeConfiguration
            modelBuilder.SetAllDbSetTableNameAndColumnName<CustumDbContext>(true);
            modelBuilder.SetAllDbQueryViewNameAndColumnName<CustumDbContext>(true);

            #endregion
        }


        #region System
        public DbSet<Role> Role { get; set; }

        public DbSet<RolePermission> RolePermission { get; set; }

        /// <summary>
        /// 系统日志
        /// </summary>
        public DbSet<SystemLog> SystemLog { get; set; }

        #endregion

        #region User
        public DbSet<LoginHistory> LoginHistory { get; set; }

        public DbSet<UserInfo> UserInfo { get; set; }

        public DbSet<UserPasswd> UserPasswd { get; set; }

        public DbSet<UserValidateLog> UserValidateLog { get; set; }

        public DbSet<UserRoles> UserRoles { get; set; }

        #endregion

        #region Content

        public DbSet<ContentInfo> ContentInfo { get; set; }

        public DbSet<UserCollection> UserCollection { get; set; }

        public DbSet<ContentType> ContentType { get; set; }

        #endregion

    }
}
