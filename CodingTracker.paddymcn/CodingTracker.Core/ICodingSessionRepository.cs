using CodingTracker.Core.DataModel;

namespace CodingTracker.Core;

public interface ICodingSessionRepository
{
    bool Delete(int id);
    void EnsureCreated();
    List<CodingSession> GetAll();
    CodingSession? GetById(int id);
    CodingSession Add(CodingSession codingSession);
    bool Update(CodingSession codingSession);
}