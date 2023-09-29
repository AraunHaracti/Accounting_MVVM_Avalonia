using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Avalonia.Controls;
using MySqlConnector;
using PZ17_MVVM.Models;
using PZ17_MVVM.Views.WindowClient.Dialogs;
using ReactiveUI;

namespace PZ17_MVVM.ViewModels;

public class ClientWindowViewModel : ViewModelBase
{
    private Window owner;
    public ObservableCollection<Client> Clients
    {
        get => _clients1;
        set
        {
            _clients1 = value;
            this.RaisePropertyChanged(nameof(Clients));
        }
    }
    
    public Client CurrentItem { get; set; }

    private List<Client> _clients;

    private string _searchQuery = "";
    private ObservableCollection<Client> _clients1;

    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            _searchQuery = value;
            this.RaisePropertyChanged();
        }
    }

    public void DeleteClient()
    {
        string sql = $"delete from pz17.client c where c.ClientID = '{CurrentItem.ClientId}'";
        Database.Open();
        Database.SetData(sql);
        Database.Exit();
        GetDataFromDatabase();
        this.RaisePropertyChanged();
    }

    public void EditClient(Window window)
    {
        ClientEditWindowView cewv = new ClientEditWindowView(CurrentItem);
        cewv.ShowDialog(owner);
        GetDataFromDatabase();
        this.RaisePropertyChanged();
    }

    public void UpdateClient()
    {
        GetDataFromDatabase();
        this.RaisePropertyChanged();
    }

    public void AddClient()
    { 
        ClientAddWindowView cawv = new ClientAddWindowView();
        cawv.ShowDialog(owner);
    }
    
    public ClientWindowViewModel(Window window)
    {
        owner = window;
        GetDataFromDatabase();
        PropertyChanged += OnSearchQueryChanged;
    }

    private void OnSearchQueryChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(SearchQuery)) return;
        if (SearchQuery == "")
        {
            Clients = new(_clients);
        }
        Clients = new(Clients.Where(it =>
            it.FirstName.ToLower().Contains(SearchQuery.ToLower()) ||
            it.MiddleName.ToLower().Contains(SearchQuery.ToLower()) ||
            it.LastName.ToLower().Contains(SearchQuery.ToLower()))
            );
    }

    private void GetDataFromDatabase()
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
                Dob = reader.GetDateTime("DOB")
            };
            
            _clients.Add(currentClient);
        }
        
        Database.Exit();
        Clients = new ObservableCollection<Client>(_clients);
    }
}