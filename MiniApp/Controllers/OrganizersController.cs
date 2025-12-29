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
    public class OrganizersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public OrganizersController(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var organizers = await _context.Organizers.ToListAsync();
            return Ok(_mapper.Map<List<OrganizerDto>>(organizers));
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrganizerDto dto)
        {
            var entity = _mapper.Map<Organizer>(dto);
            _context.Organizers.Add(entity);
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<OrganizerDto>(entity));
        }

        [HttpPost("{id}/logo")]
        public async Task<IActionResult> UploadLogo(int id, IFormFile file)
        {
            var org = await _context.Organizers.FindAsync(id);
            if (org == null) return NotFound();

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var path = Path.Combine(_env.WebRootPath, "uploads", fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            org.LogoUrl = $"/uploads/{fileName}";
            await _context.SaveChangesAsync();
            return Ok(org.LogoUrl);
        }

        [HttpGet("{organizerId}/events")]
        public async Task<IActionResult> GetEvents(int organizerId)
        {
            var events = await _context.Events.Where(e => e.OrganizerId == organizerId).ToListAsync();
            return Ok(_mapper.Map<List<EventDto>>(events));
        }
    }

}
