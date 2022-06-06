# FHTWBA2022BIF4_SWEN2_TourPlanner

## App architecture
Our tour planner solution consists of the following projects which are responsible for a smaller part of the overall project:

- TourPlanner
  - Contains all the WPF specific code.
  - Contains all the views (.xaml) and interaction logic (.xaml.cs)
  - Contains the ViewModels and command logic

- TourPlanner.BL
  - Contains the business logic of the solution
  - Contains the controllers for tours and tour logs

- TourPlanner.Common
  - Contains shared resources for the other projects
  - Was used during implementation with multiple entry points

- TourPlanner.DAL
  - Contains all the code to communicate with MapQuest
  - Contains all the code to communicate with WeatherApi
  - Contains the repositories to communicate with the database
  - Contains the database context

- TourPlanner.Tests
  - Contains integration tests for the controllers
  - Contains integration tests for the view models

## Use Cases
The tour planner is a tool to track the progress progress of tours of any kind. The user is able to track tours by foot, bicycle or car. Once a tour is created, the user can add as many logs as he pleases. These logs get automatically filled with weather data. The user is also able to export tours and summary reports in PDF format.

## UX / UI library decision
Due to the "outdated" look and feel of WPF we opted to use a UI theme called "Material Design", which is based on the user interface guidelines from Google. Although it took a lot of time to get the theme to work, we are satisfied with the result. The application now looks a lot more modern and clean.

## Implemented design patterns
The tour planner implements various different design patterns. The WPF code is structured using a MVVM pattern, where each view corresponds to a view model. Furthermore we are using command / observer patterns to synchronize data between view models. The tour planner also uses factory and builder patterns throughout the different app architecture layers.

## Leassons learned
Since we both have not used WPF before, the experience of working with the framework was challenging at first. After we played around with different layouts, etc. we became more familiar with the concepts of the framework. Although the development process was not bad, we likely will not use WPF again if it's not necessary.

We also ran into a few small issues because of our IDE setup. One team member used Microsoft's Visual Studio whereas the other one used JetBrains' Rider. While both IDEs are outstanding at their job, we would not recommend the combination of them both. We ran into several smaller issues regarding file encoding etc. There were no major problems but even the smaller ones are avoidable by using the same IDE amongst the team.

## Unit testing decisions
Since the tour planner does contain very little business logic we have decided against unit tests and opted for integration tests. To test the controller(s) we have mocked our tour- and tour-log-repositories and check if the correct methods are called with the expected parameters. We have also implemented tests for our view models. For the view model tests we mocked the underlying controller and test functionallity using the mock.

## Unique feature
As a unique feature, our tour planner contains weather data. If you submit a tour log within 7 days of completing it, the log automatically includes temperature data of the destination. The data is fetched from the [www.weatherapi.com](https://www.weatherapi.com/) API. Due to the pricing model of said API, we can only retrieve weather data for the last 7 days. Therefore older logs do not contain weather data.

## Tracked time
During development we split different tasks amongst the team and tracked the time spent using Toggle.

In total the development of the tour planner adds up to **74 hours**.

## Git link
https://github.com/misternic/fhtwba2022bif4_swen2_tourplanner
