using CodingTracker.Models;
using CodingTracker.Repository;
using CodingTracker.UI;

namespace CodingTracker.Controllers
{
    internal class CodingSessionsController : BaseController, ICodingSessionsController
    {
        private readonly ICodingSessionsRepository _repository;
        
        public CodingSessionsController(ICodingSessionsRepository repository)
        {
            _repository = repository;
        }

        public void ViewSessions()
        {
            var sessions = _repository.GetAll();

            if (sessions.Any())
            {
                DisplayTable(sessions, "ALL OF THEM");
            }
            else
            {
                DisplayMessage("Sessions was not found :(", color: "blue");
            }
        }

        public void AddSession()
        {
            var dateTimeUserInput = DateTimeInput();

            var newSession = new CodingSession { StartTime = dateTimeUserInput.startTime, EndTime = dateTimeUserInput.endTime };

            int newId = _repository.Create(newSession);

            DisplayMessage($"Session #{newId} was added successfully!");
        }

        public void DeleteSession()
        {
            ViewSessions();
            int userIdInput = InputId();

            bool isDeleted = _repository.Delete(userIdInput);

            if (isDeleted) DisplayMessage($"The session with this ID - {userIdInput} was deleted.");
            else DisplayError($"The session with that ID - {userIdInput} does not exist.");
        }
        public void UpdateSession()
        {
            ViewSessions();
            int userIdInput = InputId();
            var session = _repository.GetById(userIdInput);

            if (session is null)
            {
                DisplayError("No session with this ID was found");
                return;
            }

            CodingSession updatedSession = DateTimeUpdateInput(session);

            bool isUpdated = _repository.Update(updatedSession);

            if (isUpdated) DisplayMessage($"The session with this ID - {userIdInput} was updated.");
            else DisplayError($"The session with that ID - {userIdInput} does not exist.");
        }
    }
}
