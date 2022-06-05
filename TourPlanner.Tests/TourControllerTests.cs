using MaterialDesignThemes.Wpf;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TourPlanner.BL;
using TourPlanner.Common.DTO;
using TourPlanner.DAL.Repositories;
using TourPlanner.ViewModels;

namespace TourPlanner.Tests;

public class TourControllerTests
{
    private ITourController tourController;

    private Mock<ITourRepository> tourRepositoryMock;
    private Mock<IRepository<TourLogDto>> tourLogRepositoryMock;

    private List<TourDto> toursCollection = new List<TourDto>();

    private TourDto tour1 = new TourDto()
    {
        Id = Guid.NewGuid(),
        Name = "FH-Ausflug zur Donauinsel",
        Description = "Offizieller Ausflug zur Donausinsel",
        From = "Höchstädtplatz 6, 1200 Wien",
        To = "Parkplatz Donauinsel, Floridsdorfer Brücke, 1210 Wien"
    };

    [SetUp]
    public void Setup()
    {
        toursCollection.Clear();

        tourRepositoryMock = new Mock<ITourRepository>();
        tourLogRepositoryMock = new Mock<IRepository<TourLogDto>>();

        tourController = new TourController(tourRepositoryMock.Object, tourLogRepositoryMock.Object);

        tourRepositoryMock
            .Setup(c => c.Insert(It.IsAny<TourDto>()))
            .Callback((TourDto tour) => toursCollection.Add(tour))
            .Returns(true);

        tourRepositoryMock
            .Setup(c => c.Get())
            .Returns(() => toursCollection);

        tourRepositoryMock
            .Setup(c => c.GetById(It.IsAny<Guid>()))
            .Returns((Guid id) => toursCollection.Where(t => t.Id == id).SingleOrDefault());

        tourRepositoryMock
            .Setup(c => c.Delete(It.IsAny<Guid>()))
            .Callback((Guid id) => toursCollection.RemoveAll(t => t.Id == id))
            .Returns(true);
    }

    [Test]
    public void Test_WhenAddingATourControllerCallsInsertOfTourRepository()
    {
        // act
        tourController.AddItem(tour1);

        // assert
        tourRepositoryMock.Verify(mock => mock.Insert(tour1), Times.Once());
    }

    [Test]
    public void Test_WhenRemovingATourControllerCallsDeleteOfTourRepository()
    {
        // act
        tourController.RemoveItem(tour1);

        // assert
        tourRepositoryMock.Verify(mock => mock.Delete(tour1.Id), Times.Once());
    }

    [Test]
    public void Test_WhenGettingATourControllerCallsGetByIdOfTourRepository()
    {
        // act
        tourController.GetById(tour1.Id);

        // assert
        tourRepositoryMock.Verify(mock => mock.GetById(tour1.Id), Times.Once());
    }

    [Test]
    public void Test_AddingTourAndGetItBackFromRepository()
    {
        // act
        tourController.AddItem(tour1);
        var tour = tourController.GetById(tour1.Id);

        // assert
        Assert.AreEqual(tour1.Id, tour.Id);
    }

    [Test]
    public void Test_WhenGettingTourLogsOfATourControllerCallsGetOfTourLogRepository()
    {
        // act
        tourController.GetLogsOfTour(tour1.Id);

        // assert
        tourLogRepositoryMock.Verify(mock => mock.Get(), Times.Once());
    }

    [Test]
    public void Test_WhenGettingToursWithoutFilterControllerCallsGetOfTourRepository()
    {
        // act
        tourController.GetItems(null);

        // assert
        tourRepositoryMock.Verify(mock => mock.Get(), Times.Once());
    }

    [Test]
    public void Test_WhenGettingToursWithFilterControllerCallsGetOfTourRepository()
    {
        // act
        tourController.GetItems("filter");

        // assert
        tourRepositoryMock.Verify(mock => mock.Get(), Times.Once());
    }

    [Test]
    public void Test_AddingTourAndGetAll()
    {
        // act
        tourController.AddItem(tour1);

        // assert
        Assert.AreEqual(tourController.GetItems(null).Count(), 1);
    }
}