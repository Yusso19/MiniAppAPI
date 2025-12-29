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
    public class TicketsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TicketsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tickets = await _context.Tickets.ToListAsync();
            return Ok(_mapper.Map<List<TicketDto>>(tickets));
        }

        [HttpPost]
        public async Task<IActionResult> Create(TicketDto dto)
        {
            var ticket = _mapper.Map<Ticket>(dto);
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<TicketDto>(ticket));
        }
    }

}
