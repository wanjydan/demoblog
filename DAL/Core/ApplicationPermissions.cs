using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DAL.Core
{
    public static class ApplicationPermissions
    {
        private const string UsersPermissionGroupName = "User Permissions";

        private const string RolesPermissionGroupName = "Role Permissions";

        private const string ArticlePermissionGroupName = "Article Permission";
        public static readonly ReadOnlyCollection<ApplicationPermission> AllPermissions;

        public static readonly ApplicationPermission ViewUsers = new ApplicationPermission("View Users", "users.view",
            UsersPermissionGroupName, "Permission to view other users account details");

        public static readonly ApplicationPermission ManageUsers = new ApplicationPermission("Manage Users",
            "users.manage", UsersPermissionGroupName,
            "Permission to create, delete and modify other users account details");

        public static readonly ApplicationPermission ViewRoles = new ApplicationPermission("View Roles", "roles.view",
            RolesPermissionGroupName, "Permission to view available roles");

        public static readonly ApplicationPermission ManageRoles = new ApplicationPermission("Manage Roles",
            "roles.manage", RolesPermissionGroupName, "Permission to create, delete and modify roles");

        public static readonly ApplicationPermission AssignRoles = new ApplicationPermission("Assign Roles",
            "roles.assign", RolesPermissionGroupName, "Permission to assign roles to users");

        public static readonly ApplicationPermission ManageArticles = new ApplicationPermission("Manage Articles",
            "articles.manage", ArticlePermissionGroupName, "Permission to create, delete and modify articles");


        static ApplicationPermissions()
        {
            var allPermissions = new List<ApplicationPermission>
            {
                ViewUsers,
                ManageUsers,

                ViewRoles,
                ManageRoles,
                AssignRoles,

                ManageArticles
            };

            AllPermissions = allPermissions.AsReadOnly();
        }

        public static ApplicationPermission GetPermissionByName(string permissionName)
        {
            return AllPermissions.FirstOrDefault(p => p.Name == permissionName);
        }

        public static ApplicationPermission GetPermissionByValue(string permissionValue)
        {
            return AllPermissions.FirstOrDefault(p => p.Value == permissionValue);
        }

        public static string[] GetAllPermissionValues()
        {
            return AllPermissions.Select(p => p.Value).ToArray();
        }

        public static string[] GetAdministrativePermissionValues()
        {
            return new string[] {ManageUsers, ManageRoles, AssignRoles, ManageArticles};
        }
    }


    public class ApplicationPermission
    {
        public ApplicationPermission()
        {
        }

        public ApplicationPermission(string name, string value, string groupName, string description = null)
        {
            Name = name;
            Value = value;
            GroupName = groupName;
            Description = description;
        }


        public string Name { get; set; }
        public string Value { get; set; }
        private string GroupName { get; }
        private string Description { get; }


        public override string ToString()
        {
            return Value;
        }


        public static implicit operator string(ApplicationPermission permission)
        {
            return permission.Value;
        }
    }
}