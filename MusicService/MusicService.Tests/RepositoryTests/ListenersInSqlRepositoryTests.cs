using MusicService.Dto;
using MusicService.Models;
using MusicService.Services;
using MusicService.Tests.Extensions;
using MusicService.Tests.TestsServices;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicService.Tests.RepositoryTests
{
    [TestFixture]
    class ListenersInSqlRepositoryTests
    {
        private Guid _songId = Guid.NewGuid();
        private Guid _albumId = Guid.NewGuid();

        private List<Song> GetSongs()
        {
            return new List<Song>()
            {
                new Song() {Title = "title1", DurationMs = 20000},
                new Song() {Title = "title2", DurationMs = 30000},
                new Song() {Title = "title3", DurationMs = 40000},
            };
        }
        
        private List<SongDto> GetSongsDto()
        {
            var collection = GetSongs();
            var collectionDto = new List<SongDto>();

            foreach(var song in collection)
            {
                collectionDto.Add(new SongDto(song));
            }

            return collectionDto;
        }


        private List<Album> GetAlbums()
        {
            return new List<Album>()
            {
                new Album() {Name = "title1" },
                new Album() {Name = "title2" },
                new Album() {Name = "title3" }
            };
        }


        private List<AlbumDto> GetAlbumsDto()
        {
            var collection = GetAlbums();
            var collectionDto = new List<AlbumDto>();

            foreach (var album in collection)
            {
                collectionDto.Add(new AlbumDto(album));
            }

            return collectionDto;
        }

        [Test]
        public async Task GetAllListenerSongs_Id_ListSongsDto()
        {
            //Arrange;
            var songs = GetSongs();
            var listener = new Listener();
            listener.Songs = songs;

            var db = TestsRepositoryService.GetClearDataBase();
            db.AddEntity(listener);

            var repository = new ListenersInSQLRepository(db);
            var expected = GetSongsDto();

            //Act
            var actualIEnumerable = await repository.GetAllListenerSongs(listener.EntityId);

            List<SongDto> actual = new List<SongDto>();

            foreach (var song in actualIEnumerable)
            {
                actual.Add(song);
            }


            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected.Count, actual.Count);
                Assert.AreEqual(expected[2].DurationMs, actual[2].DurationMs);
                Assert.AreEqual(expected[2].Title, actual[2].Title);
            });

        }


        [Test]
        public async Task GetAllListenerAlbums_Id_ListAlbumsDto()
        {
            //Arrange;
            var albums = GetAlbums();
            var listener = new Listener();
            listener.Albums = albums;

            var db = TestsRepositoryService.GetClearDataBase();
            db.AddEntity(listener);

            var repository = new ListenersInSQLRepository(db);
            var expected = GetAlbumsDto();

            //Act
            var actualIEnumerable = await repository.GetAllListenerAlbums(listener.EntityId);

            List<AlbumDto> actual = new List<AlbumDto>();

            foreach (var album in actualIEnumerable)
            {
                actual.Add(album);
            }


            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected.Count, actual.Count);
                Assert.AreEqual(expected[2].Name, actual[2].Name);
            });

        }

        [Test]
        public async Task AttachSongToListener_SongId_ResponceCode()
        {
            //Arrange;
            var song = new Song() { Title = "title", DurationMs = 20000 };
            var listener = new Listener();
            listener.Songs.Add(song);

            var db = TestsRepositoryService.GetClearDataBase();
            db.AddEntity(listener);

            var repository = new ListenersInSQLRepository(db);
            var expected = true;

            //Act
            var actual = await repository.AttachSong(song.EntityId, listener.EntityId);

            //Assert

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected, actual);
            });

        }

        [Test]
        public async Task AttachAlbumToListener_AlbumId_ResponceCode()
        {
            //Arrange;
            var album = new Album() { Name = "title"};
            var listener = new Listener();
            listener.Albums.Add(album);

            var db = TestsRepositoryService.GetClearDataBase();
            db.AddEntity(listener);

            var repository = new ListenersInSQLRepository(db);
            var expected = true;

            //Act
            var actual = await repository.AttachAlbum(album.EntityId, listener.EntityId);

            //Assert

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expected, actual);
            });

        }


    }
}
