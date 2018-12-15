using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MVC_MUSIC_STORE.Models;
//ADD a refernce to web project controler
using MVC_MUSIC_STORE.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace MVC_MUSIC_STORE.Tests.Controllers
{
    [TestClass]
    public class AlbumControllerTest
    {
        Mock<IAlbumsMock> mock;
        List<Album> albums;
        AlbumsController controller;
        List<Artist> artists;

        [TestInitialize]
        public void TestInitialize()
        {
            // arrange mock data for all unit tests
            mock = new Mock<IAlbumsMock>();

            albums = new List<Album>
            {
                new Album { AlbumId = 100, Title = "One Hundred", Price = 6.99m, Artist = new Artist {
                    ArtistId = 4000, Name = "Some One",  }
                },
                new Album { AlbumId = 200, Title = "Two Hundred", Price = 7.99m, Artist = new Artist {
                    ArtistId = 4001, Name = "Another Person" }
                },
                new Album { AlbumId = 300, Title = "Three Hundred", Price = 8.99m, Artist = new Artist {
                    ArtistId = 4002, Name = "Third Artist" }
                }
            };

            artists = new List<Artist> {
                new Artist {
                     Name = "Some thing" }
            };

            // populate interface from mock data
            mock.Setup(m => m.Albums).Returns(albums.AsQueryable());

            controller = new AlbumsController(mock.Object);
        }

        [TestMethod]
        public void IndexReturnsView()
        {
            //act
            ViewResult result = controller.Index() as ViewResult;

            // assert
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void IndexReturnsAlbums()
        {

            //act - does view result model equal a list of albums?
            var actual = (List<Album>)((ViewResult)controller.Index()).Model;

            //Assert
            CollectionAssert.AreEqual(albums.OrderBy(a => a.Artist.Name).ThenBy(a => a.Title).ToList(), actual);

        }

        [TestMethod]
        public void DetailsNotNull()
        {

            // Act
            var result = (ViewResult)controller.Details(null);

            // Assert
            Assert.AreEqual("Error", result.ViewName);
        }

        [TestMethod]
        public void DetailsInvaildID()
        {
            //Act
            var result = (ViewResult)controller.Details(123123);

            //Assert
            Assert.AreEqual("Error", result.ViewName);
        }

        [TestMethod]
        public void DetailsVaildId()
        {

            //Act
            Album actual = (Album)((ViewResult)controller.Details(100)).Model;

            //Assert
            Assert.AreEqual(albums[0], actual);
        }

        [TestMethod]
        public void DetailsViewLoads()
        {

            //Act
            ViewResult result = (ViewResult)controller.Details(100);

            //Assert
            Assert.AreEqual("Details", result.ViewName);
        }
        
        [TestMethod]
        public void CreateReturnsAView()
        {

            //Act
            ViewResult result = (ViewResult)controller.Create();
            //Assert
            Assert.AreEqual("Create", result.ViewName);
        }
        [TestMethod]
        public void CreateReturnsAArtistID()
        {

            //Act
            // var actual = (List<Artist>)((ViewResult)controller.Create()).Model;
            //Artist actual = (Artist)((ViewResult)controller.Create()).Model;
            ViewResult result = (ViewResult)controller.Create();
            //ViewResult result = controller.Create() as ViewResult;
            //var selectList = result.Value as SelectList;
            //Assert
            Assert.IsNotNull(result.ViewBag.ArtistId);

        }
        [TestMethod]
        public void CreateGenreiD() {

            //Act
            ViewResult result = (ViewResult)controller.Create();

            //Assert
            Assert.IsNotNull(result.ViewBag.GenreId);
        }
        [TestMethod]
        public void EditWithNoID() {
            // arrange
            int? id = null;

            // act 
            var result = (ViewResult)controller.Edit(id);

            // assert
            Assert.AreEqual("Error", result.ViewName);

        }
        [TestMethod]
        public void EditWithInvalidId()
        {
            // act
            var result = (ViewResult)controller.Edit(89899090);

            // assert
            Assert.AreEqual("Error", result.ViewName);
        }
        [TestMethod]
        public void EditViewBagArtist()
        {
            // act
            ViewResult actual = (ViewResult)controller.Edit(100);

            // assert
            Assert.IsNotNull(actual.ViewBag.ArtistId);
        }

        [TestMethod]
        public void EditViewBagGenre()
        {
            // act
            ViewResult actual = (ViewResult)controller.Edit(100);

            // assert
            Assert.IsNotNull(actual.ViewBag.GenreId);
        }

        // GET: Albums/Delete
        [TestMethod]
        public void DeleteNoId()
        {
            // act
            var result = (ViewResult)controller.Delete(null);

            // assert
            Assert.AreEqual("Error", result.ViewName);
        }

        [TestMethod]
        public void DeleteInvalidId()
        {
            // act
            var result = (ViewResult)controller.Delete(3739);

            // assert
            Assert.AreEqual("Error", result.ViewName);
        }

        [TestMethod]
        public void DeleteValidIdLoadsView()
        {
            // act
            var result = (ViewResult)controller.Delete(100);

            // assert
            Assert.AreEqual("Delete", result.ViewName);
        }

        [TestMethod]
        public void DeleteValidIdLoadsAlbum()
        {
            // act
            Album result = (Album)((ViewResult)controller.Delete(100)).Model;

            // assert
            Assert.AreEqual(albums[0], result);
        }

        // POST: Albums/Edit
        [TestMethod]
        public void EditPostLoadsIndex()
        {
            // act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Edit(albums[0]);

            // assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void EditPostViewBagArtist()
        {
            // arrange
            Album invalid = new Album { AlbumId = 27 };
            controller.ModelState.AddModelError("Error", "Won't Save");

            // act
            ViewResult result = (ViewResult)controller.Edit(invalid);

            // assert
            Assert.IsNotNull(result.ViewBag.ArtistId);
        }

        [TestMethod]
        public void EditPostViewBagGenre()
        {
            // arrange
            Album invalid = new Album { AlbumId = 27 };
            controller.ModelState.AddModelError("Error", "Won't Save");

            // act
            ViewResult result = (ViewResult)controller.Edit(invalid);

            // assert
            Assert.IsNotNull(result.ViewBag.GenreId);
        }

        [TestMethod]
        public void EditPostInvalidLoadsView()
        {
            // arrange
            Album invalid = new Album { AlbumId = 27 };
            controller.ModelState.AddModelError("Error", "Won't Save");

            // act
            ViewResult result = (ViewResult)controller.Edit(invalid);

            // assert
            Assert.AreEqual("Edit", result.ViewName);
        }

        [TestMethod]
        public void EditPostInvalidLoadsAlbum()
        {
            // arrange
            Album invalid = new Album { AlbumId = 100 };
            controller.ModelState.AddModelError("Error", "cannot Save");

            // act
            Album result = (Album)((ViewResult)controller.Edit(invalid)).Model;

            // assert
            Assert.AreEqual(invalid, result);
        }

        // POST: Albums/Create
        [TestMethod]
        public void CreateValidAlbum()
        {
            // arrange
            Album newAlbum = new Album
            {
                AlbumId = 400,
                Title = "Four Hundred",
                Price = 9.99m,
                Artist = new Artist
                {
                    ArtistId = 4004,
                    Name = "Some Four"
                }
            };

            // act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Create(newAlbum);

            // assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void CreateInvalidAlbum()
        {
            // arrange
            Album invalid = new Album();

            // act
            controller.ModelState.AddModelError("Cannot create", "create exception");
            ViewResult result = (ViewResult)controller.Create(invalid);

            // assert
            Assert.AreEqual("Create", result.ViewName);
        }

        [TestMethod]
        public void CreateInvalidViewBagArtist()
        {
            // arrange
            Album invalid = new Album();

            // act
            controller.ModelState.AddModelError("Cannot create", "create exception");
            ViewResult result = (ViewResult)controller.Create(invalid);

            // assert
            Assert.IsNotNull(result.ViewBag.ArtistId);
        }

        [TestMethod]
        public void CreateInvalidViewBagGenre()
        {
            // arrange
            Album invalid = new Album();

            // act
            controller.ModelState.AddModelError("Cannot create", "create exception");
            ViewResult result = (ViewResult)controller.Create(invalid);

            // assert
            Assert.IsNotNull(result.ViewBag.GenreId);
        }
        // POST: Albums/DeleteConfirmed/100
        [TestMethod]
        public void DeleteConfirmedNoId()
        {
            // act
            ViewResult result = (ViewResult)controller.DeleteConfirmed(null);

            // assert
            Assert.AreEqual("Error", result.ViewName);
        }

        [TestMethod]
        public void DeleteConfirmedInvalidId()
        {
            // act
            ViewResult result = (ViewResult)controller.DeleteConfirmed(3972);

            // assert
            Assert.AreEqual("Error", result.ViewName);
        }

        [TestMethod]
        public void DeleteConfirmedValidId()
        {
            // act
            RedirectToRouteResult result = (RedirectToRouteResult)controller.DeleteConfirmed(100);

            // assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
       

    }
}
