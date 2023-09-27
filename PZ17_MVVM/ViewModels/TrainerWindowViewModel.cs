using System.Collections.Generic;
using System.Collections.ObjectModel;
using MySqlConnector;
using PZ17_MVVM.Models;

namespace PZ17_MVVM.ViewModels;

public class TrainerWindowViewModel
{
    public ObservableCollection<Trainer> Trainers { get; private set; }

    private List<Trainer> _trainers;

    public TrainerWindowViewModel()
    {
        string sql = "select * " +
                     "from pz17.trainer";
        _trainers = new List<Trainer>();
        
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
            
            _trainers.Add(currentTrainer);
        }
        
        Database.Exit();
        Trainers = new ObservableCollection<Trainer>(_trainers);
    }
}