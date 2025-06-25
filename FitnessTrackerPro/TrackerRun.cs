using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using FuzzySharp;
using Microsoft.VisualBasic.FileIO;

namespace FitnessTrackerPro;

public static class TrackerRun
{
    //Get and Write Functions
    public static string CheckDates(string day)
    {
        StreamReader reader =
            new StreamReader(
                "/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/PreviousProgress.txt",
                Encoding.UTF8);
        string line;
        List<string> dates = new List<string>();
        Regex regex = new Regex(@"^\d{2}\.\d{2}\.\d{4}$");
        while ((line = reader.ReadLine()) != null)
        {
            if (line.Length > 14 && regex.IsMatch(line.Substring(0, 10)) && day == line.Substring(13))
            {
                dates.Add(line.Substring(0, 10));
            }
        }
        
        List<DateTime> parsedDates = dates
            .Select(d => DateTime.ParseExact(d, "dd.MM.yyyy", null))
            .ToList();
        DateTime latestDate = parsedDates.Max();
        reader.Close();
        return latestDate.ToString("dd.MM.yyyy");
    }
    
   public static string GetExercise(string category, string position)
   {
       string line;
       var sb = new StringBuilder();
       StreamReader exercisesReader = new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/Exercises.txt", Encoding.UTF8);
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
       exercisesReader.Close();
       return sb.ToString(); 
   }

   public static string GetExerciseName(string category, string position)
   {
       string line;
       string exercise = "";
       bool isCategory = false;
       StreamReader exercisesReader = new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/Exercises.txt", Encoding.UTF8);
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
       exercisesReader.Close();
       return exercise;
   }

   public static List<string> GetExerciseExplanation(string exerciseName)
   {
       string line;
       string position = GetExercisePosition(exerciseName);
       List <string> explanation = new List<string>();
       StreamReader exercisesReader = new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/Exercises.txt", Encoding.UTF8);
       bool foundExercise = false;
       while ((line = exercisesReader.ReadLine()) != null)
       {
           if (line.IndexOf(" ") == 3 && line.Substring(line.IndexOf(" ") + 1) == exerciseName)
           {
               foundExercise = true;
           }
           else if (line.IndexOf(" ") == 2 && line.Substring(line.IndexOf(" ") + 1) == exerciseName)
           {
               foundExercise = true;
           }

           if (foundExercise)
           {
               if (line.StartsWith("-"))
               {
                   while (line.StartsWith("-"))
                   {
                       explanation.Add("?" + line.Substring(2));
                       line = exercisesReader.ReadLine();
                   }
                   break;
               }
           }
       }
       exercisesReader.Close();
       return explanation;
   }
   
   public static void PrintExerciseExplanation(string exerciseName)
   {
       string line;
       
       StreamReader exercisesReader = new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/Exercises.txt", Encoding.UTF8);
       bool foundExercise = false;
       while ((line = exercisesReader.ReadLine()) != null)
       {
           if (line.IndexOf(" ") == 3 && line.Substring(line.IndexOf(" ") + 1) == exerciseName)
           {
               foundExercise = true;
           }
           else if (line.IndexOf(" ") == 2 && line.Substring(line.IndexOf(" ") + 1) == exerciseName)
           {
               foundExercise = true;
           }

           if (foundExercise)
           {
               if (line.StartsWith("-"))
               {
                   while (line.StartsWith("-"))
                   {
                       Console.WriteLine(line = exercisesReader.ReadLine());
                   }
                   break;
               }
           }
       }
       exercisesReader.Close();
   }

   public static string GetExercisePosition(string exerciseName)
   {
       string line;
       string position = "";
       Regex regex = new Regex(@"^(1[0-9]|20|[1-9])\.");
       StreamReader exercisesReader = new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/Exercises.txt", Encoding.UTF8);

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
       exercisesReader.Close();
       return position;
   }
   /*
   public static string[,] CreateSetPrevSessMatrix(string date, string exerciseName) //Създава матрица от резултатите от предишната сесия
   {
       string line;
       bool isExercise = false;
       bool inRange = false;
       Regex dateRegex = new Regex(@"^\d{2}\.\d{2}\.\d{4}$");
       List<string[]> preMatrix = new List<string[]>();
       int matrixRows = 0;
       StreamReader prevProgressReader = new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/PreviousProgress.txt", Encoding.UTF8);
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
       prevProgressReader.Close();
       return matrix;
   }
   */
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

