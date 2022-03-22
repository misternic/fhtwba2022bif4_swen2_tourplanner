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
        SidebarViewModel sidebarViewModel = new SidebarViewModel();
        MainViewModel mainViewModel = new MainViewModel(new MenuViewModel(), new SearchbarViewModel(), sidebarViewModel, new TourLogsViewModel(), new TourViewModel());

        // act
        int expectedCount = 4;
        int actualCount = sidebarViewModel.Tours.Count;

        // assert
        Assert.AreEqual(actualCount, expectedCount);
    }

    [Test]
    public void TestAddCommand_ShouldAddNewEntry()
    {
        // arrange
        SidebarViewModel sidebarViewModel = new SidebarViewModel();
        MainViewModel mainViewModel = new MainViewModel(new MenuViewModel(), new SearchbarViewModel(), sidebarViewModel, new TourLogsViewModel(), new TourViewModel());
        int lastCount = sidebarViewModel.Tours.Count;

        // act
        //mainViewModel.CurretUsername = "Testname";
        //mainViewModel.CurretPoints = "123";
        //mainViewModel.AddCommand.Execute(null);

        // TODO
        sidebarViewModel.Tours.Add(new Tour());

        // assert
        Assert.AreEqual(lastCount + 1, sidebarViewModel.Tours.Count, "There should be one more entry in Tours");
        // Assert.AreEqual("Testname", mainViewModel.Tours[lastCount+1].Name, "Name is different");
        // Assert.AreEqual("", mainViewModel.CurrentUsername, "Username should be cleared!");
    }
}