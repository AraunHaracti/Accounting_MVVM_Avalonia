using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Avalonia.Controls;
using MySqlConnector;
using PZ17_MVVM.Models;
using PZ17_MVVM.Views.WindowTrainer.Dialogs;
using ReactiveUI;

namespace PZ17_MVVM.ViewModels;

public class TrainerWindowViewModel : ViewModelBase
{
    private readonly Window _parentWindow;
    
    private List<Trainer> _trainersFromDatabase;

    private List<Trainer> _trainersFilter;
    
    private ObservableCollection<Trainer> _trainersOnDataGrid;
    
    private string _searchQuery = "";
    
    private int _indexTake = 0;
    
    public Trainer CurrentItem { get; set; }
    
    public ObservableCollection<Trainer> TrainersOnDataGrid
    {
        get => _trainersOnDataGrid;
        set
        {
            _trainersOnDataGrid = value;
            this.RaisePropertyChanged();
        }
    }
    
    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            _searchQuery = value;
            UpdateTrainer();
        }
    }
    
    private int IndexTake
    {
        get => _indexTake;
        set
        {
            _indexTake = value;
            
            if (_indexTake > _trainersFilter.Count - 10)
            {
                _indexTake = _trainersFilter.Count - 10;
            }
            
            if (_indexTake < 0)
            {
                _indexTake = 0;
            } 
        }
    }

    public TrainerWindowViewModel(Window window)
    {
        _parentWindow = window;

        GetDataFromDatabase();

        _trainersFilter = _trainersFromDatabase;

        TakeFirstTrainer();

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
            _trainersFilter = new(_trainersFromDatabase);
        }
        _trainersFilter = new(_trainersFromDatabase.Where(it =>
            it.FirstName.ToLower().Contains(SearchQuery.ToLower()) ||
            it.MiddleName!.ToLower().Contains(SearchQuery.ToLower()) ||
            it.LastName!.ToLower().Contains(SearchQuery.ToLower()))
        );
    }

    
    public void UpdateTrainer()
    {
        GetDataFromDatabase();
        Search();
        TakeElements(TakeElementsEnum.FirstElements);
    }
    
    private void GetDataFromDatabase()
    {
        string sql = "select * " +
                     "from pz17.trainer";
        
        _trainersFromDatabase = new List<Trainer>();
        
        Database.Open();
    
        MySqlDataReader reader = Database.GetData(sql);
    
        while (reader.Read() && reader.HasRows)
        {
            var currentTrainer = new Trainer()
            {
                TrainerId = reader.GetInt32("TrainerId"),
                FirstName = reader.GetString("FirstName"),
                MiddleName = reader.GetString("MiddleName"),
                LastName = reader.GetString("LastName"),
            };
            
            _trainersFromDatabase.Add(currentTrainer);
        }
        
        Database.Exit();
    }

    public void AddTrainer()
    { 
        TrainerAddWindowView trainerAddWindowView = new TrainerAddWindowView(UpdateTrainer);
        trainerAddWindowView.ShowDialog(_parentWindow);
        UpdateTrainer();
    }
    
    
    public void DeleteTrainer()
    {
        if (CurrentItem == null) return;
        string sql = $"delete from pz17.trainer t where t.TrainerID = '{CurrentItem.TrainerId}'";
        Database.Open();
        Database.SetData(sql);
        Database.Exit();
        UpdateTrainer();
    }

    public void EditTrainer()
    {
        if (CurrentItem == null) return;
        TrainerEditWindowView trainerEditWindowView = new TrainerEditWindowView(UpdateTrainer, CurrentItem);
        trainerEditWindowView.ShowDialog(_parentWindow);
    }

    public void TakeFirstTrainer()
    {
        TakeElements(TakeElementsEnum.FirstElements);
    }

    public void TakePreviousTrainer()
    {
        TakeElements(TakeElementsEnum.PreviousElements);
    }

    public void TakeNextTrainer()
    {
        TakeElements(TakeElementsEnum.NextElements);
    }

    public void TakeLastTrainer()
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
                IndexTake = _trainersFilter.Count - 10;
                break;
            case TakeElementsEnum.NextElements:
                IndexTake += 10;
                break;
            case TakeElementsEnum.PreviousElements:
                IndexTake -= 10;
                break;
        }
        
        TrainersOnDataGrid = new ObservableCollection<Trainer>(_trainersFilter.GetRange(IndexTake, _trainersFilter.Count > 10 ? 10 : _trainersFilter.Count));
    }
    
    private enum TakeElementsEnum
    {
        FirstElements,
        PreviousElements,
        NextElements,
        LastElements
    }
}