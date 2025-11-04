#nullable enable
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
    public class EventsModel : PageModel
    {
        private readonly ILogger<EventsModel> _logger;
        private readonly IWebHostEnvironment _env;
        private static readonly object _fileLock = new object();

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
            lock (_fileLock)
            {
                try
                {
                    var dir = Path.GetDirectoryName(JsonPath)!;
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    if (!System.IO.File.Exists(JsonPath))
                    {
                        System.IO.File.WriteAllText(JsonPath, "[]");
                        return new List<EventDto>();
                    }

                    var json = System.IO.File.ReadAllText(JsonPath);

                    // Handle empty or whitespace-only files
                    if (string.IsNullOrWhiteSpace(json))
                    {
                        System.IO.File.WriteAllText(JsonPath, "[]");
                        return new List<EventDto>();
                    }

                    var result = JsonSerializer.Deserialize<List<EventDto>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result ?? new List<EventDto>();
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Failed to parse events.json. Resetting to empty list.");
                    System.IO.File.WriteAllText(JsonPath, "[]");
                    return new List<EventDto>();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error loading events");
                    return new List<EventDto>();
                }
            }
        }
        /// <summary>
        /// Saves updated event data back to the json file
        /// </summary>
        private void Save(IEnumerable<EventDto> items)
        {
            lock (_fileLock)
            {
                try
                {
                    var dir = Path.GetDirectoryName(JsonPath)!;
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });

                    // Write to temp file first, then move (atomic operation)
                    var tempPath = JsonPath + ".tmp";
                    System.IO.File.WriteAllText(tempPath, json);
                    System.IO.File.Move(tempPath, JsonPath, overwrite: true);

                    _logger.LogInformation("Saved {Count} events to {Path}", items.Count(), JsonPath);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving events");
                }
            }
        }

        public void OnGet()
        {
            Events = Load().OrderBy(e => e.StartUtc).ToList();
        }
        /// <summary>
        /// Adds a new event to the list and saves it to json
        /// </summary>
        public IActionResult OnPostAdd(string matchup, DateTimeOffset startUtc, string? card, string? venue, string? location)
        {
            try
            {
                var items = Load().ToList();
                items.Add(new EventDto
                {
                    Matchup = matchup,
                    StartUtc = startUtc,
                    Card = card ?? "",
                    Venue = venue ?? "",
                    Location = location ?? ""
                });
                Save(items);
                _logger.LogInformation("Added event: {Matchup}", matchup);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding event");
            }

            return RedirectToPage();
        }
        /// <summary>
        /// Updates an existing event by ID and the changes
        /// </summary>
        public IActionResult OnPostUpdate(string id, string matchup, DateTimeOffset startUtc, string? card, string? venue, string? location)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToPage();

            try
            {
                var items = Load().ToList();
                var ev = items.FirstOrDefault(e => e.Id == id);

                if (ev != null)
                {
                    ev.Matchup = matchup;
                    ev.StartUtc = startUtc;
                    ev.Card = card ?? "";
                    ev.Venue = venue ?? "";
                    ev.Location = location ?? "";

                    Save(items);
                    _logger.LogInformation("Updated event: {Id}", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating event {Id}", id);
            }

            return RedirectToPage();
        }
        /// <summary>
        /// Deletes an event by its ID from the json file
        /// </summary>
        public IActionResult OnPostDelete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToPage();

            try
            {
                var items = Load().ToList();
                var initialCount = items.Count;
                items.RemoveAll(e => e.Id == id);

                if (items.Count < initialCount)
                {
                    Save(items);
                    _logger.LogInformation("Deleted event: {Id}. Remaining events: {Count}", id, items.Count);
                }
                else
                {
                    _logger.LogWarning("Event {Id} not found for deletion", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting event {Id}", id);
            }

            return RedirectToPage();
        }
    }
}