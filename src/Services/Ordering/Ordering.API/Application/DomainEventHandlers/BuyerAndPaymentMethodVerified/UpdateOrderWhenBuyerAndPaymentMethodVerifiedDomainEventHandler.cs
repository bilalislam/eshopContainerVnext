using MediatR;
using Microsoft.eShopOnContainers.Services.Ordering.Domain.AggregatesModel.OrderAggregate;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Application.DomainEventHandlers.BuyerAndPaymentMethodVerified
{
    public class UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler
                   : INotificationHandler<BuyerAndPaymentMethodVerifiedDomainEvent>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILoggerFactory _logger;

        public UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler(
            IOrderRepository orderRepository, ILoggerFactory logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Domain Logic comment:
        // When the Buyer and Buyer's payment method have been created or verified that they existed, 
        // then we can update the original Order with the BuyerId and PaymentId (foreign keys)
        // Bounded context aslında bu işte . Order aynı bc de oldugu zaman microservice olur ve bir 
        // mikroservice birden fazla agg. root'u yonetebilir ki dbleri aynı olabilir soa gibi
        // ama order ayrı bir db de  ise o zaman bir context aslında başka bir context'in bilgilerini içerebilir
        // o zaman o context ki ayrılmıs olması lazım ama olmaya da bilir. Ayrık ise async degilse  sync olarak
        // tutarlılıgı saglamak gerekir ve sadece order ar'u buyer ar'un Id'sini ve payment entity'sinin idsini bilebilir.
        // peki burda eger event gondermesi gerekseydi mutlaka bir transaction scope'un bitmesi gerekirdi
        // yani bir domain'e apply edilen bir event birden fazla domain event handler'a düşebilir
        // ama eger bounded context içinde kaç ar varsa ve hangileri etkileniyorsa mutlaka hepsi bittikten
        // sonra integration event raise edilebilir domain eventlerin arasında event bus'a publish edilmez.Scope bitmedi çünkü.
        // eger hepsinin save edip eventleri  dispatch ediyorsam zaten ayrı bir bc'ye async olarak gonderiyorumdur.
        // eger save etmeden dispatch ediyorsam aynı bc'de farklı ar'ları yonetiyorumdur yani async olamaz.
        // peki buyer ve order aynı bc'de sync iken save edilene kadar model tamamlanır ve internal handler'lar context tutarlı hale getirir.
        // ama save ettikten sonra artık integration event atabilirim çünkü scope tamamlandı ve başka bir bc bundan etkilenebilir.
        // ozetle domain eventler hem sync olabilir eger scope tamamlandı ise async olarak bus'a pusblish edebilir.
        // integration handler'lar hem input olarak consumer'lar ile hem de output olarak (scope tamamlanmıs domain handler'dan) kullanılabilirler.
        public async Task Handle(BuyerAndPaymentMethodVerifiedDomainEvent buyerPaymentMethodVerifiedEvent, CancellationToken cancellationToken)
        {
            var orderToUpdate = await _orderRepository.GetAsync(buyerPaymentMethodVerifiedEvent.OrderId);
            orderToUpdate.SetBuyerId(buyerPaymentMethodVerifiedEvent.Buyer.Id);
            orderToUpdate.SetPaymentId(buyerPaymentMethodVerifiedEvent.Payment.Id);

            _logger.CreateLogger<UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler>()
                .LogTrace("Order with Id: {OrderId} has been successfully updated with a payment method {PaymentMethod} ({Id})",
                    buyerPaymentMethodVerifiedEvent.OrderId, nameof(buyerPaymentMethodVerifiedEvent.Payment), buyerPaymentMethodVerifiedEvent.Payment.Id);
        }
    }
}
