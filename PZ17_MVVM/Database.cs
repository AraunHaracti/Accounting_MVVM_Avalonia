using MySqlConnector;

namespace PZ17_MVVM;

public static class Database
{
    private static string _connString = "server=localhost;port=3306;database=pz17;user=root;password=password;";
    private static MySqlConnection _connector;
    
    public static MySqlDataReader GetData(string sql)
    {
        MySqlCommand command = new MySqlCommand(sql, _connector);
        MySqlDataReader reader = command.ExecuteReader();

        return reader;
    }

    public static void SetData(string sql)
    {
        MySqlCommand command = new MySqlCommand(sql, _connector);
        command.ExecuteNonQuery();
    }

    public static void Open()
    {
        _connector = new MySqlConnection(_connString);
        _connector.Open();
    }
    
    public static void Exit()
    {
        _connector.Close();
    }
}