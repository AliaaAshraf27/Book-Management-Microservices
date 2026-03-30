using Application.Events;
using Application.IRepository;
using Application.Services.Notifications;
using Application.Services.Notifications.DTOS;
using Domain.Entity;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class BorrowingApprovedHandler(INotificationService _repo)
    {
        public async Task Handle(BorrowingApprovedEvent approvedEvent)
        {
            await _repo.CreateAsync(new CreateNotificationDto
            {
                UserId = approvedEvent.UserId,
                BookTitle = approvedEvent.BookTitle,
                Type = NotificationType.BorrowApproved,
                Message = $"Your borrow request for '{approvedEvent.BookTitle}' has been approved! Due date: {approvedEvent.DueDate:dd/MM/yyyy}"
            });
           
        }
    }
}
