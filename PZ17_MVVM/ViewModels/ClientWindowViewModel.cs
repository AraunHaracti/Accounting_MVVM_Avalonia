using System.Collections.Generic;
using System.Collections.ObjectModel;
using MySqlConnector;
using PZ17_MVVM.Models;

namespace PZ17_MVVM.ViewModels;

public class ClientWindowViewModel
{
    public ObservableCollection<Client> Clients { get; private set; }

    private List<Client> _clients;

    public ClientWindowViewModel()
    {
        string sql = "select * " +
                     "from pz17.client";
        
        _clients = new List<Client>();
        
        Database.Open();
    
        MySqlDataReader reader = Database.GetData(sql);
    
        while (reader.Read() && reader.HasRows)
        {
            var currentClient = new Client()
            {
                ClientId = reader.GetInt32("ClientId"),
                FirstName = reader.GetString("FirstName"),
                MiddleName = reader.GetString("MiddleName"),
                LastName = reader.GetString("LastName"),
                Dob = reader.GetDateOnly("DOB")
            };
            
            _clients.Add(currentClient);
        }
        
        Database.Exit();
        Clients = new ObservableCollection<Client>(_clients);
    }
}