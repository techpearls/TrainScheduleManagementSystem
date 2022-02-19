`TrainScheduleManagementSystem` lets you easily create custom train schedule for a given station via friendly APIs.

Contents
========

 * [Design](#design)
 * [Installation](#installation)
 * [Assumptions](#assumptions)
 * [Usage](#usage)
 * [Future Considerations](#future-considerations)

### Design

The project is organized into Api, Services and Tests.
- The API layer does basic validation and acts as a wrapper.
- The services layer contains the core business logic and interacts with the database.
- The tests consists of unit tests for API, services and the database layer.

### Installation

1. Install [`Visual Studio`](https://visualstudio.microsoft.com/downloads/)
2. Clone git repo locally using `$ git clone <link-to-repo>`
3. Open the solution in Visual Studio
4. Set Trains.Api as the Startup Project by right clicking on the Trains.Api and selecting `Set As Startup Project`
5. Build the solution by choosing Build -> Rebuild All (or Build All) from the menu bar
6. Run using the Run -> Start Without Debugging from the menu bar
7. Use a tool similar to Postman to use the locally running APIs

### Assumptions
1. All times in the APIs are in UTC timezone.
2. Since POST requests are non-idempotent, if one of the values to create train schedule is wrong, the entire request is rejected.

### Future Considerations

The project is extensible to hold specific station data. It can also be extended to add many-to-many train schedule and station data.
We can also add local time handling capabilities.
We can also add PUT, PATCH and DELETE endpoints to manage train schedules. 