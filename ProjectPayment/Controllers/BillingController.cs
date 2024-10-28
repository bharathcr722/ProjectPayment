using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using ProjectPayment.Model;
using ProjectPayment.Services;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace ProjectPayment.Controllers
{
    public class BillingController : Controller
    {
        private readonly IPaymentService _paymentService;
        public BillingController(IPaymentService paymentService) {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadTranscation(IFormFile file)
        {
            if (file == null|| file.Length ==0 )
            {
                return BadRequest("no file uploaded");
            }
            try {            
            List<PaymentTranscation> transcations = new List<PaymentTranscation>();
                using (var stream = new StreamReader(file.OpenReadStream()))
                using(var csv = new CsvReader(stream, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.Delimiter.Insert(0, ",");
                    transcations = csv.GetRecords<PaymentTranscation>().ToList();
                }

                var matchedInvoices = _paymentService.MatchPayments(transcations);
                return Ok(matchedInvoices);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
