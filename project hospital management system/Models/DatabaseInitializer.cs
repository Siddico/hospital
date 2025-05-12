using System.Data.Entity;

namespace project_hospital_management_system.Models
{
    public class DatabaseInitializer : IDatabaseInitializer<ApplicationDbContext>
    {
        public void InitializeDatabase(ApplicationDbContext context)
        {
            if (!context.Database.Exists())
            {
                // إنشاء قاعدة البيانات كود هنا
                context.Database.Create();
                // يمكنك إضافة بيانات ابتدائية هنا إذا أردت
            }
        }
    }
}