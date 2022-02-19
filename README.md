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
- The trains API allows to create and manage train schedules.
- The services layer contains the core business logic and interacts with the database.
- The tests consists of unit tests for API, services and the database.

### Installation

1. Install [`Visual Studio`](https://visualstudio.microsoft.com/downloads/)
2. Clone git repo locally using `$ git clone https://github.com/techpearls/TrainScheduleManagementSystem.git`
3. Open the solution in Visual Studio
4. Set Trains.Api as the Startup Project by right clicking on the Trains.Api and selecting `Set As Startup Project`
5. Build the solution by choosing Build -> Rebuild All (or Build All) from the menu bar
6. Run using the Run -> Start Without Debugging from the menu bar
7. Use a tool like Curl or Postman to send requests to the API at endpoint:http://localhost:5000/api/v1/

### Assumptions
1. All times in the APIs are in UTC timezone.
2. Since POST requests are non-idempotent, if one of the values to create train schedule is invalid ([0-2359]) the entire request is rejected.

### Usage
1. Creating a new schedule
<img width="863" alt="Screen Shot 2022-02-18 at 8 55 51 PM" src="https://user-images.githubusercontent.com/11620079/154786892-783011b6-03e5-4ebd-bf38-cdfd190f736e.png">

2. Getting a train schedule
<img width="850" alt="Screen Shot 2022-02-18 at 9 49 35 PM" src="https://user-images.githubusercontent.com/11620079/154788306-e8b8f0c5-891b-4a76-ab99-e71079ea9176.png">

3. Getting all schedules
<img width="873" alt="Screen Shot 2022-02-18 at 10 01 31 PM" src="https://user-images.githubusercontent.com/11620079/154788595-f249cb0f-8cd8-4f54-b5fc-d28440f61d4c.png">

4. Get next time when 2 or more trains are at the station
<img width="771" alt="Screen Shot 2022-02-18 at 10 16 02 PM" src="https://user-images.githubusercontent.com/11620079/154789022-2135c04b-95f9-4c8f-a380-d479eed7fab3.png">


### Future Considerations

- The project is extensible to hold specific station data. It can also be extended to add many-to-many train schedule and station data.
- We can also add local time handling capabilities.
- We can also add PUT, PATCH and DELETE endpoints to manage train schedules. 
- We can additionally add authentication and authorization capabilities to the APIs.
