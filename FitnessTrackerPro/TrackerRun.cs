using System.Text;
using System.Text.RegularExpressions;

namespace FitnessTrackerPro;

public static class TrackerRun
{
   static StreamReader exercisesReader = new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/Exercises.txt", Encoding.UTF8);

   private static StreamReader prevProgressReader =
       new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/PreviousProgress.txt",
           Encoding.UTF8);
   static StreamReader weeklyRoutineReader = new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/WeeklyRoutine.txt", Encoding.UTF8);
   
   public static string GetExercise(string category, string position)
   {
       string line;
       var sb = new StringBuilder();
       bool isCategory = false;
       bool foundExercise = false;
       while ((line = exercisesReader.ReadLine()) != null)
       {
           if (line.StartsWith(category + ":"))
           {
               isCategory = true;
               sb.AppendLine(line);
               continue;
           }

           if (isCategory)
           {
               if (line.StartsWith(position + "."))
               {
                   foundExercise = true;
                   sb.AppendLine(line);
                   continue;
               }
           }

           if (foundExercise)
           {
               if (line.StartsWith("-"))
               {
                   while (line.StartsWith("-"))
                   {
                       sb.AppendLine(line);
                       line = exercisesReader.ReadLine();
                   }
                   break;
               }
           }
       }
       return sb.ToString();
   }

   public static string GetExercise1(string category, string position)
   {
       string line;
       string exercise = "";
       bool isCategory = false;
       while ((line = exercisesReader.ReadLine()) != null)
       {
           if (line.StartsWith(category + ":"))
           {
               isCategory = true;
               continue;
           }

           if (isCategory)
           {
               if (line.StartsWith(position + "."))
               {
                   exercise = line;
                   break;
               }
           }
       }
       return exercise;
   }

   public static string GetExercisePosition(string exerciseName)
   {
       string line;
       string position = "";
       Regex regex = new Regex(@"^(1[0-9]|20|[1-9])\.");

       while ((line = exercisesReader.ReadLine()) != null)
       {
           if (line.Length > 2)
           {
               if (regex.IsMatch(line.Substring(0, 2)) || regex.IsMatch(line.Substring(0, 3)))
               {
                   if (line.IndexOf(" ") == 2 && exerciseName == line.Substring(3))
                   {
                       position = line.Substring(0, 1);
                   }
                   else if (line.IndexOf(" ") == 3 && exerciseName == line.Substring(4))
                   {
                       position = line.Substring(0, 2);
                   }
               }
           }
       }
       return position;
   }

   public static string[,] CreateSetPrevSessMatrix(string date, string exerciseName) //Създава матрица от резултатите от предишната сесия
   {
       string line;
       bool isExercise = false;
       bool inRange = false;
       Regex dateRegex = new Regex(@"^\d{2}\.\d{2}\.\d{4}$");
       List<string[]> preMatrix = new List<string[]>();
       int matrixRows = 0;
       string notes;

       while ((line = prevProgressReader.ReadLine()) != null)
       {
           if (dateRegex.IsMatch(line))
           {
               if (line == date)
               {
                   inRange = true;
               }
               else if (inRange)
               {
                   break;
               }
           }
           else if (inRange)
           {
               if (line.StartsWith("-" + exerciseName))
               {
                   while ((line = prevProgressReader.ReadLine()) != null)
                   {
                       if (line.StartsWith("&"))
                       {
                           matrixRows++;
                           string[] temp = line.Substring(1).Split('@');
                           preMatrix.Add(temp);
                       }
                       else
                       {
                           break;
                       }
                   }
               }
           }
       }
       string[,] matrix = new string[matrixRows, 3];
       for (int row = 0; row < matrixRows; row++)
       {
           matrix[row, 0] = preMatrix[row][0];
           matrix[row, 1] = preMatrix[row][1];
           matrix[row, 2] = preMatrix[row][2];
       }
       
       return matrix;
   }

   public static void CreateWorkout(string day, string exerciseName, string set, string reps) //Създава първото упражнение в даден ден от седмичната рутина
   {
       List<string > temp = new List<string>(File.ReadAllLines("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/WeeklyRoutine.txt", Encoding.UTF8));
       File.Delete("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/WeeklyRoutine.txt");
       StreamWriter writer = new StreamWriter("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/WeeklyRoutine.txt", true, Encoding.UTF8);
       for (int i = 0; i < temp.Count; i++)
       {
           if (temp[i] == day)
           {
               temp.Insert(i + 1, "-" + exerciseName);
               temp.Insert(i + 2, "&" + set + "@" + reps);
               break;
           }
       }

       foreach (string s in temp)
       {
           writer.WriteLine(s);
       }
       writer.Close();
   }

   public static void AddExerciseToWorkout(string day, string exerciseName) //Добавя упражнение към седмичната рутина
   {
       List<string > temp = new List<string>(File.ReadAllLines("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/WeeklyRoutine.txt", Encoding.UTF8));
       File.Delete("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/WeeklyRoutine.txt");
       StreamWriter writer = new StreamWriter("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/WeeklyRoutine.txt", true, Encoding.UTF8);
       bool isDay = false;
       for (int i = 0; i < temp.Count; i++)
       {
           if (temp[i] == day)
           {
               isDay = true;
           }

           if (isDay && !temp[i].StartsWith("&") && !temp[i].StartsWith("-") && temp[i] != day)
           {
               temp.Insert(i, "-" + exerciseName);
               break;
           }

           if (isDay && temp[i] == temp[temp.Count - 1])
           {
               temp.Insert(i + 1, "-" + exerciseName);
               break;
           }
       }
       foreach (string s in temp)
       {
           writer.WriteLine(s);
       }
       writer.Close();
   }

   public static void AddRepsToExercise(string day, string exerciseName, string set, string reps) //Добавя сетове и повторения
   {
       List<string > temp = new List<string>(File.ReadAllLines("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/WeeklyRoutine.txt", Encoding.UTF8));
       File.Delete("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/WeeklyRoutine.txt");
       StreamWriter writer = new StreamWriter("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/WeeklyRoutine.txt", true, Encoding.UTF8);
       bool isDay = false;
       bool isExercise = false;
       for (int i = 0; i < temp.Count; i++)
       {
           if (temp[i] == day)
           {
               isDay = true;
           }

           if (isDay && "-" + exerciseName == temp[i])
           {
               isExercise = true;
           }

           if (isDay && isExercise && temp[i] != temp[temp.Count - 1] && !temp[i + 1].StartsWith("&") && temp[i].StartsWith("-"))
           {
               temp.Insert(i + 1, "&" + set + "@" + reps);
               break;
           }

           if (isDay && isExercise && temp[i] != temp[temp.Count - 1] && temp[i].StartsWith("&") && temp[i + 1].StartsWith("-"))
           {
               temp.Insert(i + 1, "&" + set + "@" + reps);
               break;
           }

           if (isDay && isExercise && temp[i] != temp[temp.Count - 1] && !temp[i + 1].StartsWith("&") && !temp[i + 1].StartsWith("-"))
           {
               temp.Insert(i + 1, "&" + set + "@" + reps);
               break;
           }
           
           if (isDay && isExercise && temp[i].StartsWith("-") && temp[i] == temp[temp.Count - 1])
           {
               temp.Add("&" + set + "@" + reps);
               break;
           }
           
           if (isDay && isExercise && temp[i].StartsWith("&") && temp[i] == temp[temp.Count - 1])
           {
               temp.Add("&" + set + "@" + reps);
               break;
           }
       }
       foreach (string s in temp)
       {
           writer.WriteLine(s);
       }
       writer.Close();
   }
   
   
   
   public static void SetExerciseinTodaysWorkout(string date, string exerciseName, string exerciseExplanation,
       string prevSessSet, string prevSessReps, string previousSessionKGs, string NotesFromPreviousSession,
       string TodaysSessSet, string TodaysSessReps, string TodaysSessKGs, string TodaysSessNotes)
   {
       string line;
   }
}