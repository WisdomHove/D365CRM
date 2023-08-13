using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using D365CRM.Data;
using D365CRM.Model;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;


namespace D365CRM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                ConfigureHttpClient(httpClient);

                var response = await httpClient.GetAsync("https://org2e1270cb.api.crm4.dynamics.com/api/data/v9.2/products");
                if (response.IsSuccessStatusCode)
                {
                    var products = await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
                    return Ok(products);
                }
                else
                {
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                ConfigureHttpClient(httpClient);

                var response = await httpClient.GetAsync($"https://org2e1270cb.api.crm4.dynamics.com/api/data/v9.2/products({id})");
                if (response.IsSuccessStatusCode)
                {
                    var product = await response.Content.ReadFromJsonAsync<Product>();
                    return Ok(product);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                ConfigureHttpClient(httpClient);

                var response = await httpClient.PostAsJsonAsync("https://org2e1270cb.api.crm4.dynamics.com/api/data/v9.2/products", product);
                if (response.IsSuccessStatusCode)
                {
                    var createdProduct = await response.Content.ReadFromJsonAsync<Product>();
                    return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
                }
                else
                {
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product updatedProduct)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                ConfigureHttpClient(httpClient);

                var response = await httpClient.PutAsJsonAsync($"https://org2e1270cb.api.crm4.dynamics.com/api/data/v9.2/products({id})", updatedProduct);
                if (response.IsSuccessStatusCode)
                {
                    return NoContent();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                ConfigureHttpClient(httpClient);

                var response = await httpClient.DeleteAsync($"https://org2e1270cb.api.crm4.dynamics.com/api/data/v9.2/products({id})");
                if (response.IsSuccessStatusCode)
                {
                    return NoContent();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private void ConfigureHttpClient(HttpClient httpClient)
        {
            var accessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Ii1LSTNROW5OUjdiUm9meG1lWm9YcWJIWkdldyIsImtpZCI6Ii1LSTNROW5OUjdiUm9meG1lWm9YcWJIWkdldyJ9.eyJhdWQiOiJodHRwczovL29yZzJlMTI3MGNiLmNybS5keW5hbWljcy5jb20iLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC9jOTFiMTcwMC04MmVhLTRjM2MtOWM4OS01YTM4OTQwM2RmZGUvIiwiaWF0IjoxNjkxOTQyNDU0LCJuYmYiOjE2OTE5NDI0NTQsImV4cCI6MTY5MTk0NjM1NCwiYWlvIjoiRTJGZ1lEQlplUFhHOTBjVHQrVk5YN09iVVNVOENRQT0iLCJhcHBpZCI6IjcyMjE5M2I2LTY0ZDMtNDRiYS1iMmRhLWFmMDNjYWQwOGI2OSIsImFwcGlkYWNyIjoiMSIsImlkcCI6Imh0dHBzOi8vc3RzLndpbmRvd3MubmV0L2M5MWIxNzAwLTgyZWEtNGMzYy05Yzg5LTVhMzg5NDAzZGZkZS8iLCJvaWQiOiJhOWZjZWI3MC1kODlmLTQ5ODgtOGNkNS1jMmQzNmU4NGU5MjUiLCJyaCI6IjAuQVhNQUFCY2J5ZXFDUEV5Y2lWbzRsQVBmM2djQUFBQUFBQUFBd0FBQUFBQUFBQUJ6QUFBLiIsInN1YiI6ImE5ZmNlYjcwLWQ4OWYtNDk4OC04Y2Q1LWMyZDM2ZTg0ZTkyNSIsInRpZCI6ImM5MWIxNzAwLTgyZWEtNGMzYy05Yzg5LTVhMzg5NDAzZGZkZSIsInV0aSI6Imh3OFZucHRhWmstdjBOX3p3ZjFvQUEiLCJ2ZXIiOiIxLjAiLCJ4bXNfY2FlIjoiMSJ9.OiZ8dImNaMAND73nQx41CBLlpS_Ayw8axxWIMWXLTQK4W3zwaSQpdGwpiljkYOHjmUtGezLAZwJt2Maj5OdejWu21Tlh0m8b9OE2oOUCRDMzDW7c8O0faKFsiEVNulZDHwkjkiaYXYuEXodyVOITyr_blT1PRg0s6yktNUncMJRORukwCue1kXMs2U1E4VRaidNFglm8l4XBDJv3ONv57jlYPCVXcu31PzEPsF7WkFBdoMim3laDEU90YrMcyPaD3Y-fBUSzTYZTR9M0cKrWlYIMNgj2Feg7myJuMdraHZVfyG2YCI5loKspg_2tRbNy62W_WAwkLz1fPbj_mBTj0Q"; // Obtain access token through Azure AD authentication
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