   public static void AddExerciseToWorkoutToWeeklyRoutine(string day, string exerciseName) //Добавя упражнение към седмичната рутина
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

   public static void AddRepsToExerciseToWeeklyRoutine(string day, string exerciseName, string set, string reps) //Добавя сетове и повторения
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

   public static List<string> GetExercisesFromWeeklyRoutine(string day) //TODO
   {
       List<string> temp = new List<string>();
       return temp;
   }

   public static List<string> GetExercisesFromPreviousProgress()
   {
       List<string> temp = new List<string>();
       string line;
       string day = Dates.DayOfWeek.ToString();
       string date = CheckDates(day);
       bool inRange = false;
       Regex dateRegex = new Regex(@"^\d{2}\.\d{2}\.\d{4}$");
       StreamReader prevProgressReader = new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/PreviousProgress.txt", Encoding.UTF8);
       while ((line = prevProgressReader.ReadLine()) != null)
       {
           if (line.Length > 14 && dateRegex.IsMatch(line.Substring(0, 10)))
           {
               if (line.Substring(0, 10) == date)
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
               temp.Add(line);
           }
       }
       prevProgressReader.Close();
       return temp;
   }

   public static List<string> ExtractExerciseFromPreviousProgress(string exerciseName)
   {
       List<string> temp = GetExercisesFromPreviousProgress();
       List<string> result = new List<string>();
       bool inRange = false;

       foreach (string s in temp)
       {
           if (s.StartsWith("-"))
           {
               if (exerciseName == s.Substring(1))
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
               result.Add(s);
           }
       }
       return result;
   }
   
   public static void SetExerciseInTodaysWorkout() //TODO
   {
       List <string> temp = new List<string>();
       string date = Dates.ToDateString();
       temp.Add(date);
       string day = Dates.DayOfWeek.ToString();
       temp.Add(day);

       if (!IsRestDay(day))
       {
           StreamReader weeklyRoutineReader =
               new StreamReader(
                   "/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/WeeklyRoutine.txt",
                   Encoding.UTF8);
           string line;
           bool inRange = false;
           Regex regexDay = new Regex("^(Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday)$");
           while ((line = weeklyRoutineReader.ReadLine()) != null)
           {
               if (regexDay.IsMatch(line))
               {
                   if (day == line)
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
                   if (line.StartsWith("&"))
                   {
                       temp.Add("*" + line.Substring(1));
                   }

                   if (line.StartsWith("-"))
                   {
                       temp.Add(line);
                       List<string> explanations = GetExerciseExplanation(line.Substring(1));
                       foreach (string explanation in explanations)
                       {
                           temp.Add(explanation);
                       }

                       List<string> previousExercises = ExtractExerciseFromPreviousProgress(line.Substring(1));
                       foreach (string prevData in previousExercises)
                       {
                           temp.Add(prevData);
                       }
                   }
               }
           }


           File.Delete("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/TodaysWorkout.txt");
           StreamWriter writer =
               new StreamWriter(
                   "/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/TodaysWorkout.txt", true,
                   Encoding.UTF8);
           foreach (string s in temp)
           {
               writer.WriteLine(s);
           }

           writer.Close();
           weeklyRoutineReader.Close();
       }
       else
       {
           File.Delete("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/TodaysWorkout.txt");
           StreamWriter writer =
               new StreamWriter(
                   "/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/TodaysWorkout.txt", true,
                   Encoding.UTF8);
           writer.WriteLine("+Restday");
           writer.Close();
       }
   }

   public static void AddKGsToExerciseToTodaysWorkout(string exerciseName, string set, string kgs) //TODO
   {
       List<string> temp = new List<string>(File.ReadAllLines("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/TodaysWorkout.txt", Encoding.UTF8));
       File.Delete("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/TodaysWorkout.txt");
       bool inRange = false;
       for (int i = 0; i < temp.Count; i++)
       {
           if (temp[i].StartsWith("-"))
           {
               if ("-" + exerciseName == temp[i])
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
               if (temp[i].StartsWith("*" + set))
               {
                   temp[i] = temp[i] + "@" + kgs;
               }
           }
       }

       StreamWriter writer =
           new StreamWriter("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/TodaysWorkout.txt",
               false, Encoding.UTF8);
       foreach (string s in temp)
       {
           writer.WriteLine(s);
       }
       writer.Close();
   }

