using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ContosoCrafts.WebSite.Pages
{
    public class EventsModel : PageModel
    {
        private readonly ILogger<EventsModel> _logger;

        public EventsModel(ILogger<EventsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
