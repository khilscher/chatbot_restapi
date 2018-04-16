using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Collections.Generic;
using EGDRestApi.Models;
using Newtonsoft.Json;

namespace EGDRestApi
{
    public static class Authenticate
    {
        [FunctionName("Authenticate")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("Authenticate API called.");

            Customer customer1 = new Customer();
            customer1.FirstName = "Jane";
            customer1.LastName = "Doe";
            customer1.StreetName = "Cherry Lane";
            customer1.HouseNumber = 111;
            customer1.Email = "user@domain.com";

            Customer customer2 = new Customer();
            customer2.FirstName = "Kevin";
            customer2.LastName = "Hilscher";
            customer2.StreetName = "Valley Ridge Rd";
            customer2.HouseNumber = 1814;
            customer2.Email = "user@domain.com";

            List<Customer> customerList = new List<Customer>();
            customerList.Add(customer1);
            customerList.Add(customer2);

            Customer unauthenticatedCustomer = await req.Content.ReadAsAsync<Customer>();

            Customer authenticatedCustomer = customerList.Find(x => x.LastName == unauthenticatedCustomer.LastName);

            if(authenticatedCustomer != null)
            {
                log.Info("**********************************************************************");
                log.Info("Found match!");
                log.Info($"First Name: {authenticatedCustomer.FirstName}");
                log.Info($"Last Name: {authenticatedCustomer.LastName}");
                log.Info($"House #: {authenticatedCustomer.HouseNumber}");
                log.Info($"Street Name: {authenticatedCustomer.StreetName}");
                log.Info($"Email: {authenticatedCustomer.Email}");
                log.Info("**********************************************************************");

                return req.CreateResponse(HttpStatusCode.OK, authenticatedCustomer);
            }

            log.Info("**********************************************************************");
            log.Info("No match found!");
            log.Info("**********************************************************************");

            return req.CreateResponse(HttpStatusCode.BadRequest);

        }
    }
}
