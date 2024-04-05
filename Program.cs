using System;
using System.Collections.Generic;
using System.Linq;

namespace PracticTask1
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, List<DateTime>> VacationDictionary = new()
            {
                ["Иванов Иван Иванович"] = new List<DateTime>(),
                ["Петров Петр Петрович"] = new List<DateTime>(),
                ["Юлина Юлия Юлиановна"] = new List<DateTime>(),
                ["Сидоров Сидор Сидорович"] = new List<DateTime>(),
                ["Павлов Павел Павлович"] = new List<DateTime>(),
                ["Георгиев Георг Георгиевич"] = new List<DateTime>()
            };
            List<string> WorkingDaysOfWeek = new() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            // Список отпусков сотрудников
            List<DateTime> dateList = new List<DateTime>();
            List<DateTime> SetDateList = new List<DateTime>();

            DateTime start = new DateTime(DateTime.Now.Year, 1, 1);
            int range = (start.AddYears(1) - start).Days;
            foreach (var VacationList in VacationDictionary)
            {
                List<DateTime> Vacations = new List<DateTime>();
                dateList = VacationList.Value;
                int iterationsCount = 0;
                for (int vacationCount = 28; vacationCount > 0;)
                {
                    iterationsCount++;
                    if(iterationsCount == 25) // Костыль для оптимизации
                    {
                        vacationCount = 28;
                        Vacations.Clear();
                        dateList.Clear();
                    }
                    Random random = new Random();
                    DateTime endDate = new DateTime();
                    var startDate = start.AddDays(random.Next(range));
                    if (WorkingDaysOfWeek.Contains(startDate.DayOfWeek.ToString()))
                    {
                        int[] vacationSteps = { 7, 14 };
                        int vacIndex = random.Next(vacationSteps.Length);

                        int difference = 0;
                        switch (vacationSteps[vacIndex])
                        {
                            case 7:
                                {
                                    if (vacationCount == 0) break;
                                    endDate = startDate.AddDays(7);
                                    difference = 7;
                                    break;
                                }
                            case 14:
                                {
                                    if (vacationCount == 0) break;
                                    if (vacationCount <= 7)
                                    {
                                        endDate = startDate.AddDays(7);
                                        difference = 7;
                                    }
                                    else
                                    {
                                        endDate = startDate.AddDays(14);
                                        difference = 14;
                                    }
                                    break;
                                }
                        }

                        // Проверка условий по отпуску
                        bool CanCreateVacation = false;
                        bool existStart = false;
                        bool existEnd = false;
                        if (!Vacations.Any(element => element >= startDate && element <= endDate))
                        {
                            if (!Vacations.Any(element => element.AddDays(3) >= startDate && element.AddDays(3) <= endDate)
                                && !Vacations.Any(element => element.Year == DateTime.Now.Year + 1)) // проверка на конец года
                            {
                                existStart = dateList.Any(element => element.AddMonths(1) >= startDate && element.AddMonths(1) >= endDate);
                                existEnd = dateList.Any(element => element.AddMonths(-1) <= startDate && element.AddMonths(-1) <= endDate);
                                if (!existStart || !existEnd)
                                    CanCreateVacation = true;
                            }
                        }
                        if (CanCreateVacation)
                        {
                            for (DateTime dt = startDate; dt < endDate; dt = dt.AddDays(1))
                            {
                                Vacations.Add(dt);
                                dateList.Add(dt);
                            }
                            vacationCount -= difference;
                        }
                    }
                }
            }
            foreach (var VacationList in VacationDictionary)
            {
                SetDateList = VacationList.Value;
                Console.WriteLine("Дни отпуска " + VacationList.Key + " : ");
                for (int i = 0; i < SetDateList.Count; i++) { Console.WriteLine(SetDateList[i].ToShortDateString()); }
            }
            Console.ReadKey();
        }
    }
}
