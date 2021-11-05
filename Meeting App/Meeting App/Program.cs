using System;
using System.IO;
using System.Collections.Generic;

namespace Meeting_App
{
    public class Meeting
    {
        private string title;
        private string location;
        private DateTime startDateTime;
        private DateTime endDateTime;

        public Meeting() { }

        public Meeting(string title, string location, DateTime start, DateTime end)
        {
            Title = title;
            Location = location;
            StartDateTime = start;
            EndDateTime = end;
        }
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }

        public string Location
        {
            get
            {
                return this.location;
            }
            set
            {
                this.location = value;
            }
        }

        public DateTime StartDateTime
        {
            get
            {
                return this.startDateTime;
            }
            set
            {
                this.startDateTime = value;
            }
        }

        public DateTime EndDateTime
        {
            get
            {
                return this.endDateTime;
            }
            set
            {
                this.endDateTime = value;
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<Meeting> currentCalendar = new List<Meeting>();
            bool running = true;
            do
            {
                Console.WriteLine("Welcome to the Meeting App\n");
                Console.WriteLine("What would you like to do?\n1. Load an existing Calendar\n2. Create new Calendar\n3. Exit program");
                string calendarMenuInput = Console.ReadLine();
                try
                {
                    switch (calendarMenuInput)
                    {
                        case "1":
                            Console.WriteLine("Where will this calendar be loaded from? (Enter a file path)");
                            string filePath = Console.ReadLine();
                            currentCalendar = ReadCalendarFromFile(filePath);
                            DisplayCalendarMenu(currentCalendar);
                            WriteCalendarToFile(currentCalendar, filePath);
                            break;

                        case "2":
                            Console.WriteLine("Where would you like to save this calendar? (Enter a file path)");
                            string filePathForSave = Console.ReadLine();
                            DisplayCalendarMenu(currentCalendar);
                            WriteCalendarToFile(currentCalendar, filePathForSave);
                            break;

                        case "3":
                            Console.WriteLine("Goodbye.");
                            running = false;
                            break;

                        default:
                            Console.WriteLine("Please choose option 1, 2, or 3.\n");
                            break;
                    }
                }
                catch (FormatException err)
                {
                    Console.WriteLine(err.Message + "\n");
                }
                catch (IOException err)
                {
                    Console.WriteLine(err.Message + "\n");
                }
                catch (Exception catchAll)
                {
                    Console.WriteLine(catchAll.Message + "\n");
                }
            }
            while (running == true);
        }

        static List<Meeting> ReadCalendarFromFile(string filePath)
        {
            List<Meeting> existingCalendar = new List<Meeting>();
            string[] fileContents = File.ReadAllLines(filePath);

            foreach (string line in fileContents)
            {
                string[] lineData = line.Split(",");
                string title = lineData[0];
                string location = lineData[1];
                DateTime start = DateTime.Parse(lineData[2]);
                DateTime end = DateTime.Parse(lineData[3]);

                Meeting existingMeeting = new Meeting(title, location, start, end);
                existingCalendar.Add(existingMeeting);
            }
            return existingCalendar;
        }

        static void WriteCalendarToFile(List<Meeting> calendar, string filePath)
        {
            string dataString = "";
            foreach (Meeting meeting in calendar)
            {
                dataString += (meeting.Title + "," + meeting.Location + "," + meeting.StartDateTime.ToString() + "," + meeting.EndDateTime.ToString() + "\n");
            }
            File.WriteAllText(@filePath, dataString);
        }

        static void DisplayCalendarMenu(List<Meeting> calendar)
        {
            bool display = true;
            do
            { 
             Console.WriteLine("What would you like to do?\n1. Add meeting to calendar\n2. Remove meeting\n3. View Schedule\n4. Go back");
             string menuInput = Console.ReadLine();
                try
                {
                    switch (menuInput)
                    {
                        case "1":
                            Console.WriteLine("Add a meeting");
                            AddMeetingToCalendar(calendar);
                            break;
                        case "2":
                            Console.WriteLine("Remove Meeting");
                            break;
                        case "3":
                            Console.WriteLine("View Schedule");
                            PrintCalendar(calendar);
                            break;
                        case "4":
                            display = false;
                            break;
                        default:
                            Console.WriteLine("Please select one of the menu items.");
                            break;
                    }
                }
                catch(Exception catchAll)
                {
                    Console.WriteLine(catchAll.Message + "\n");
                    Console.WriteLine(catchAll.StackTrace + "\n");
                }
            }
            while (display == true);
        }

        static void AddMeetingToCalendar(List<Meeting> calendar)
        {
            Console.Write("Meeting Title: ");
            string title = Console.ReadLine();
            Console.Write("Meeting Location: ");
            string location = Console.ReadLine();
            Console.Write("Meeting Start Date/Time: ");
            DateTime start = DateTime.Parse(Console.ReadLine());
            Console.Write("Meeting End Date/Time: ");
            DateTime end = DateTime.Parse(Console.ReadLine());      
            
            Meeting newMeeting = new Meeting(title, location, start, end);
            calendar.Add(newMeeting);
            /*bool conflict = false;

            for(int i = 0; i < calendar.Count; i++)
            {
                if ((newMeeting.StartDateTime >= calendar[i].StartDateTime && newMeeting.StartDateTime <= calendar[i].EndDateTime) ||
                    (newMeeting.StartDateTime <= calendar[i].StartDateTime && newMeeting.EndDateTime > calendar[i].StartDateTime))
                {
                    Console.WriteLine("This meeting conflicts with: {0} - {1} ({2} - {3})",
                        calendar[i].Title, calendar[i].Location, calendar[i].StartDateTime, calendar[i].EndDateTime + "\n");

                    Console.WriteLine("Do you wish to schedule meeting anyway? (y/n)");
                    char choice = char.Parse(Console.ReadLine());

                    if (choice == 'y' || choice == 'Y')
                    {
                        calendar.Add(newMeeting);
                        conflict = true;
                    }
                    else continue;
                }
                if (!conflict)
                {
                    calendar.Add(newMeeting);
                }
            }*/
        }

        static void PrintCalendar(List<Meeting> calendar)
        {
            DateTime scheduleStart = DateTime.Today.AddHours(8);
            DateTime scheduleEnd = DateTime.Today.AddHours(17);

            do
            {
                Console.Write(scheduleStart.ToString("g") + " | ");
                foreach (Meeting meeting in calendar)
                {
                    if ((scheduleStart >= meeting.StartDateTime) && (scheduleStart < meeting.EndDateTime))
                    {
                        Console.Write(meeting.Title + " at " + meeting.Location + " ");
                    }
                }
                Console.WriteLine();
                scheduleStart = scheduleStart.AddMinutes(30);
            }
            while (scheduleStart <= scheduleEnd);
        }

        static void ValidateFilePath(string filePath)
        {
            bool valid = File.Exists(filePath);

            if (!valid) throw new Exception("Error, the file path is incorrect.");
        }

    }
}



