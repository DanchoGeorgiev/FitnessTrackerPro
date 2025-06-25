namespace FitnessTrackerPro;

public enum DaysOfWeek
{
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday
}

public static class Dates
{
    static DaysOfWeek dayOfWeek;
    private static int day;
    private static int month;
    private static int year;


    public static DaysOfWeek DayOfWeek
    {
        get => dayOfWeek;
        set => dayOfWeek = value;
    }
    
    public static int Day
    {
        get => day;
        set => day = value;
    }

    public static int Month
    {
        get => month;
        set => month = value;
    }

    public static int Year
    {
        get => year;
        set => year = value;
    }

    public static void SetDate(string dayOfWeek1, int day, int month, int year)
    {
        DayOfWeek = (DaysOfWeek)Enum.Parse(typeof(DaysOfWeek), dayOfWeek1);
        Day = day;
        Month = month;
        Year = year;
    }
    
    public static bool IsLeapYear(int year)
    {
        return (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
    }

    public static void StartNewDay()
    {
        Day++;
        int dayoftheweek = (int)DayOfWeek;
        dayoftheweek++;
        if (dayoftheweek == 7)
        {
            dayoftheweek = 0;
        }
        DayOfWeek = (DaysOfWeek)dayoftheweek;
        if (Month == 2 && Day == 29 && !IsLeapYear(Year))
        {
            Month++;
            Day = 1;
        }
        else if (Month == 2 && Day == 30 && IsLeapYear(Year))
        {
            Month++;
            Day = 1;
        }
        else if (Month == 1 || Month == 3 || Month == 5 || Month == 7 || Month == 8 || Month == 10)
        {
            if (Day == 32)
            {
                Month++;
                Day = 1;
            }
        }
        else if (Month == 4 || Month == 6 || Month == 9 || Month == 11)
        {
            if (Day == 31)
            {
                Month++;
                Day = 1;
            }
        }
        else if (Month == 12)
        {
            if (Day == 32)
            {
                Month = 1;
                Day = 1;
                Year++;
            }
        }
    }

    public static void SaveDate()
    {
        File.Delete("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/Dates.txt");
        StreamWriter sw = new StreamWriter("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/Dates.txt");
        sw.WriteLine(ToDateString());
        sw.WriteLine(DayOfWeek.ToString());
        sw.Close();
    }

    public static void SaveDateFirstTime()
    {
        StreamWriter sw = new StreamWriter("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/Dates.txt");
        sw.WriteLine(ToDateString());
        sw.WriteLine(DayOfWeek.ToString());
        sw.Close();
    }
    
    public static void FromStringDate()
    {
        StreamReader sr =
               new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/Dates.txt");
        string dateString = sr.ReadLine();
        string[] dateArray = dateString.Split('.');
        Day = int.Parse(dateArray[0]);
        Month = int.Parse(dateArray[1]);
        Year = int.Parse(dateArray[2]);
        DayOfWeek = (DaysOfWeek)Enum.Parse(typeof(DaysOfWeek), sr.ReadLine());
        sr.Close();
    }

    public static string ToDateString()
    {
        string date = "";
        if (Day < 10 && Month > 10)
        { 
            date = "0" + Day.ToString() + "." + Month.ToString() + "." + Year.ToString();
        }
        else if (Day < 10 && Month < 10)
        {
            date = "0" + Day.ToString() + "." + "0" + Month.ToString() + "." + Year.ToString();
        }
        else if (Day > 10 && Month < 10)
        {
            date = Day.ToString() + "." + "0" + Month.ToString() + "." + Year.ToString();
        }
        else
        {
            date = Day.ToString() + "." + Month.ToString() + "." + Year.ToString();
        }
        return date;
    }
}