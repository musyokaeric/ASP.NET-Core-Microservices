using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Unit>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        private readonly ILogger<UpdateOrderCommandHandler> logger;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<UpdateOrderCommandHandler> logger)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            // await orderRepository.UpdateAsync(mapper.Map<Order>(request));

            var order = await orderRepository.GetByIdAsync(request.Id);
            if (order == null)
            {
                logger.LogError("Order does not exist.");
                throw new NotFoundException(nameof(Order), request.Id);
            }
            mapper.Map(request, order, typeof(UpdateOrderCommand), typeof(Order));
            await orderRepository.UpdateAsync(order);

            logger.LogInformation($"Order {order.Id} updated successfully.");

            return Unit.Value;
        }
    }
}
