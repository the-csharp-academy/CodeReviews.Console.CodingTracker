using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.Entities
{
    public class CodingSessionChoice
    {
        public CodingSession? Session { get; }
        public bool IsCancel { get; }

        public CodingSessionChoice(CodingSession session)
        {
            Session = session;
        }

        public CodingSessionChoice()
        {
            IsCancel = true;
        }
    }
}
