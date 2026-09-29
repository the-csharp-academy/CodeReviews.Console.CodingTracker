using CodingTracker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.Repository
{
    internal interface ICodingSessionsRepository
    {
        List<CodingSession> GetAll();
        CodingSession? GetById(int id);
        int Create(CodingSession session);
        bool Delete(int id);
        bool Update(CodingSession session);
    }
}
