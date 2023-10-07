using System.Diagnostics;
using System.Reflection;

namespace AV00_Control_Application.Utilities
{
    public static class DbPath
    {
        public static string GetPath(string DbName)
        {
            string execDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string sqliteDbPath = Directory.GetParent(execDir).Parent.Parent.Parent.Parent.FullName;
            sqliteDbPath = Path.Combine(sqliteDbPath, "Database");
            sqliteDbPath = Path.Combine(sqliteDbPath, DbName);
            Trace.WriteLine($"Database path: {sqliteDbPath}");
            return sqliteDbPath;
        }
    }
}
