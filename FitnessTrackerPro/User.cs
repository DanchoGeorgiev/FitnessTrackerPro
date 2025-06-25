using System.Text;

namespace FitnessTrackerPro;

public class User
{
    private string name;
    private string email;
    string password;

    public string Name
    {
        get => name;
        set => name = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Email
    {
        get => email;
        set => email = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Password
    {
        get => password;
        set => password = value ?? throw new ArgumentNullException(nameof(value));
    }

    public User(string name, string email, string password)
    {
        this.Name = name;
        this.Email = email;
        this.Password = password;
    }

    public void SaveUser()
    {
        StreamWriter sw = new StreamWriter("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/UserData.txt", true, Encoding.UTF8);
        sw.WriteLine(Name);
        sw.WriteLine(Email);
        sw.WriteLine(Password);
        sw.Close();
    }

    public bool ValidateUser()
    {
        List<string> data = File.ReadAllLines("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/UserData.txt", Encoding.UTF8).ToList();
        if (data[0] == Name && data[2] == Password)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}