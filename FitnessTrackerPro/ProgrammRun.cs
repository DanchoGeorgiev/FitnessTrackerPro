using System.Text;
using System.Text.RegularExpressions;
using FuzzySharp;

namespace FitnessTrackerPro;

public static class ProgramRun
{
    //User methods
    public static bool IsUser()
    {
        List<string> data = File.ReadAllLines("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/UserData.txt", Encoding.UTF8).ToList();
        if (data.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool ValidateUser()
    {
        List<string> data = File.ReadAllLines("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/UserData.txt", Encoding.UTF8).ToList();
        string username;
        string password;
        while (true)
        {
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Log in to your account");
            Console.Write("Username: ");
            username = Console.ReadLine();
            Console.Write("Password: ");
            password = Console.ReadLine();
            if (username == data[0] && password == data[2])
            {
                Console.WriteLine("You are logged in!");
                return true;
            }
            else
            {
                Console.WriteLine("Wrong username or password!");
            }
        }
    }

    public static string ValidateEmail()
    {
        string inputEmail;

        while (true)
        {
            Console.Write("Enter your email: ");
            inputEmail = Console.ReadLine();

            if (IsValidEmail(inputEmail))
            {
                Console.WriteLine("Valid email address.");
                break;
            }
            else
            {
                Console.WriteLine("Invalid email. Please try again.\n");
                IConsolePressEnter();
            }
        }
        return inputEmail;
    }
    static bool IsValidEmail(string email)
    {
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }
    
    //Set date method

    public static void SetDate()
    {
        Console.WriteLine("Please enter a year: ");
        int year = CheckNumberChoice();
        
        Console.WriteLine("Please enter a month: ");
        int inputMonth;
        while (true)
        {
            inputMonth = CheckNumberChoice();   
            if (inputMonth < 1 || inputMonth > 12)
            {
                Console.WriteLine("Please enter a valid month.");
                IConsolePressEnter();
            }
            else
            {
                break;
            }
        }

        int date;
        while (true)
        {
            Console.WriteLine("Enter your date: ");
            date = CheckNumberChoice();
            if (inputMonth == 1 || inputMonth == 3 || inputMonth == 5 || inputMonth == 7 || inputMonth == 8 ||
                inputMonth == 10 || inputMonth == 12)
            {
                if (date < 1 || date > 31)
                {
                    Console.WriteLine("Please enter a valid date.");
                    IConsolePressEnter();
                }
                else
                {
                    break;
                }
            }
            else if (inputMonth == 2 && Dates.IsLeapYear(year))
            {
                if (date < 1 || date > 29)
                {
                    Console.WriteLine("Please enter a valid date.");
                    IConsolePressEnter();
                }
                else
                {
                    break;
                }
            }
            else if (inputMonth == 2 && !Dates.IsLeapYear(year))
            {
                if (date < 1 || date > 28)
                {
                    Console.WriteLine("Please enter a valid date.");
                    IConsolePressEnter();
                }
                else
                {
                    break;
                }
            }
            if (inputMonth == 4 || inputMonth == 6 || inputMonth == 9 || inputMonth == 11)
            {
                if (date < 1 || date > 30)
                {
                    Console.WriteLine("Please enter a valid date.");
                    IConsolePressEnter();
                }
                else
                {
                    break;
                }
            }
        }

        string day = CheckIfValidDay();
        
        Dates.SetDate(day, date, inputMonth, year);
    }
    //Create First Workout Method

    public static void FirstWorkout()
    {
        Console.WriteLine("=====================================================================");
        Console.WriteLine("Let's create your first workout!");
        string day = CheckIfValidDay();
        List<string> possibleChoices = ListExercisesFromSearch();
        int exercise = CheckNumberChoice() - 1;
        string exerciseName = possibleChoices[exercise];
        string set = TrackerRun.CreateSetName(day, exerciseName);
        Console.WriteLine("=====================================================================");
        Console.WriteLine("How many reps you would you like to add to your first set?");
        int reps = CheckNumberChoice();
        string rep1 = Convert.ToString(reps);
        TrackerRun.CreateWorkout(day, exerciseName, set, rep1);
        AddSetsToExerciseFirstime(day, exerciseName);
        AddExerciseToWorkoutFirstTime(day);
    }
    //Add exercises or add to exercises

    public static void AddSetsToExercise(string day, string exerciseName)
    {
        Console.WriteLine("=====================================================================");
        Console.WriteLine("How many reps you would you like to add to the set?");
        int reps = CheckNumberChoice();
        string rep1 = Convert.ToString(reps);
        string set = TrackerRun.CreateSetName(day, exerciseName);
        TrackerRun.AddRepsToExerciseToWeeklyRoutine(day, exerciseName, set, rep1);
        int choice;
        while (true)
        {
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Would you like to add more sets to this exercise?");
            Console.WriteLine("1.Yes");
            Console.WriteLine("2.No");
            choice = CheckYesorNo();
            if (choice == 1)
            {
                AddSetsToExercise(day, exerciseName);
                break;
            }
            else if (choice == 2)
            {
                break;
            }
        }
    }

    public static void AddSetsToExerciseFirstime(string day, string exerciseName)
    {
        int choice;
        while (true)
        {
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Would you like to add more sets to this exercise?");
            Console.WriteLine("1.Yes");
            Console.WriteLine("2.No");
            choice = CheckYesorNo();
            if (choice == 1)
            {
                AddSetsToExercise(day, exerciseName);
                break;
            }
            else if (choice == 2)
            {
                break;
            }
        }
    }
    
    public static void AddExerciseToWorkout()
    {
        string day = CheckIfValidDay();
        List<string> possibleChoices = ListExercisesFromSearch();
        int exercise = CheckNumberChoice() - 1;
        string exerciseName = possibleChoices[exercise];
        string set = TrackerRun.CreateSetName(day, exerciseName);
        TrackerRun.AddExerciseToWorkoutToWeeklyRoutine(day, exerciseName);
        AddSetsToExercise(day, exerciseName);
        
        int choice1;
        while (true)
        {
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Would you like to add more exercises?");
            Console.WriteLine("1.Yes");
            Console.WriteLine("2.No");
            choice1 = CheckYesorNo();
            if (choice1 == 1)
            {
                AddExerciseToWorkout();
                break;

            }
            else if (choice1 == 2)
            {
                break;
            }
        }
    }
    
    public static void AddExerciseToWorkout1(string day)
    {
        List<string> possibleChoices = ListExercisesFromSearch();
        int exercise = CheckNumberChoice() - 1;
        string exerciseName = possibleChoices[exercise];
        string set = TrackerRun.CreateSetName(day, exerciseName);
        TrackerRun.AddExerciseToWorkoutToWeeklyRoutine(day, exerciseName);
        AddSetsToExercise(day, exerciseName);
        
        int choice1;
        while (true)
        {
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Would you like to add more exercises?");
            Console.WriteLine("1.Yes");
            Console.WriteLine("2.No");
            choice1 = CheckYesorNo();
            if (choice1 == 1)
            {
                AddExerciseToWorkout1(day);
                break;

            }
            else if (choice1 == 2)
            {
                break;
            }
        }
    }
    
    public static void AddExerciseToWorkoutFirstTime(string day)
    {
        int choice1;
        while (true)
        {
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Would you like to add more exercises?");
            Console.WriteLine("1.Yes");
            Console.WriteLine("2.No");
            choice1 = CheckYesorNo();
            if (choice1 == 1)
            {
                List <string> possibleChoices = ListExercisesFromSearch();
                int exercise = CheckNumberChoice() - 1;
                string exerciseName = possibleChoices[exercise];
                string set = TrackerRun.CreateSetName(day, exerciseName);
                TrackerRun.AddExerciseToWorkoutToWeeklyRoutine(day, exerciseName);
                AddSetsToExercise(day, exerciseName);
            }
            else if (choice1 == 2)
            {
                break;
            }
        }
    }
    
    //Check num method

    public static int CheckNumberChoice()
    {
        int inputNumber;
        while (true)
        {
            Console.Write("Enter your choice: ");
            if (int.TryParse(Console.ReadLine(), out inputNumber))
            {
                break;
            }
            else
            {
                Console.WriteLine("Please enter only numbers");
                IConsolePressEnter();
            }
        }
        return inputNumber;
    }
    //Check yes no method

    public static int CheckYesorNo()
    {
        int choice;
        while (true)
        {
            choice = CheckNumberChoice();
            if (choice == 1 || choice == 2)
            {
                break;
            }
            else
            {
                Console.WriteLine("Please enter a valid choice");
            }
        }
        return choice;
    }
    
    //Check day method

    public static string CheckIfValidDay()
    {
        string day;
        while (true)
        {
            Console.Write("Enter a day of the week: ");
            day = Console.ReadLine();
            if (day == "Monday" || day == "Tuesday" || day == "Wednesday" || day == "Thursday" || day == "Friday" ||
                day == "Saturday" || day == "Sunday")
            {
                break;
            }
            else
            {
                Console.WriteLine("Please enter a valid day.");
                IConsolePressEnter();
            }
        }

        return day;
    }
    //Search day method

    
    
    //List exercises closest to match

    public static List<string> ListExercisesFromSearch()
    {
        Console.WriteLine("=====================================================================");
        Console.WriteLine("Search for the exercise you would like to add:");
        string search;
        List<string> results = new List<string>();
        while (true)
        {
            search = Console.ReadLine();
            results = TrackerRun.SearchForExerciseList(search);
            if (results.Count == 0)
            {
                Console.WriteLine("No exercises found.");

            }
            else
            {
                Console.WriteLine("Exercises closest to your search:");
                int i = 1;
                foreach (string exercise in results)
                {
                    Console.WriteLine(i.ToString() + ". " + exercise);
                    i++;
                }

                break;
            }
        }
        return results;
    }


    //Console Methods
    
    public static void IConsolePressEnter()
    {
        Console.WriteLine("Press enter to continue...");
        Console.ReadKey();
    }
    
    //Main menu

    public static void MainMenu()
    {
        Console.WriteLine("=====================================================================");
        Console.WriteLine($"Today is {Dates.DayOfWeek.ToString()} the {Dates.ToDateString()}");
        Console.WriteLine("1.Start a new day");
        Console.WriteLine("2.View Current Day");
        Console.WriteLine("3.Weekly Plan");
        Console.WriteLine("4.Exercises");
        Console.WriteLine("5.Exit the program");
        Console.WriteLine("=====================================================================");
    }

    public static void ViewCurrentDayMenu()
    {
        Console.WriteLine("=====================================================================");
        TrackerRun.PrintTodaysWorkout();
        Console.WriteLine("=====================================================================");
        Console.WriteLine("Would you like to begin today's workout?");
        Console.WriteLine("1.Yes");
        Console.WriteLine("2.Go back");
    }

    public static void WeeklyPlanMenu()
    {
        Console.WriteLine("================================================================");
        Console.WriteLine("1.View workout for a particular day");
        Console.WriteLine("2.Edit workout for a particular day");
        Console.WriteLine("3.Go back");
        Console.WriteLine("================================================================");

    }
    
}