   public static void ExtportTodaysWorkoutToPreviousProgress() //TODO
   {
       List<string> temp = new List<string>(File.ReadAllLines("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/TodaysWorkout.txt", Encoding.UTF8));
       List<string> toExport = new List<string>();
       toExport.Add(temp[0] + " - " + temp[1]);

       for (int i = 0; i < temp.Count; i++)
       {
           if (temp[i].StartsWith("-"))
           {
               toExport.Add(temp[i]);
           }
           else if (temp[i].StartsWith("*"))
           {
               toExport.Add("&" + temp[i].Substring(1));
           }
       }
       
       StreamWriter writer = new StreamWriter("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/PreviousProgress.txt", true, Encoding.UTF8);
       writer.WriteLine();
       foreach (string s in toExport)
       {
           writer.WriteLine(s);
       }
       writer.Close();
   }

   public static void DeleteAnExerciseFrowWeeklyRoutine(string day, string exerciseName)
   {
       List<string> temp = new List<string>(File.ReadAllLines("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/WeeklyRoutine.txt", Encoding.UTF8));
       File.Delete("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/WeeklyRoutine.txt");
       bool isDay = false;
       bool inRange = false;
       Regex regexDay = new Regex("^(Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday)$");
       for (int i = 0; i < temp.Count; i++)
       {
           if (temp[i] == day)
           {
               isDay = true;
           }

           if (isDay)
           {
               if (temp[i].StartsWith("-") || regexDay.IsMatch(temp[i]))
               {
                   if ("-" + exerciseName == temp[i])
                   {
                       inRange = true;
                       temp.RemoveAt(i);
                   }
                   else if (inRange)
                   {
                       break;
                   }
               }
               else if (inRange)
               {
                   temp.Remove(temp[i]);
               }
           }
       }
       StreamWriter writer = new StreamWriter("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/WeeklyRoutine.txt", false, Encoding.UTF8);
       foreach (string s in temp)
       {
           writer.WriteLine(s);
       }
       writer.Close();
   }

   public static string SearchForExercise(string exerciseName)
   {
       StreamReader exercisesReader =
           new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/Exercises.txt",
               Encoding.UTF8);

       Regex exerciseRegex = new Regex(@"^\d+\.\s");
       string line;
       List<string> temp = new List<string>();
       while ((line = exercisesReader.ReadLine()) != null)
       {
           if (exerciseRegex.IsMatch(line))
           {
               temp.Add(line.Substring(line.IndexOf(" ") + 1));
           }
       }

       var bestMatch = Process.ExtractOne(exerciseName, temp);
       exercisesReader.Close();
       if (bestMatch != null && bestMatch.Score > 60)
           return bestMatch.Value;

