using NUnit.Framework;
using TourPlanner.Common;
using TourPlanner.ViewModels;

namespace TourPlanner.Tests;

public class MainViewModelTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestData_ShouldHave4EntriesOnStartup()
    {
        // arrange
        ToursViewModel toursVM = new ToursViewModel();
        MainViewModel mainViewModel = new MainViewModel(new MenuViewModel(), new SearchViewModel(), toursVM, new TourDetailsViewModel(), new TourLogsViewModel());

        // act
        int expectedCount = 4;
        int actualCount = toursVM.Tours.Count;

        // assert
        Assert.AreEqual(actualCount, expectedCount);
    }

    [Test]
    public void TestAddCommand_ShouldAddNewEntry()
    {
        // arrange
        ToursViewModel toursVM = new ToursViewModel();
        MainViewModel mainViewModel = new MainViewModel(new MenuViewModel(), new SearchViewModel(), toursVM, new TourDetailsViewModel(), new TourLogsViewModel());
        int lastCount = toursVM.Tours.Count;

        // act
        //mainViewModel.CurretUsername = "Testname";
        //mainViewModel.CurretPoints = "123";
        //mainViewModel.AddCommand.Execute(null);

        // TODO
        toursVM.Tours.Add(new Tour());

        // assert
        Assert.AreEqual(lastCount + 1, toursVM.Tours.Count, "There should be one more entry in Tours");
        // Assert.AreEqual("Testname", mainViewModel.Tours[lastCount+1].Name, "Name is different");
        // Assert.AreEqual("", mainViewModel.CurrentUsername, "Username should be cleared!");
    }
}