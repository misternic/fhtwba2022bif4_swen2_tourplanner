using MaterialDesignThemes.Wpf;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TourPlanner.BL;
using TourPlanner.Common.DTO;
using TourPlanner.ViewModels;

namespace TourPlanner.Tests;

public class ViewModelsTests
{
    private MenuViewModel menuViewModel;
    private SearchViewModel searchViewModel;
    private ToursViewModel toursViewModel;
    private TourDetailsViewModel tourDetailsViewModel;
    private TourLogsViewModel tourLogsViewModel;

    private Mock<ITourController> tourControllerMock;
    private Mock<ITourLogController> tourLogControllerMock;

    private MainViewModel mainViewModel;

    private List<TourDto> toursCollection = new List<TourDto>();

    private TourDto tour1 = new TourDto()
    {
        Id = Guid.NewGuid(),
        Name = "FH-Ausflug zur Donauinsel",
        Description = "Offizieller Ausflug zur Donausinsel",
        From = "Höchstädtplatz 6, 1200 Wien",
        To = "Parkplatz Donauinsel, Floridsdorfer Brücke, 1210 Wien"
    };

    private TourDto tour2 = new TourDto()
    {
        Id = Guid.NewGuid(),
        Name = "Von Heiligenstadt bis nach Klosterneuburg",
        Description = "Ab nach Niederösterreich!",
        From = "Heiligenstädter Straße 131, 1190 Wien",
        To = "Weidlinger Straße 2, 3400 Klosterneuburg"
    };



    [SetUp]
    public void Setup()
    {
        toursCollection.Clear();

        menuViewModel = new MenuViewModel();
        searchViewModel = new SearchViewModel();
        toursViewModel = new ToursViewModel();
        tourDetailsViewModel = new TourDetailsViewModel();
        tourLogsViewModel = new TourLogsViewModel();

        tourControllerMock = new Mock<ITourController>();
        tourLogControllerMock = new Mock<ITourLogController>();

        mainViewModel = new MainViewModel(menuViewModel, searchViewModel, toursViewModel, tourDetailsViewModel, tourLogsViewModel, tourControllerMock.Object, tourLogControllerMock.Object, (new Mock<ISnackbarMessageQueue>()).Object);

        tourControllerMock
            .Setup(c => c.AddItem(It.IsAny<TourDto>()))
            .Callback((TourDto tour) => toursCollection.Add(tour))
            .Returns(true);

        tourControllerMock
            .Setup(c => c.GetItems(It.IsAny<string>()))
            .Returns(() => toursCollection);

        tourControllerMock
            .Setup(c => c.GetById(It.IsAny<Guid>()))
            .Returns((Guid id) => toursCollection.Where(t => t.Id == id).Single());

        tourControllerMock
            .Setup(c => c.RemoveItem(It.IsAny<TourDto>()))
            .Callback((TourDto tour) => toursCollection.RemoveAll(t => t.Id == tour.Id))
            .Returns(true);
    }

