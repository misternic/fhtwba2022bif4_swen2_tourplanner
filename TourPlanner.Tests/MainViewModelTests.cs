using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TourPlanner.BL;
using TourPlanner.Common.DTO;
using TourPlanner.ViewModels;

namespace TourPlanner.Tests;

public class MainViewModelTests
{
    private MenuViewModel menuViewModel;
    private SearchViewModel searchViewModel;
    private ToursViewModel toursViewModel;
    private TourDetailsViewModel tourDetailsViewModel;
    private TourLogsViewModel tourLogsViewModel;

    private Mock<ITourController> tourControllerMock;
    private Mock<ITourLogController> tourLogControllerMock;

    private TourDto tour1 = new TourDto()
    {
        Id = Guid.NewGuid(),
        Name = "New Tour",
        Description = "",
        From = "",
        To = ""
    };

    [SetUp]
    public void Setup()
    {
        menuViewModel = new MenuViewModel();
        searchViewModel = new SearchViewModel();
        toursViewModel = new ToursViewModel();
        tourDetailsViewModel = new TourDetailsViewModel();
        tourLogsViewModel = new TourLogsViewModel();

        tourControllerMock = new Mock<ITourController>();
        tourLogControllerMock = new Mock<ITourLogController>();
    }

    [Test]
    public void TestMainViewModel_ShouldHaveZeroEntriesOnStartup()
    {
        // arrange
        var mainViewModel = new MainViewModel(menuViewModel, searchViewModel, toursViewModel, tourDetailsViewModel, tourLogsViewModel, tourControllerMock.Object, tourLogControllerMock.Object);
        int expectedCount = 0;

        // act
        int actualCount = toursViewModel.Tours.Count;

        // assert
        Assert.AreEqual(actualCount, expectedCount);
    }

    [Test]
    public void TestMainViewModel_ConstructorOfMainViewModelShouldLoadAllToursFromTourController()
    {
        // arrange & act
        var mainViewModel = new MainViewModel(menuViewModel, searchViewModel, toursViewModel, tourDetailsViewModel, tourLogsViewModel, tourControllerMock.Object, tourLogControllerMock.Object);

        // assert
        tourControllerMock.Verify(mock => mock.GetItems(It.IsAny<string>()), Times.Once());
    }

    [Test]
    public void TestTestMainViewModel_CountOfToursShouldBeOneHigherAfterAddingOne()
    {
        // arrange
        var mainViewModel = new MainViewModel(menuViewModel, searchViewModel, toursViewModel, tourDetailsViewModel, tourLogsViewModel, tourControllerMock.Object, tourLogControllerMock.Object);
        tourControllerMock.Setup(c => c.GetItems(It.IsAny<string>())).Returns(() => new List<TourDto> { tour1 });
        int lastCount = toursViewModel.Tours.Count;

        // act
        toursViewModel.AddCommand.Execute(null);
        int actualCount = toursViewModel.Tours.Count;

        // assert
        Assert.AreEqual(actualCount, lastCount+1);
    }

    [Test]
    public void TestMainViewModel_TourControllerShouldCalledAddItemAfterAddingOne()
    {
        // arrange
        var mainViewModel = new MainViewModel(menuViewModel, searchViewModel, toursViewModel, tourDetailsViewModel, tourLogsViewModel, tourControllerMock.Object, tourLogControllerMock.Object);

        // act
        int lastCount = toursViewModel.Tours.Count;
        toursViewModel.AddCommand.Execute(null);

        // assert
        tourControllerMock.Verify(mock => mock.AddItem(It.IsAny<TourDto>()), Times.Once(), "There should be one more entry as before in Tours");
    }
}