using Microsoft.EntityFrameworkCore;

class TransactionHeader { }
interface ITransaction
{
    public void Confirm(object requestMessage, ref object responseMessage, TransactionHeader transactionHeader);
    public void Execute(object requestMessage, ref object responseMessage, TransactionHeader transactionHeader);
    public void Start(object requestMessage, ref object responseMessage, TransactionHeader transactionHeader);
}


class MoneyTransferHayyamTransaction : ITransaction
{
    public void Confirm(object requestMessage, ref object responseMessage, TransactionHeader transactionHeader)
    {
        throw new NotImplementedException();
    }

    public void Execute(object requestMessage, ref object responseMessage, TransactionHeader transactionHeader)
    {
        MoneyTransferHayyamRequest moneyTransferHayyamRequest = (MoneyTransferHayyamRequest)requestMessage;
        MoneyTransferHayyamResponse moneyTransferHayyamResponse = (MoneyTransferHayyamResponse)requestMessage;

        using var context = new Context();
        using var transaction = context.Database.BeginTransaction();
        try
        {
            using var currencyService = new CurrencyService();
            // var trensferTo = contex.accounts.find(moneyTransferHayyamRequest.TransferTo) 
            currencyService.ConvertCurrency(moneyTransferHayyamRequest.Amount,"... Transfer to account from context account ");
            // ... execute
            

            context.SaveChanges();
            transaction.Commit();

        }
        catch (System.Exception)
        {
            transaction.Dispose();
            throw;
        }

    }

    public void Start(object requestMessage, ref object responseMessage, TransactionHeader transactionHeader)
    {
        throw new NotImplementedException();
    }
}


class MoneyTransferHayyamRequest
{
    public int TransferFrom { get; set; }
    public int TransferTo { get; set; }
    public Amount Amount { get; set; }
    public DateTime Date { get; set; }
    public Scheduled? Scheduled { get; set; }
    public string? Description { get; set; }
}

class Scheduled
{
    public Frequency Frequency { get; set; }
    public DateTime EndDate { get; set; }
}

class Amount
{
    public string Currency { get; set; }
    public decimal Value { get; set; }
}
enum Frequency
{
    Daily,
    Weekly,
    Bi_Weekly,
    Monthly,
    Yearly
}
class MoneyTransferHayyamResponse
{
    public int TransferFrom { get; set; }
    public int TransferTo { get; set; }
    public Amount Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
}





class Context : DbContext
{

}


class CurrencyService :IDisposable
{
    public Amount ConvertCurrency(Amount amount, string Currency)
    {

        // ... Conver business codes ...
        return new Amount() { Currency = Currency, Value = amount.Value };
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}