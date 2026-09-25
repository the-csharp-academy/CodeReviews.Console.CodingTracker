namespace Coding.Tracker
{
    public interface ICodeSessionService
    {
        bool CreateDatabase();
        bool Create(CodeSession session);
        List<CodeSession> ReadAllData();
        bool Update(CodeSession session);
        bool Delete(int id);
    }
}
