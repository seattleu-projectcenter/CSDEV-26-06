using System;
using System.IO;
using System.Linq;
using System.Text.Json;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

using ContosoCrafts.WebSite.Pages;

namespace UnitTests.Pages.Events
{
    public class EventsPageTests
    {
        private ILogger<EventsModel> _logger;

        [SetUp]
        public void Setup()
        {
            _logger = Mock.Of<ILogger<EventsModel>>();
        }

        [Test]
        public void OnGet_NoExistingFile_ShouldCreateEmptyEventsFile()
        {
            using var root = TempWebRoot.Create();
            var model = CreateModel(root.Path);

            model.OnGet();

            Assert.AreEqual(0, model.Events.Count);
            Assert.IsTrue(File.Exists(root.EventsPath));
            Assert.AreEqual("[]", File.ReadAllText(root.EventsPath));
        }

        [Test]
        public void OnGet_WhenEventsExist_ShouldOrderByStartTime()
        {
            using var root = TempWebRoot.Create();
            WriteEvents(root, new EventsModel.EventDto
            {
                Id = "later",
                Matchup = "Later Fight",
                StartUtc = DateTimeOffset.UtcNow.AddDays(1)
            },
            new EventsModel.EventDto
            {
                Id = "earlier",
                Matchup = "Earlier Fight",
                StartUtc = DateTimeOffset.UtcNow.AddDays(-1)
            });

            var model = CreateModel(root.Path);
            model.OnGet();

            Assert.AreEqual(2, model.Events.Count);
            Assert.AreEqual("earlier", model.Events[0].Id);
        }

        [Test]
        public void OnGet_BlankFile_ShouldResetToEmptyArray()
        {
            using var root = TempWebRoot.Create();
            Directory.CreateDirectory(root.DataPath);
            File.WriteAllText(root.EventsPath, "    ");

            var model = CreateModel(root.Path);
            model.OnGet();

            Assert.AreEqual(0, model.Events.Count);
            Assert.AreEqual("[]", File.ReadAllText(root.EventsPath));
        }

        [Test]
        public void OnGet_NullJson_ShouldReturnEmptyList()
        {
            using var root = TempWebRoot.Create();
            Directory.CreateDirectory(root.DataPath);
            File.WriteAllText(root.EventsPath, "null");

            var model = CreateModel(root.Path);
            model.OnGet();

            Assert.AreEqual(0, model.Events.Count);
        }

        [Test]
        public void OnGet_InvalidJson_ShouldRecoverGracefully()
        {
            using var root = TempWebRoot.Create();
            Directory.CreateDirectory(root.DataPath);
            File.WriteAllText(root.EventsPath, "{not-valid-json");

            var model = CreateModel(root.Path);
            model.OnGet();

            Assert.AreEqual(0, model.Events.Count);
            Assert.AreEqual("[]", File.ReadAllText(root.EventsPath));
        }

        [Test]
        public void OnGet_WhenEnvironmentThrows_ShouldReturnEmptyList()
        {
            var env = new Mock<IWebHostEnvironment>();
            env.Setup(e => e.WebRootPath).Throws(new InvalidOperationException("boom"));

            var model = new EventsModel(_logger, env.Object);
            model.OnGet();

            Assert.AreEqual(0, model.Events.Count);
        }

        [Test]
        public void OnPostAdd_ShouldAppendEvent()
        {
            using var root = TempWebRoot.Create();
            var model = CreateModel(root.Path);
            var start = new DateTimeOffset(2025, 1, 2, 3, 4, 0, TimeSpan.Zero);

            var result = model.OnPostAdd("Volk vs Topuria", start, "Main", "T-Mobile", "Las Vegas");

            Assert.IsInstanceOf<RedirectToPageResult>(result);
            var saved = ReadEvents(root);
            Assert.AreEqual(1, saved.Length);
            Assert.AreEqual("Volk vs Topuria", saved[0].Matchup);
            Assert.AreEqual(start, saved[0].StartUtc);
        }

        [Test]
        public void OnPostAdd_WhenSaveFails_ShouldStillReturnRedirect()
        {
            using var root = TempWebRoot.Create();
            Directory.CreateDirectory(root.DataPath);
            Directory.CreateDirectory(root.EventsPath); // Force Save to fail when moving temp file
            var model = CreateModel(root.Path);

            var result = model.OnPostAdd("Fail Fight", DateTimeOffset.UtcNow, null, null, null);

            Assert.IsInstanceOf<RedirectToPageResult>(result);
        }

        [Test]
        public void OnPostAdd_WhenExceptionThrown_ShouldReturnRedirect()
        {
            var env = new Mock<IWebHostEnvironment>();
            env.Setup(e => e.WebRootPath).Throws(new InvalidOperationException("boom"));
            var model = new EventsModel(_logger, env.Object);

            var result = model.OnPostAdd("Any", DateTimeOffset.UtcNow, null, null, null);

            Assert.IsInstanceOf<RedirectToPageResult>(result);
        }

        [Test]
        public void OnPostUpdate_ShouldModifyExistingEvent()
        {
            using var root = TempWebRoot.Create();
            var existing = new EventsModel.EventDto
            {
                Id = "update-me",
                Matchup = "Old",
                StartUtc = DateTimeOffset.UtcNow,
                Card = "Old Card",
                Venue = "Old Venue",
                Location = "Old Location"
            };
            WriteEvents(root, existing);

            var model = CreateModel(root.Path);
            var newStart = existing.StartUtc.AddDays(1);

            var result = model.OnPostUpdate("update-me", "New", newStart, "New Card", "New Venue", "New Location");

            Assert.IsInstanceOf<RedirectToPageResult>(result);
            var saved = ReadEvents(root);
            Assert.AreEqual("New", saved.Single().Matchup);
            Assert.AreEqual("New Card", saved.Single().Card);
            Assert.AreEqual(newStart, saved.Single().StartUtc);
        }

