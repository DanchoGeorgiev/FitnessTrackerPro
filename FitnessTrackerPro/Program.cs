using System.Diagnostics;
using System.Text;

namespace FitnessTrackerPro;

class Program
{
    static void Main(string[] args)
    {
        /*
         Set CurrentDay
         
         Start a new day
         View Current Day
            Today's Workout
              Start Workout
                Your Previous Session In KGs
                Progress For Today In KGs   
         Weekly Plan
            Monday - Sunday
            Exercises
            Your Previous Session
         Exercises
            Search For An Exercise
                Exercise How To
         */
        /*StreamReader reader = new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/Exercises.txt", Encoding.UTF8);
        string test;
        var exercise = new StringBuilder(); 
        
        bool inCategory = false;
        bool foundExecise = false;
        while ((test = reader.ReadLine()) != null)
        {
            if (test == "Back" + ":")
            {
                inCategory = true;
                exercise.AppendLine(test);
                continue;
            }

            if (inCategory)
            {
                if (test.StartsWith("3."))
                {
                    exercise.AppendLine(test);
                    foundExecise = true;
                }
            }

            if (foundExecise)
            {
                if (test.StartsWith("-"))
                {
                    while ((test.StartsWith("-")))
                    {
                        exercise.AppendLine(test);
                        test = reader.ReadLine();
                    }
                    break;
                }
            }
        }
        
        Console.WriteLine(exercise);*/

        //Console.WriteLine(TrackerRun.GetExercise("Chest", "10"));
        //TrackerRun.CreateWorkout("Monday", "Pull-Up", "set1", "8");
        //TrackerRun.AddRepsToExerciseToWeeklyRoutine("Monday", "Pull-Up", "set2", "8");
        //TrackerRun.AddExerciseToWorkoutToWeeklyRoutine("Monday", "Deadlift");
        /*string test = TrackerRun.GetExercisePosition("Chest Dip");
        Console.WriteLine(test);
        Dates.SetDate("Sunday", 22, 6, 2025);
        Dates.StartNewDay();
        
       TrackerRun.SetExerciseInTodaysWorkout();
       TrackerRun.AddKGsToExerciseToTodaysWorkout("Pull-Up", "set1", "200");
       TrackerRun.DeleteAnExerciseFrowWeeklyRoutine("Sunday", "Deadlift");
       List<string> test1 = TrackerRun.SearchForExerciseList("dip");
       foreach (string s in test1)
       {
           Console.WriteLine(s);
       }
       Console.WriteLine(TrackerRun.CheckIfHadDonePreviousProgress("Tuesday"));*/

        bool isFirstTime = false; //Трябва да е false!!!
        bool testMode = false;
        if (ProgramRun.IsUser())
        {
            Console.WriteLine("=====================================================================");
            Console.WriteLine("Hello User, what is your name:");
            string userName = Console.ReadLine();
            string email = ProgramRun.ValidateEmail();
            Console.WriteLine("PLease enter a password:");
            string password = Console.ReadLine();
            User user = new User(userName, email, password);
            user.SaveUser();
            isFirstTime = true;
        }

        if (!ProgramRun.IsUser())
        {
            if (!testMode)
            {
                if (ProgramRun.ValidateUser())
                {
                    if (isFirstTime)
                    {
                        ProgramRun.SetDate();
                        ProgramRun.FirstWorkout();
                        Dates.SaveDateFirstTime();
                        isFirstTime = false;
                    }

                    if (!isFirstTime)
                    {
                        Dates.FromStringDate();
                        bool hadTrained = false;
                        while (true)
                        {
                            ProgramRun.MainMenu();
                            int choice = ProgramRun.CheckNumberChoice();
                            switch (choice)
                            {
                                case 1:
                                    Dates.StartNewDay();
                                    TrackerRun.SetExerciseInTodaysWorkout();
                                    break;
                                case 2:
                                    ProgramRun.ViewCurrentDayMenu();
                                    int choice2 = ProgramRun.CheckNumberChoice();
                                    if (choice2 == 1)
                                    {
                                        TrackerRun.RunWorkout();
                                    }

                                    break;
                                case 3:
                                    ProgramRun.WeeklyPlanMenu();
                                    int choice3 = ProgramRun.CheckNumberChoice();
                                    switch (choice3)
                                    {
                                        case 1:
                                            Console.WriteLine(
                                                "Enter the day, you would like to see the training plan: ");
                                            string day = ProgramRun.CheckIfValidDay();
                                            if (TrackerRun.PrintWeeklyWorkout(day))
                                            {
                                                Console.WriteLine(
                                                    "=====================================================================");
                                                Console.WriteLine("Would you like to edit the training plan?");
                                                Console.WriteLine("1.Yes");
                                                Console.WriteLine("2.No");
                                                int choice4 = ProgramRun.CheckNumberChoice();
                                                switch (choice4)
                                                {
                                                    case 1:
                                                        choice3 = 2;
                                                        break;
                                                    case 2:
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine(
                                                    "=====================================================================");
                                                Console.WriteLine("Day is empty");
                                                Console.WriteLine("1.Add Exercises");
                                                Console.WriteLine("2.Make it a rest day");
                                                Console.WriteLine("3.Go Back");
                                                int choice5 = ProgramRun.CheckNumberChoice();
                                                switch (choice5)
                                                {
                                                    case 1:
                                                        ProgramRun.AddExerciseToWorkout1(day);
                                                        break;
                                                    case 2:
                                                        TrackerRun.MarkDayAsRestDay(day);
                                                        break;
                                                    case 3:
                                                        break;
                                                }
                                            }

                                            break;
                                        case 2:
                                            Console.WriteLine("Comming Soon ;)");
                                            break;
                                        case 3:
                                            break;
                                    }

                                    break;
                                case 5:
                                    Dates.SaveDate();
                                    break;
                            }

                            if (choice == 5)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                TrackerRun.DeleteAllExercises("Monday");
            }
        }
    }
}