using CodingTracker.Controllers;

CodingController codingController = new CodingController();
DataBaseController.CreateTableIfNotExists(codingController.connectionString);
codingController.SeedDatabase();
codingController.BootMenu();