       return "No close match found.";
   }
   
   public static List<string> SearchForExerciseList(string exerciseName)
   {
       StreamReader exercisesReader =
           new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/Exercises.txt",
               Encoding.UTF8);

       Regex exerciseRegex = new Regex(@"^\d+\.\s");
       string line;
       List<string> temp = new List<string>();
       List<string> exercises = new List<string>();
       while ((line = exercisesReader.ReadLine()) != null)
       {
           if (exerciseRegex.IsMatch(line))
           {
               temp.Add(line.Substring(line.IndexOf(" ") + 1));
           }
       }

       foreach (var exercise in temp)
       {
           int score = Fuzz.PartialRatio(exerciseName.ToLower(), exercise.ToLower());

           if (score > 66)
           {
               exercises.Add(exercise);
           }
       }
       
       return exercises;
   }

   public static string CreateSetName(string day, string exerciseName)
   {
       StreamReader weeklyRoutineReader =
           new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/WeeklyRoutine.txt",
               Encoding.UTF8);
       string line;
       int setNumber = 0;
       bool isCorrectDay = false;
       bool isCorrectExercise = false;
       Regex regexDay = new Regex("^(Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday)$");
       

       while ((line = weeklyRoutineReader.ReadLine()) != null)
       {
           if (regexDay.IsMatch(line))
           {
               if (line == day)
               {
                   isCorrectDay = true;
               }
               else if (isCorrectDay)
               { 
                   break;
               }
           }
           else if (isCorrectDay)
           {
               if (!isCorrectExercise && line == "-" + exerciseName)
               {
                   isCorrectExercise = true;
                   continue;
               }

               if (isCorrectExercise)
               {
                   if (line.StartsWith("&"))
                   {
                       setNumber++;
                   }
                   else if (line.StartsWith("-") || regexDay.IsMatch(line))
                   {
                       break;
                   }
               }
           }
       }
       weeklyRoutineReader.Close();
       return "set" + setNumber.ToString();
   }
   
   //Booleans

   public static bool CheckIfExerciseExists(string exerciseName)
   {
       StreamReader exercisesReader =
           new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/Exercises.txt",
               Encoding.UTF8);

       Regex exerciseRegex = new Regex(@"^\d+\.\s");
       string line;
       List<string> temp = new List<string>();
       while ((line = exercisesReader.ReadLine()) != null)
       {
           if (exerciseRegex.IsMatch(line))
           {
               temp.Add(line.Substring(line.IndexOf(" ") + 1));
           }
       }

       var bestMatch = Process.ExtractOne(exerciseName, temp);
       exercisesReader.Close();
       if (bestMatch != null && bestMatch.Score > 60)
           return true;

       return false;
   }
   
   public static bool CheckForMultipleExerciseList(string exerciseName) //Checks if a list is possible with closest matches
   {
       StreamReader exercisesReader =
           new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/Exercises.txt",
               Encoding.UTF8);

       Regex exerciseRegex = new Regex(@"^\d+\.\s");
       string line;
       List<string> temp = new List<string>();
       List<string> exercises = new List<string>();
       while ((line = exercisesReader.ReadLine()) != null)
       {
           if (exerciseRegex.IsMatch(line))
           {
               temp.Add(line.Substring(line.IndexOf(" ") + 1));
           }
       }

       foreach (var exercise in temp)
       {
           int score = Fuzz.PartialRatio(exerciseName.ToLower(), exercise.ToLower());

           if (score > 66)
           {
               exercises.Add(exercise);
           }
       }

       if (exercises != null && exercises.Count > 1)
       {
           return true;
       }
       return false;
   }

   public static bool IsRestDay(string day)
   {
       StreamReader weeklyRoutineReader = new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/WeeklyRoutine.txt", Encoding.UTF8);
       string line;
       bool isDay = false;
       Regex regexDay = new Regex("^(Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday)$");
       while ((line = weeklyRoutineReader.ReadLine()) != null)
       {
           if (regexDay.IsMatch(line))
           {
               if (line == day)
               {
                   isDay = true;
               }
               else if (isDay)
               {
                   break;
               }
           }
           else if (isDay)
           {
               if (line.StartsWith("+"))
               {
                   return true;
               }
               else
               {
                   return false;
               }
                   
           }
       }

       return default;
   }

   public static bool CheckIfDayEmpty(string day)
   {
       StreamReader weeklyRoutineReader = new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/WeeklyRoutine.txt", Encoding.UTF8);
       string line;
       bool isDay = false;
       Regex regexDay = new Regex("^(Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday)$");
       while ((line = weeklyRoutineReader.ReadLine()) != null)
       {
           if (line == "")
               continue;
           if (regexDay.IsMatch(line))
           {
               if (line == day)
               {
                   isDay = true;
               }
               else if (isDay)
               {
                   return true;
               }
           }
           else if (isDay)
           {
               return false;
           }
       }
       if (isDay)
       {
           return true;
       }
       weeklyRoutineReader.Close();
       return default;
   }

   public static bool CheckIfExerciseExistsInWeeklyRoutine(string exerciseName, string day)
   {
       StreamReader weeklyRoutineReader = new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/WeeklyRoutine.txt", Encoding.UTF8);
       string line;
       bool isDay = false;
       Regex regexDay = new Regex("^(Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday)$");
       while ((line = weeklyRoutineReader.ReadLine()) != null)
       {
           if (regexDay.IsMatch(line))
           {
               if (line == day)
               {
                   isDay = true;
               }
               else if (isDay)
               { 
                   break;
               }
           }
           else if (isDay)
           {
               if (line == "-" + exerciseName)
               {
                   return true;
               }
           }
       }
       weeklyRoutineReader.Close();
       return false;
   }

   public static bool CheckIfHadDonePreviousProgress(string day)
   {
       StreamReader previousProgressReader = new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/PreviousProgress.txt", Encoding.UTF8);
       string line;
       bool isDay = false;
       Regex regexDay = new Regex("^(Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday)$");
       while ((line = previousProgressReader.ReadLine()) != null)
       {
           if (line.Length > 14 && line.Substring(13) == day)
           {
               return true;
           }
       }
       previousProgressReader.Close();
       return false;
   }
   
   //Console prints

   public static void PrintTodaysWorkout()
   { 
       List<string> workouts = File.ReadAllLines("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/TodaysWorkout.txt", Encoding.UTF8).ToList();
       Console.WriteLine("=====================================================================");

       Console.WriteLine("Todays Workout: ");
       foreach (var workout in workouts)
       {
           if (workout.StartsWith("-"))
           {
               Console.WriteLine(workout);
           }
           if (workout.StartsWith("*"))
           {
               string[] sets = workout.Substring(1).Split("@"); 
               Console.WriteLine("Set " + sets[0].Substring(3, 1) + ":" + sets[1] + " Reps");
           }
       }
       /*Console.WriteLine("Last time you did:");
       foreach (var workout in workouts)
       {
           if (workout.StartsWith("-"))
           {
               Console.WriteLine(workout);
           }
           if (workout.StartsWith("&"))
           {
               string[] sets = workout.Substring(1).Split("@"); 
               Console.WriteLine("Set " + sets[0].Substring(3, 1) + ":" + sets[1] + " Reps");
           }
       }*/
       
   }

   //Print methods
   
   public static bool PrintWeeklyWorkout(string day)
   {
       using var sr = new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/WeeklyRoutine.txt",
           Encoding.UTF8);
       string line;
       Regex regexDay = new Regex("^(Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday)$");
       bool inRange = false;
       if (!CheckIfDayEmpty(day))
       {
           while ((line = sr.ReadLine()) != null)
           {
               if (regexDay.IsMatch(line))
               {
                   if (line == day)
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
                   if (line.StartsWith("-"))
                   {
                       Console.WriteLine(line);
                   }

                   if (line.StartsWith("&"))
                   {
                       string[] sets = line.Substring(1).Split("@");
                       Console.WriteLine("Set " + sets[0].Substring(3, 1) + ": " + sets[1] + " Reps");
                   }

                   if (line.StartsWith("+"))
                   {
                       Console.WriteLine("Rest Day");
                   }
               }
           }
           return true;
       }
       else
       {
           return false;
       }
   }

   public static void PrintLastWorkout(string exerciseName)
   {
       using StreamReader sr =
           new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/TodaysWorkout.txt",
               Encoding.UTF8);
       Console.WriteLine("Last Workout: ");
       string line;
       bool inRange = false;
       while ((line = sr.ReadLine()) != null)
       {
           if (line.StartsWith("-"))
           {
               if (line == "-" + exerciseName)
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
               if (line.StartsWith("&"))
               {
                   string[] sets = line.Substring(1).Split("@");
                   Console.WriteLine("Set " + sets[0].Substring(3, 1) + ": " + sets[1] + " Reps" + " With " + sets[2] + " KGs");

               }
           }
       }
   }

   public static void PrintExplanationForTodaysWorkout(string exerciseName)
   {
       using StreamReader sr =
           new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/TodaysWorkout.txt",
               Encoding.UTF8);
       string line;
       bool inRange = false;
       while ((line = sr.ReadLine()) != null)
       {
           if (line.StartsWith("-"))
           {
               if (line == "-" + exerciseName)
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
               if (line.StartsWith("?"))
               {
                   Console.WriteLine("- " + line.Substring(1));
               }
           }
       }
   }

   public static int CountSetsInTodaysWorkout(string exerciseName)
   {
       using StreamReader sr =
           new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/TodaysWorkout.txt",
               Encoding.UTF8);
       int num = 0;
       string line;
       bool inRange = false;
       while ((line = sr.ReadLine()) != null)
       {
           if (line.StartsWith("-"))
           {
               if (line == "-" + exerciseName)
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
               if (line.StartsWith("*"))
               {
                   num++;
               }
           }
       }

       return num;
   }
   
   //Run workout

   public static bool RunWorkout()
   {
       using StreamReader sr =
           new StreamReader("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/TodaysWorkout.txt",
               Encoding.UTF8);
       List<string> workouts = File.ReadAllLines("/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/TodaysWorkout.txt", Encoding.UTF8).ToList();
       string line;
       if (!(line = sr.ReadLine()).StartsWith("+"))
       {
           string exerciseName = "";
           bool isExercise = false;
           while ((line = sr.ReadLine()) != null)
           {
               if (line.StartsWith("-"))
               {
                   exerciseName = line.Substring(1);
                   Console.WriteLine("=====================================================================");
                   Console.WriteLine(line);
                   PrintExplanationForTodaysWorkout(exerciseName);
                   Console.WriteLine("=====================================================================");
                   PrintLastWorkout(exerciseName);
                   int countSets = CountSetsInTodaysWorkout(exerciseName);
                   Console.WriteLine("You have to do " + countSets + " Sets!");
               }

               if (line.StartsWith("*"))
               {
                   string[] sets = line.Substring(1).Split("@");
                   Console.WriteLine("=====================================================================");
                   Console.WriteLine($"The Goal for Set {sets[0].Substring(3, 1)} is {sets[1]} Reps");
                   Console.WriteLine("Enter the amount of KGs you lifted: ");
                   int kgs = ProgramRun.CheckNumberChoice();
                   string kgsString = kgs.ToString();
                   AddKGsToExerciseToTodaysWorkout(exerciseName, sets[0], kgsString);
               }
           }

           ExtportTodaysWorkoutToPreviousProgress();
           return true;
       }
       else
       {
           Console.WriteLine("Today is Rest Day");
           return false;
       }
   }

   public static void DeleteAllExercises(string day)
   {
       string path = "/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/WeeklyRoutine.txt";
       List<string> lines = new List<string>(File.ReadAllLines(path, Encoding.UTF8));
       List<string> updatedLines = new List<string>();

       bool isTargetDay = false;
       Regex regexDay = new Regex("^(Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday)$");

       foreach (var line in lines)
       {
           if (regexDay.IsMatch(line))
           {
               if (line == day)
               {
                   isTargetDay = true;
                   updatedLines.Add(line); // Keep the day header
                   continue;
               }
               else
               {
                   isTargetDay = false;
               }
           }

           if (!isTargetDay)
           {
               updatedLines.Add(line); // Keep lines not in the target day
           }
           // else: skip lines under the selected day
       }

       File.WriteAllLines(path, updatedLines, Encoding.UTF8);
       Console.WriteLine($"All exercises for {day} have been deleted.");
   }

   public static void MarkDayAsRestDay(string day)
   {
       string path = "/Users/danchogeorgiev/RiderProjects/FitnessTrackerPro/FitnessTrackerPro/WeeklyRoutine.txt";
       string[] lines = File.ReadAllLines(path);
       List<string> updatedLines = new List<string>();
       bool isDay = false;
       Regex regexDay = new Regex("^(Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday)$");

       foreach (string line in lines)
       {
           if (regexDay.IsMatch(line))
           {
               if (isDay)
                   isDay = false;

               if (line == day)
               {
                   updatedLines.Add(line);
                   updatedLines.Add("+Restday");
                   isDay = true;
                   continue;
               }
           }

           if (isDay)
           {
               if (regexDay.IsMatch(line)) 
               {
                   updatedLines.Add(line);
                   isDay = false;
               }
               
               continue;
           }

           updatedLines.Add(line);
       }

       File.WriteAllLines(path, updatedLines);
       Console.WriteLine($"{day} marked as rest day.");
   }

}