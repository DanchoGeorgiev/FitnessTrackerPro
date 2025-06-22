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
            Calories Consumed
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
        string[,] result = TrackerRun.CreateSetPrevSessMatrix("21.06.2025", "Bench Press");

        for (int i = 0; i < result.GetLength(0); i++)
        {
            Console.WriteLine($"Set: {result[i,0]}, Reps: {result[i,1]}, KG: {result[i,2]}");
        }

        string test = TrackerRun.GetExercisePosition("Chest Dip");
        Console.WriteLine(test);
    }
}