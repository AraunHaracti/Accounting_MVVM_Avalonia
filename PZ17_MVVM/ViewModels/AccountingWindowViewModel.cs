using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Avalonia.Controls;
using MySqlConnector;
using PZ17_MVVM.Models;
using ReactiveUI;

namespace PZ17_MVVM.ViewModels;

public class AccountingWindowViewModel : ViewModelBase
{
    private readonly Window _parentWindow;
    
    private List<Accounting> _accountingFromDatabase;

    private List<Accounting> _accountingFilter;
    
    private ObservableCollection<Accounting> _accountingOnDataGrid;
    
    private string _searchQuery = "";
    
    private int _indexTake = 0;
    
    public Accounting CurrentItem { get; set; }
    
    public ObservableCollection<Accounting> AccountingOnDataGrid
    {
        get => _accountingOnDataGrid;
        set
        {
            _accountingOnDataGrid = value;
            this.RaisePropertyChanged();
        }
    }
    
    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            _searchQuery = value;
            UpdateAccounting();
        }
    }
    
    private int IndexTake
    {
        get => _indexTake;
        set
        {
            _indexTake = value;
            
            if (_indexTake > _accountingFilter.Count - 10)
            {
                _indexTake = _accountingFilter.Count - 10;
            }
            
            if (_indexTake < 0)
            {
                _indexTake = 0;
            } 
        }
    }

    public AccountingWindowViewModel(Window window)
    {
        _parentWindow = window;
        
        GetDataFromDatabase();

        _accountingFilter = _accountingFromDatabase;
        
        TakeFirstAccounting();
        
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
            _accountingFilter = new(_accountingFromDatabase);
        }
        _accountingFilter = new(_accountingFromDatabase.Where(it =>
            it.TrainerFullName.ToLower().Contains(SearchQuery.ToLower()) ||
            it.ClientFullName.ToLower().Contains(SearchQuery.ToLower()))
        );
    }

    public void UpdateAccounting()
    {
        GetDataFromDatabase();
        Search();
        TakeElements(TakeElementsEnum.FirstElements);
    }
    
    private void GetDataFromDatabase()
    {
        string sql = "select a.AccountingID as AccountingID, a.TrainerID, a.ClientID, " +
                     "concat(t.FirstName, ' ', t.MiddleName, ' ', t.LastName) as TrainerFullName, " +
                     "concat(c.FirstName, ' ', c.MiddleName, ' ', c.LastName) as ClientFullName, " +
                     "a.StartDate, a.CountClasses " +
                     "from pz17.trainer t " +
                     "join pz17.accounting a on t.TrainerID = a.TrainerID " +
                     "join pz17.client c on a.ClientID  = c.ClientID ";
        
        _accountingFromDatabase = new List<Accounting>();
        
        Database.Open();
    
        MySqlDataReader reader = Database.GetData(sql);
    
        while (reader.Read() && reader.HasRows)
        {
            var currentAccounting = new Accounting()
            {
                AccountingId = reader.GetInt32("AccountingId"),
                TrainerId = reader.GetInt32("TrainerId"),
                TrainerFullName = reader.GetString("TrainerFullName"),
                ClientId = reader.GetInt32("ClientId"),
                ClientFullName = reader.GetString("ClientFullName"),
                StartDate = reader.GetDateTime("StartDate"),
                CountClasses = reader.GetInt32("CountClasses")
            };
            
            _accountingFromDatabase.Add(currentAccounting);
        }
        
        Database.Exit();
    }
    
    public void AddAccounting()
    { 
        // ClientAddWindowView clientAddWindowView = new ClientAddWindowView(UpdateClient);
        // clientAddWindowView.ShowDialog(_parentWindow);
    }
    
    
    public void DeleteAccounting()
    {
        if (CurrentItem == null) return;
        string sql = $"delete from pz17.accounting a where a.AccountingID = '{CurrentItem.AccountingId}'";
        Database.Open();
        Database.SetData(sql);
        Database.Exit();
        UpdateAccounting();
    }

    public void EditAccounting(Window window)
    {
        if (CurrentItem == null) return;
        // ClientEditWindowView clientEditWindowView = new ClientEditWindowView(UpdateClient, CurrentItem);
        // clientEditWindowView.ShowDialog(_parentWindow);
    }

    public void TakeFirstAccounting()
    {
        TakeElements(TakeElementsEnum.FirstElements);
    }

    public void TakePreviousAccounting()
    {
        TakeElements(TakeElementsEnum.PreviousElements);
    }

    public void TakeNextAccounting()
    {
        TakeElements(TakeElementsEnum.NextElements);
    }

    public void TakeLastAccounting()
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
                IndexTake = _accountingFilter.Count - 10;
                break;
            case TakeElementsEnum.NextElements:
                IndexTake += 10;
                break;
            case TakeElementsEnum.PreviousElements:
                IndexTake -= 10;
                break;
        }
        
        AccountingOnDataGrid = new ObservableCollection<Accounting>(_accountingFilter.GetRange(IndexTake, _accountingFilter.Count > 10 ? 10 : _accountingFilter.Count));
    }
    
    private enum TakeElementsEnum
    {
        FirstElements,
        PreviousElements,
        NextElements,
        LastElements
    }
}