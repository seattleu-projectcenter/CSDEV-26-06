using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ContosoCrafts.WebSite.Pages
{
    /// <summary>
    /// Events page simple in-page model 
    /// </summary>
    public class EventsModel : PageModel
    {
        private readonly ILogger<EventsModel> _logger;
        private readonly IWebHostEnvironment _env;

        public EventsModel(ILogger<EventsModel> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public class EventDto
        {
            public string Id { get; set; } = Guid.NewGuid().ToString();
            public string Matchup { get; set; } = "";
            public DateTimeOffset StartUtc { get; set; } = DateTimeOffset.UtcNow;
            public string Card { get; set; } = "";
            public string Venue { get; set; } = "";
            public string Location { get; set; } = "";
            public string LocalStartDisplay => StartUtc.ToLocalTime().ToString("ddd, MMM d • h:mm tt");
        }

        public IList<EventDto> Events { get; private set; } = new List<EventDto>();

        private string JsonPath =>
            Path.Combine(_env.WebRootPath, "data", "events.json");

        private IList<EventDto> Load()
        {
            var dir = Path.GetDirectoryName(JsonPath)!;
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            if (!System.IO.File.Exists(JsonPath)) System.IO.File.WriteAllText(JsonPath, "[]");

            var json = System.IO.File.ReadAllText(JsonPath);
            return JsonSerializer.Deserialize<List<EventDto>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<EventDto>();
        }

        /// <summary>
        /// Saves event data back to JSON file.
        /// </summary>
        private void Save(IEnumerable<EventDto> items)
        {
            var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(JsonPath, json);
        }

        public void OnGet()
        {
            Events = Load().OrderBy(e => e.StartUtc).ToList();
        }


        public IActionResult OnPostAdd(string matchup, DateTimeOffset startUtc, string? card, string? venue, string? location)
        {
            var items = Load();
            items.Add(new EventDto
            {
                Matchup = matchup,
                StartUtc = startUtc,
                Card = card ?? "",
                Venue = venue ?? "",
                Location = location ?? ""
            });
            Save(items);
            return RedirectToPage();
        }

        /// <summary>
        /// Updates an existing event record.
        /// </summary>
        public IActionResult OnPostUpdate(string id, string matchup, DateTimeOffset startUtc, string? card, string? venue, string? location)
        {
            var items = Load().ToList();
            var index = items.FindIndex(e => e.Id == id);
            if (index >= 0)
            {
                items[index] = new EventDto
                {
                    Id = id,
                    Matchup = matchup,
                    StartUtc = startUtc,
                    Card = card ?? "",
                    Venue = venue ?? "",
                    Location = location ?? ""
                };
                Save(items);
            }
            return RedirectToPage();
        }
    }
}
