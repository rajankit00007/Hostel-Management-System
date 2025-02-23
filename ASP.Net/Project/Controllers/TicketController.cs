﻿
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Controllers
{
    [Route("ticket/api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {

		


		// Constructor injection of the DbContext


		// Admin side method to get all request
		[HttpGet("all")]
        public ActionResult<List<Ticket>> GetTickets()
        {
			using (var _context = new p16_hostelContext())
			{
				if (_context.Tickets == null)
				{
					return NotFound();
				}

				var tickets = _context.Tickets
					.Include(t => t.Hostler)  // Include Hostler Details
					.ThenInclude(h => h.Roomallocations)  // Include RoomAllocations
					.ThenInclude(ra => ra.Room)  // Include Room
					.ToList();
				return tickets;
			}
        }

		// hostler side for getting all sent tickets
		[HttpGet("getTicket/{hostlerId}")]
		public async Task<ActionResult<List<Ticket>>> GetTicketsByHostlerId(int hostlerId)
		{
			using (var _context = new p16_hostelContext())
			{
				if (_context.Tickets == null)
				{
					return NotFound();
				}

				// Find all tickets for the given HostlerId
				var tickets = await _context.Tickets
							.Include(t => t.Hostler)
							.ThenInclude(h => h.Roomallocations)
							.ThenInclude(ra => ra.Room)
							.Where(t => t.Hostler.HostlerId == hostlerId)
							.ToListAsync();



				return Ok(tickets); // Return thje list of tickets for the hostler
			}
		}


		//hostler side for adding the Tickets
		[HttpPost("create")]
		public async Task<ActionResult<Ticket>> PostTicket(Ticket ticket)
		{
			using (var _context = new p16_hostelContext())
			{
				Console.WriteLine(ticket.HostlerId);
				if (_context.Tickets == null)
				{
					return Problem("Entity set 'p16_hostelContext.Tickets' is null.");
				}
				var issue = await _context.Issues.FindAsync(ticket.IssueId);
				if (issue == null)
				{
					return NotFound();
				}
				ticket.Issue = issue;
				ticket.Status = false;
				ticket.RaisedAt = DateTime.Now; // Ensure the current date is set
				_context.Tickets.Add(ticket);
				await _context.SaveChangesAsync();

				return Ok(ticket);
			}
		}

		
		[HttpPut("{id}/resolve")]
		public async Task<ActionResult<Ticket>> MakeResolved(int id,[FromBody] string description)
		{
			using (var _context = new p16_hostelContext()) {
				var ticket = await _context.Tickets
										   .Include(t => t.Issue)
										   .FirstOrDefaultAsync(t => t.TicketId == id);

			if (ticket == null)
			{
				return NotFound();
			}
			ticket.ResolvedAt = DateTime.Now;
			ticket.Status = true;
			await _context.SaveChangesAsync();
				SendResolutionEmail(ticket.HostlerId, description,ticket);

			return Ok(ticket);
			}

		}

		private void SendResolutionEmail(int hostlerId,string description,Ticket ticket)
		{
			using (var _context = new p16_hostelContext())
			{
				var hostler = _context.Hostlers.Find(hostlerId);
				if(hostler == null)
				{
					return;
				}
				var email = hostler.Email;
				var complaintype = ticket.Issue.IssueName ?? "Unknown Issue";
				var message = $"Dear {hostler.Firstname},\n\n" +
					  $"Your complaint regarding '{complaintype}' has been resolved.\n\n" +
					  $"Resolution Details: {description}\n\n" +
					  $"Regards,\nHostel Admin";

				try
				{
					var stmpClient = new SmtpClient("smtp.gmail.com")
					{
						Port = 587,
						Credentials = new NetworkCredential("atharva.lohe.al@gmail.com", "enbn rxgt yipr upxk"),
						EnableSsl = true
					};

					var mailMessage = new MailMessage
					{
						From = new MailAddress("atharva.lohe.al@gmail.com"),
						Subject = "Complaint Resolved",
						Body = message,
						IsBodyHtml = false
					};

					mailMessage.To.Add(email);
					stmpClient.Send(mailMessage);
				}
				catch (Exception ex)
				{
					Console.WriteLine("Email Could Not be Sent: "+ex.Message);
				}


			}
		}



	}
}
