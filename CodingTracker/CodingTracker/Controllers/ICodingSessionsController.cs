using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.Controllers
{
    internal interface ICodingSessionsController
    {
        void ViewSessions();
        void AddSession();
        void DeleteSession();
        void UpdateSession();
    }
}
