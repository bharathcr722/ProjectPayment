using ProjectPayment.Model;
using System.CodeDom;
using System.Transactions;

namespace ProjectPayment.Services
{
    public class PaymentService : IPaymentService
    {
       
        private readonly IInvoiceService  _invoiceService;
        private readonly ICustomerService  _customerService;
        private readonly BillingDBContext  _context;
        public PaymentService(IInvoiceService invoiceService,BillingDBContext context, ICustomerService customerService)
        {
            _invoiceService = invoiceService;
            _context = context;
            _customerService = customerService;
        }


        public bool IsPaymentMatched(List<PaymentTranscation> transcation , List<Invoice> invoice)
        {
            var paymentDetails = transcation.Where(transcation => transcation.Type == "Credit")
                .GroupBy(g => g.CustomerID)
                .Select(s => new
                {
                    customerId = s.Key,
                    totalAmount = s.Sum(g => g.Amount),
                    transcation = s.Select(s => new { s.Amount, s.TransactionID }).ToList()
                }  );

            foreach(var item in paymentDetails)
            {
                var currCustoemerProject = _customerService.GetProjectByCustomer(item.customerId);
                var currInvoices = invoice.FindAll(f => currCustoemerProject.Any(a=>a.ProjectId == f.ProjectId));
                if(currInvoices.Sum(s => s.Amount ) != item.totalAmount)
                {
                    return true;
                }
                else
                {
                    return currInvoices.Any(a=> a.Amount == item.totalAmount);
                }
            }
            return false;
        }

        public List<MatchedInvoice> MatchPayments(List<PaymentTranscation> transcations)
        {
            var matchedInvoices = new List<MatchedInvoice>();
            var invoices = _invoiceService.GetAllInvoices();
            var invoiceMap = invoices.ToDictionary(i => i.InvoiceId, i => i.Amount);
            int customerIDs = transcations.FirstOrDefault(f => f.CustomerID != 0)?.CustomerID ?? 0;
            var project = _customerService.GetProjectByCustomer(customerIDs);
            invoices = invoices.FindAll(f => project.Any(a => a.ProjectId == f.ProjectId));
            if (!IsPaymentMatched(transcations,invoices))
            {
                 throw new InvalidOperationException("Transaction could not abel to fully match invoices or one invoice");
            }

            decimal totalMatchedAmount = 0;
            foreach (var tranaction in transcations.Where(t=> t.Type == "Credit"))
            { 
                decimal transactionAmount = tranaction.Amount;
                List<int> matchedInvoiceIds = new List<int>();
                foreach (var invoiceID in invoiceMap.Keys.ToList())
                {
                    if (invoiceMap[invoiceID] > 0)
                    {
                        if (transactionAmount >= invoiceMap[invoiceID])
                        {
                            matchedInvoiceIds.Add(invoiceID);
                            totalMatchedAmount += invoiceMap[invoiceID];
                            transactionAmount -= invoiceMap[invoiceID];
                            invoiceMap[invoiceID] = 0;
                        }
                    }
                }

                if (transactionAmount == 0)
                {
                    matchedInvoices = GetMatchedInvoices(matchedInvoiceIds, invoices);
                    UpdateTransaction(matchedInvoices, tranaction);
                }
                else
                {
                  throw new InvalidOperationException(tranaction.Amount + " Could not fully matched ");
                }
            }
            _context.SaveChanges();
            return matchedInvoices;
        }

        public List<MatchedInvoice> GetMatchedInvoices(List<int> matchedInvoices, List<Invoice> allInvoices )
        {
            List<MatchedInvoice> invoices = new List<MatchedInvoice>();
            foreach (var matchedId in matchedInvoices) {
                invoices.Add(new MatchedInvoice
                {
                    InvoiceId = matchedId,
                    MatchedAmount = allInvoices.First(i => i.InvoiceId == matchedId).Amount,
                    InvoiceDate = allInvoices.First(i => i.InvoiceId == matchedId).Date,
                    Status = "Paid"
                });

            }

            return invoices;
        }

        public void UpdateTransaction(List<MatchedInvoice> matchedInvoices,PaymentTranscation transaction)
        {
            foreach (MatchedInvoice invoice in matchedInvoices)
            {
                var invoiceToUpdate = _context.Invoices.Find(invoice.InvoiceId);
                if (invoiceToUpdate != null) {
                    invoiceToUpdate.Amount = 0;
                    _context.Invoices.Update(invoiceToUpdate);

                    var paymentTransaction = new PaymentTranscation
                    {
                        Date = DateTime.UtcNow,
                        Type = "Credit",
                        Amount = transaction.Amount,
                        CustomerID = transaction.CustomerID,
                    };
                    _context.PaymentTranscations.Add(paymentTransaction);
                }
            }
        }

    }
}
