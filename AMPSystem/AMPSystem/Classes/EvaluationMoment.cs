﻿using System;
using System.Collections.Generic;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class EvaluationMoment : ITimeTableItem
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Name { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public ICollection<Course> Courses { get; set; }

        public EvaluationMoment(DateTime startTime, DateTime endTime, ICollection<Room> rooms, ICollection<Course> courses )
        {
            StartTime = startTime;
            EndTime = endTime;
            Rooms = rooms;
            Courses = courses;
        }
    }
}