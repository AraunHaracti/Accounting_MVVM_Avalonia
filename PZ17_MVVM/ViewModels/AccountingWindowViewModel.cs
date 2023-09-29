using System.Collections.Generic;
using System.Collections.ObjectModel;
using MySqlConnector;
using PZ17_MVVM.Models;

namespace PZ17_MVVM.ViewModels;

public class AccountingWindowViewModel
{
    public ObservableCollection<Accounting> Accountings { get; private set; }
    
    private List<Accounting> _accountings;
    
    public AccountingWindowViewModel()
    {
        string sql = "select a.AccountingID as AccountingID, a.TrainerID, a.ClientID, " +
                     "a.StartDate, a.CountClasses " +
                     "from pz17.trainer t " +
                     "join pz17.accounting a on t.TrainerID = a.TrainerID " +
                     "join pz17.client c on a.ClientID  = c.ClientID ";

        _accountings = new List<Accounting>();

        Database.Open();
        
        MySqlDataReader reader = Database.GetData(sql);

        while (reader.Read() && reader.HasRows)
        {
            var currentAccounting = new Accounting()
            {
                AccountingId = reader.GetInt32("AccountingId"),
                TrainerId = reader.GetInt32("TrainerId"),
                ClientId = reader.GetInt32("ClientId"),
                StartDate = reader.GetDateTime("StartDate"),
                CountClasses = reader.GetInt32("CountClasses")
            };
            
            _accountings.Add(currentAccounting);
        }
        Database.Exit();
        Accountings = new ObservableCollection<Accounting>(_accountings);
    }
}