using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniApp.Data;
using MiniApp.DTOs;
using MiniApp.Models;

namespace MiniApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public EventsController(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var events = await _context.Events.Include(e => e.Organizer).ToListAsync();
            return Ok(_mapper.Map<List<EventDto>>(events));
        }

        [HttpPost]
        public async Task<IActionResult> Create(EventDto dto)
        {
            var entity = _mapper.Map<Event>(dto);
            _context.Events.Add(entity);
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<EventDto>(entity));
        }

        [HttpPost("{id}/banner")]
        public async Task<IActionResult> UploadBanner(int id, IFormFile file)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null) return NotFound();

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var path = Path.Combine(_env.WebRootPath, "uploads", fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            ev.BannerImageUrl = $"/uploads/{fileName}";
            await _context.SaveChangesAsync();
            return Ok(ev.BannerImageUrl);
        }

        [HttpGet("{eventId}/tickets")]
        public async Task<IActionResult> GetTickets(int eventId)
        {
            var tickets = await _context.Tickets
                .Include(t => t.Event)
                .Where(t => t.EventId == eventId)
                .ToListAsync();
            return Ok(_mapper.Map<List<TicketDto>>(tickets));
        }

        [HttpGet("{eventId}/organizer")]
        public async Task<IActionResult> GetOrganizer(int eventId)
        {
            var ev = await _context.Events.Include(e => e.Organizer).FirstOrDefaultAsync(e => e.Id == eventId);
            if (ev == null) return NotFound();
            return Ok(_mapper.Map<OrganizerDto>(ev.Organizer));
        }

        [HttpPost("{eventId}/tickets")]
        public async Task<IActionResult> CreateTicket(int eventId, TicketDto dto)
        {
            if (eventId != dto.EventId) return BadRequest("EventId mismatch");
            var ticket = _mapper.Map<Ticket>(dto);
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<TicketDto>(ticket));
        }
    }

}
