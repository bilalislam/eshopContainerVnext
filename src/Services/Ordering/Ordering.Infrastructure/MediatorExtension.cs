using MediatR;
using Microsoft.eShopOnContainers.Services.Ordering.Domain.Seedwork;
using Microsoft.eShopOnContainers.Services.Ordering.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Infrastructure
{
    static class MediatorExtension
    {
        // Burada domain ve integration event raise  edilebilir.
        // çünkü biri context'i tamamlarken diger diger bc'yi tamamlar.
        // sorun anında domain eventler rollback edilebilirler ama integration eventler rollback edilmez.
        // peki ie'ler domain mi create edilecek ? Aslında application'da edilmesi lazım
        // o zaman burasının integration raise etmesine gerek kalmaz çünkü bc 2 ar'den olustugu için
        // bunun handle edildiği yerde ie raise edilirse bu sefer save edilmeden ie'ler gider
        // yani tek bir transaction'a domain ve integration eventler nasıl sıgar ?
        // transaction behaivor nedir ?
        // https://github.com/dotnet-architecture/eShopOnContainers/blob/dev/src/Services/Ordering/Ordering.API/Application/Behaviors/TransactionBehaviour.cs
        // https://github.com/dotnet-architecture/eShopOnContainers/blob/dev/src/Services/Ordering/Ordering.API/Application/IntegrationEvents/OrderingIntegrationEventService.cs
        // hx de ise basket query ie iken içeri alındıgında domain oldu ve dispacher buna uygun değildi.
        // o yuzden app layer'da 2pc commit olarak yaptık ya da domain service'te. 
        // dispatcher 'ın integration mı yoksa domain event lerden mi sorumlu olacagı bastan belirlenmesi lazım
        // yok domain dispatcher ie ya da ie dispatcher'e domain raise edilmez.
        // aslında transacton behaivorda outbox pattern ve db call over kill belki ama zaten geçici süre zaten bölünecek
        // ve eger routing slip lineer devam edip de bu kadar uzun integraion event raise'i varsa zaten yanlıs modellemişizdir.
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, OrderingContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent);
        }
    }
}
