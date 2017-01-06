using System.Data.Entity;

namespace HRIS.Data
{
    public class HRISDBInitializer : DropCreateDatabaseIfModelChanges<HRISContext>
    {
    }
}