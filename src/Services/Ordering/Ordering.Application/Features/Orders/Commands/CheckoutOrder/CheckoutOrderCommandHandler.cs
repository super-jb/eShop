﻿using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder;

public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly ILogger<CheckoutOrderCommandHandler> _logger;

    public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IEmailService emailService, ILogger<CheckoutOrderCommandHandler> logger)
    {
        _orderRepository = Guard.Against.Null(orderRepository, nameof(orderRepository));
        _mapper = Guard.Against.Null(mapper, nameof(mapper));
        _emailService = Guard.Against.Null(emailService, nameof(emailService));
        _logger = Guard.Against.Null(logger, nameof(logger));
    }

    public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        Order order = _mapper.Map<Order>(request);
        Order newOrder = await _orderRepository.AddAsync(order);

        _logger.LogInformation($"Order {newOrder.Id} is successfully created.");

        await SendMail(newOrder);

        return newOrder.Id;
    }

    private async Task SendMail(Order order)
    {
        var email = new Email
        {
            To = "ezozkme@gmail.com",
            Body = $"Order was created.",
            Subject = "Order was created"
        };

        try
        {
            await _emailService.SendEmail(email);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Order {order.Id} failed due to an error with the mail service: {ex.Message}");
        }
    }
}