    [Test]
    public void Test_ToursOverviewShouldBeEmptyOnStartup()
    {
        // arrange
        int expected = 0;

        // act
        int actual = toursViewModel.Tours.Count;

        // assert
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Test_CalledGetItemsFromTourController()
    {
        // arrange: MainViewModel already initialized in SetUp

        // act: MainViewModel Constructor

        // assert
        tourControllerMock.Verify(mock => mock.GetItems(It.IsAny<string>()), Times.Once());
    }

    [Test]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    [TestCase(5)]
    public void Test_AddTourUsingToursItemView(int round)
    {
        // arrange
        int before = toursViewModel.Tours.Count;

        // act
        for(int i = 0; i < round; i++)
            toursViewModel.AddCommand.Execute(null);

        // assert
        Assert.AreEqual(before + round, toursViewModel.Tours.Count);
    }

    [Test]
    public void Test_CalledAddItemFromTourController()
    {
        // arrange: MainViewModel already initialized in SetUp

        // act
        toursViewModel.AddCommand.Execute(null);

        // assert
        tourControllerMock.Verify(mock => mock.AddItem(It.IsAny<TourDto>()), Times.Once());
    }

    [Test]
    public void Test_AddTourUsingTheMenu()
    {
        // arrange
        var before = toursViewModel.Tours.Count;

        // act
        menuViewModel.AddCommand.Execute(null);

        // assert
        Assert.AreEqual(before + 1, toursViewModel.Tours.Count);
    }

    [Test]
    public void Test_SelectedTourShowsDetails()
    {
        // arrange
        toursViewModel.AddCommand.Execute(null);

        // act
        toursViewModel.SelectTour(toursViewModel.Tours.First().Id);

        // assert
        Assert.AreEqual(toursViewModel.Tours.First().Id, tourDetailsViewModel.Tour.Id);
    }

    [Test]
    public void Test_AddedTourShouldBeTheSelectedOne()
    {
        // arrange: MainViewModel already initialized in SetUp

        // act
        toursViewModel.AddCommand.Execute(null);

        // assert
        Assert.AreEqual(toursViewModel.Tours.First(), toursViewModel.SelectedTour);
    }

    [Test]
    public void Test_EditedNameShouldBeReloadedInToursItemView()
    {
        // arrange
        toursViewModel.AddCommand.Execute(null);
        var newName = "Super cool name";

        // act
        tourDetailsViewModel.Tour.Name = newName;
        tourDetailsViewModel.SaveCommand.Execute(null);

        // assert
        Assert.AreEqual(newName, toursViewModel.Tours[0].Name);
    }

    [Test]
    public void Test_UpdateItemShouldBeCalledAfterSaving()
    {
        // arrange
        toursViewModel.AddCommand.Execute(null);
        var newName = "Super cool name";

        // act
        tourDetailsViewModel.Tour.Name = newName;
        tourDetailsViewModel.SaveCommand.Execute(null);

        // assert
        tourControllerMock.Verify(mock => mock.UpdateItem(tourDetailsViewModel.Tour), Times.Once());
    }

    [Test]
    public void Test_SaveTourUsingTheMenu()
    {
        // arrange
        toursViewModel.AddCommand.Execute(null);
        var newName = "Super cool name";

        // act
        tourDetailsViewModel.Tour.Name = newName;
        menuViewModel.SaveCommand.Execute(null);

        // assert
        tourControllerMock.Verify(mock => mock.UpdateItem(tourDetailsViewModel.Tour), Times.Once());
    }

    [Test]
    public void Test_CalledGetItemsWithSearchTextAfterSettingFilter()
    {
        // arrange: MainViewModel already initialized in SetUp
        string searchText = "offiziell";

        // act
        searchViewModel.SearchText = searchText;
        searchViewModel.SearchCommand.Execute(null);

        // assert
        tourControllerMock.Verify(mock => mock.GetItems(searchText), Times.Once());
    }

    [Test]
    public void Test_DeleteTourUsingToursOverview()
    {
        // arrange
        toursViewModel.AddCommand.Execute(null);
        var before = toursViewModel.Tours.Count;

        // act
        toursViewModel.RemoveCommand.Execute(null);

        // assert
        Assert.AreEqual(before, toursViewModel.Tours.Count);
    }

    [Test]
    public void Test_DeleteTourUsingMenu()
    {
        // arrange
        toursViewModel.AddCommand.Execute(null);
        var before = toursViewModel.Tours.Count;

        // act
        menuViewModel.DeleteCommand.Execute(null);

        // assert
        Assert.AreEqual(before, toursViewModel.Tours.Count);
    }

    [Test]
    public void Test_DeleteTourUsingDetails()
    {
        // arrange
        toursViewModel.AddCommand.Execute(null);
        var before = toursViewModel.Tours.Count;

        // act
        tourDetailsViewModel.DeleteCommand.Execute(null);

        // assert
        Assert.AreEqual(before, toursViewModel.Tours.Count);
    }

    [Test]
    public void Test_TourLogsGridShouldBeEmptyAfterAddingNewTour()
    {
        // arrange
        toursViewModel.AddCommand.Execute(null);
        int expected = 0;

        // act
        int actual = tourLogsViewModel.TourLogs.Count;

        // assert
        Assert.AreEqual(expected, actual);
    }    
}