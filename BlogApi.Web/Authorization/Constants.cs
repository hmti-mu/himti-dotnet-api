namespace BlogApi.Web.Authorization
{
    public static class PolicyNames
    {
        public const string RequireUser = "RequireUser";
        public const string RequireAuthor = "RequireAuthor";
        public const string RequireEditor = "RequireEditor";
        public const string RequireAdmin = "RequireAdmin";
        public const string RequireSuperAdmin = "RequireSuperAdmin";
    }

    public static class RoleNames
    {
        public const string Guest = "Guest";
        public const string User = "User";
        public const string Author = "Author";
        public const string Editor = "Editor";
        public const string Admin = "Admin";
        public const string SuperAdmin = "SuperAdmin";
    }

    public static class RoleLevels
    {
        public const int Guest = 0;
        public const int User = 1;
        public const int Author = 2;
        public const int Editor = 3;
        public const int Admin = 4;
        public const int SuperAdmin = 5;
    }
}