        [Test]
        public void OnPostUpdate_MissingId_ShouldShortCircuit()
        {
            using var root = TempWebRoot.Create();
            var model = CreateModel(root.Path);

            var result = model.OnPostUpdate(string.Empty, "Ignored", DateTimeOffset.UtcNow, null, null, null);

            Assert.IsInstanceOf<RedirectToPageResult>(result);
            Assert.IsFalse(File.Exists(root.EventsPath));
        }

        [Test]
        public void OnPostUpdate_IdNotFound_ShouldLeaveDataIntact()
        {
            using var root = TempWebRoot.Create();
            WriteEvents(root, new EventsModel.EventDto { Id = "existing", Matchup = "Stay" });
            var model = CreateModel(root.Path);

            model.OnPostUpdate("missing", "New", DateTimeOffset.UtcNow, null, null, null);

            var saved = ReadEvents(root);
            Assert.AreEqual("Stay", saved.Single().Matchup);
        }

        [Test]
        public void OnPostUpdate_WhenExceptionThrown_ShouldReturnRedirect()
        {
            var env = new Mock<IWebHostEnvironment>();
            env.Setup(e => e.WebRootPath).Throws(new InvalidOperationException("boom"));
            var model = new EventsModel(_logger, env.Object);

            var result = model.OnPostUpdate("any", "Any", DateTimeOffset.UtcNow, null, null, null);

            Assert.IsInstanceOf<RedirectToPageResult>(result);
        }

        [Test]
        public void OnPostDelete_ShouldRemoveEvent()
        {
            using var root = TempWebRoot.Create();
            WriteEvents(root,
                new EventsModel.EventDto { Id = "keep", Matchup = "Keep" },
                new EventsModel.EventDto { Id = "delete", Matchup = "Delete" });
            var model = CreateModel(root.Path);

            var result = model.OnPostDelete("delete");

            Assert.IsInstanceOf<RedirectToPageResult>(result);
            var saved = ReadEvents(root);
            Assert.AreEqual(1, saved.Length);
            Assert.AreEqual("keep", saved.Single().Id);
        }

        [Test]
        public void OnPostDelete_MissingId_ShouldShortCircuit()
        {
            using var root = TempWebRoot.Create();
            var model = CreateModel(root.Path);

            var result = model.OnPostDelete(null);

            Assert.IsInstanceOf<RedirectToPageResult>(result);
            Assert.IsFalse(File.Exists(root.EventsPath));
        }

        [Test]
        public void OnPostDelete_IdNotFound_ShouldLeaveDataIntact()
        {
            using var root = TempWebRoot.Create();
            WriteEvents(root, new EventsModel.EventDto { Id = "existing", Matchup = "Stay" });
            var model = CreateModel(root.Path);

            model.OnPostDelete("missing");

            var saved = ReadEvents(root);
            Assert.AreEqual(1, saved.Length);
        }

        [Test]
        public void OnPostDelete_WhenExceptionThrown_ShouldReturnRedirect()
        {
            var env = new Mock<IWebHostEnvironment>();
            env.Setup(e => e.WebRootPath).Throws(new InvalidOperationException("boom"));
            var model = new EventsModel(_logger, env.Object);

            var result = model.OnPostDelete("any");

            Assert.IsInstanceOf<RedirectToPageResult>(result);
        }

        [Test]
        public void EventDto_LocalStartDisplay_ShouldFormatInLocalTime()
        {
            var dto = new EventsModel.EventDto
            {
                StartUtc = new DateTimeOffset(2024, 1, 1, 12, 0, 0, TimeSpan.Zero)
            };

            var display = dto.LocalStartDisplay;

            Assert.IsFalse(string.IsNullOrWhiteSpace(display));
            StringAssert.Contains("•", display);
        }

        private EventsModel CreateModel(string webRootPath)
        {
            var env = new Mock<IWebHostEnvironment>();
            env.Setup(e => e.WebRootPath).Returns(webRootPath);
            env.SetupProperty(e => e.EnvironmentName, "UnitTests");

            return new EventsModel(_logger, env.Object);
        }

        private static EventsModel.EventDto[] ReadEvents(TempWebRoot root)
        {
            var json = File.ReadAllText(root.EventsPath);
            return JsonSerializer.Deserialize<EventsModel.EventDto[]>(json,
                       new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? Array.Empty<EventsModel.EventDto>();
        }

        private static void WriteEvents(TempWebRoot root, params EventsModel.EventDto[] items)
        {
            Directory.CreateDirectory(root.DataPath);
            var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(root.EventsPath, json);
        }

        private sealed class TempWebRoot : IDisposable
        {
            private TempWebRoot(string path)
            {
                Path = path;
                DataPath = System.IO.Path.Combine(path, "data");
                EventsPath = System.IO.Path.Combine(DataPath, "events.json");
            }

            public string Path { get; }
            public string DataPath { get; }
            public string EventsPath { get; }

            public static TempWebRoot Create()
            {
                var baseDir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "EventsPageTests", Guid.NewGuid().ToString("N"));
                if (Directory.Exists(baseDir))
                {
                    Directory.Delete(baseDir, true);
                }

                Directory.CreateDirectory(baseDir);
                return new TempWebRoot(baseDir);
            }

            public void Dispose()
            {
                if (Directory.Exists(Path))
                {
                    Directory.Delete(Path, true);
                }
            }
        }
    }
}
