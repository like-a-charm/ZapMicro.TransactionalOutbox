using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Entities;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Services;
using ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.Shared.API;

namespace ZapMicro.TransactionalOutbox.Samples.CreateOrderSaga.OrderService.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        
        public OrderController(ILogger<OrderController> logger, IOrderService orderService, IMapper mapper)
        {
            _logger = logger;
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost(Name = "CreateOrder")]
        public async Task<ActionResult> Post([FromBody] CreateOrderRequest createOrderRequest)
        {
            var lines = createOrderRequest.Lines.Select(x => _mapper.Map<OrderLine>(x));
            var adjustments = createOrderRequest.Adjustments.Select(x => _mapper.Map<Adjustment>(x));
            var order = await _orderService.CreateOrder(lines, adjustments);
            return new ObjectResult(order)
            {
                StatusCode = 201
            };
        }
    }
}