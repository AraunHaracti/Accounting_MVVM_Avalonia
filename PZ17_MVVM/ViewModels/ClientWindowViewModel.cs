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
    private readonly Window _parentWindow;
    
    private List<Client> _clientsFromDatabase;

    private List<Client> _clientsFilter;
    
    private ObservableCollection<Client> _clientsOnDataGrid;
    
    private string _searchQuery = "";
    
    private int _indexTake = 0;
    
    public Client CurrentItem { get; set; }
    
    public ObservableCollection<Client> ClientsOnDataGrid
    {
        get => _clientsOnDataGrid;
        set
        {
            _clientsOnDataGrid = value;
            this.RaisePropertyChanged();
        }
    }
    
    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            _searchQuery = value;
            UpdateClient();
        }
    }
    
    private int IndexTake
    {
        get => _indexTake;
        set
        {
            _indexTake = value;
            
            if (_indexTake > _clientsFilter.Count - 10)
            {
                _indexTake = _clientsFilter.Count - 10;
            }
            
            if (_indexTake < 0)
            {
                _indexTake = 0;
            } 
        }
    }

    public ClientWindowViewModel(Window window)
    {
        _parentWindow = window;
        
        GetDataFromDatabase();

        _clientsFilter = _clientsFromDatabase;
        
        TakeFirstClient();
        
        PropertyChanged += OnSearchQueryChanged;
    }

    private void OnSearchQueryChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(SearchQuery)) return;
        Search();
    }

    private void Search()
    {
        if (SearchQuery == "")
        {
            _clientsFilter = new(_clientsFromDatabase);
        }
        _clientsFilter = new(_clientsFromDatabase.Where(it =>
            it.FirstName.ToLower().Contains(SearchQuery.ToLower()) ||
            it.MiddleName!.ToLower().Contains(SearchQuery.ToLower()) ||
            it.LastName!.ToLower().Contains(SearchQuery.ToLower()))
        );
    }

    public void UpdateClient()
    {
        GetDataFromDatabase();
        Search();
        TakeElements(TakeElementsEnum.FirstElements);
    }
    
    private void GetDataFromDatabase()
    {
        string sql = "select * " +
                     "from pz17.client";
        
        _clientsFromDatabase = new List<Client>();
        
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
            
            _clientsFromDatabase.Add(currentClient);
        }
        
        Database.Exit();
    }
    
    public void AddClient()
    { 
        ClientAddWindowView clientAddWindowView = new ClientAddWindowView(UpdateClient);
        clientAddWindowView.ShowDialog(_parentWindow);
        UpdateClient();
    }
    
    
    public void DeleteClient()
    {
        if (CurrentItem == null) return;
        string sql = $"delete from pz17.client c where c.ClientID = '{CurrentItem.ClientId}'";
        Database.Open();
        Database.SetData(sql);
        Database.Exit();
        UpdateClient();
    }

    public void EditClient(Window window)
    {
        if (CurrentItem == null) return;
        ClientEditWindowView clientEditWindowView = new ClientEditWindowView(UpdateClient, CurrentItem);
        clientEditWindowView.ShowDialog(_parentWindow);
        UpdateClient();
    }

    public void TakeFirstClient()
    {
        TakeElements(TakeElementsEnum.FirstElements);
    }

    public void TakePreviousClient()
    {
        TakeElements(TakeElementsEnum.PreviousElements);
    }

    public void TakeNextClient()
    {
        TakeElements(TakeElementsEnum.NextElements);
    }

    public void TakeLastClient()
    {
        TakeElements(TakeElementsEnum.LastElements);
    }

    private void TakeElements(TakeElementsEnum takeElements)
    {
        switch (takeElements)
        {
            case TakeElementsEnum.FirstElements:
                IndexTake = 0;
                break;
            case TakeElementsEnum.LastElements:
                IndexTake = _clientsFilter.Count - 10;
                break;
            case TakeElementsEnum.NextElements:
                IndexTake += 10;
                break;
            case TakeElementsEnum.PreviousElements:
                IndexTake -= 10;
                break;
        }
        
        ClientsOnDataGrid = new ObservableCollection<Client>(_clientsFilter.GetRange(IndexTake, _clientsFilter.Count > 10 ? 10 : _clientsFilter.Count));
    }
    
    private enum TakeElementsEnum
    {
        FirstElements,
        PreviousElements,
        NextElements,
        LastElements
    }
}