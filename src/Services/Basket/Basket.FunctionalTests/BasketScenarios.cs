using Basket.FunctionalTests.Base;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Basket.FunctionalTests.Helpers;
using Xunit;

namespace Basket.FunctionalTests
{
    /// <summary>
    /// Aslında bu da  api'in , UI tarafından contract test için mocklanarak temsil ettiği yer.
    /// Yani api'da UI olmaksızın kendini fake data ile test etmiş oluyor.
    /// Bunun birleşimi ise e2e çünkü uyumsuzluk olabilir . Contract'ın talebi ise fe tarafından yani client
    /// tarafından verilmelidir. Api'in test ortamına çıkmadan çalışması gereken en son test.
    /// https://www.pgs-soft.com/blog/test-pyramid-in-practice/
    /// </summary>
    public class BasketScenarios
        : BasketScenarioBase
    {
        [Fact]
        public async Task Get_basket_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync(Get.GetBasket(1));

                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task Post_basket_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {
                var content = new StringContent(BuildBasket("1"), UTF8Encoding.UTF8, "application/json");
                var response = await server.CreateClient()
                    .PostAsync(Post.Basket, content);

                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task Send_Checkout_basket_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {
                var contentBasket = new StringContent(BuildBasket("1"), UTF8Encoding.UTF8, "application/json");

                await server.CreateClient()
                    .PostAsync(Post.Basket, contentBasket);

                var contentCheckout = new StringContent(BuildCheckout("1"), UTF8Encoding.UTF8, "application/json");

                var response = await server.CreateIdempotentClient()
                    .PostAsync(Post.CheckoutOrder, contentCheckout);

                await server.CreateClient()
                    .DeleteAsync(Delete.DeleteBasket("1"));

                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task Delete_basket_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {
                var contentBasket = new StringContent(BuildBasket("2"), UTF8Encoding.UTF8, "application/json");

                await server.CreateClient()
                    .PostAsync(Post.Basket, contentBasket);

                var response = await server.CreateClient()
                    .DeleteAsync(Delete.DeleteBasket("2"));

                response.EnsureSuccessStatusCode();
            }
        }


        string BuildBasket(string id)
        {
            var order = FakeDataGenerator.CreateBasketContract(id);
            return JsonConvert.SerializeObject(order);
        }

        string BuildCheckout(string id)
        {
            var checkoutBasket = FakeDataGenerator.CreateBasketCheckout(id);
            return JsonConvert.SerializeObject(checkoutBasket);
        }
    }
}