﻿using System;
using StudentList.Domain.Actions;
using StudentList.Domain.States;
using StudentList.Models;

namespace StudentList.Domain.Reducers
{
    public static class FilterStudentReducer
    {
        public static FilterStudentsState Reduce(FilterStudentsState state, object action)
        {
            switch (action)
            {
                case FiltersApplied filterApplied:
                    return Reduce(filterApplied);
                case ResetAppliedFilters resetAppliedFilters:
                    return Reduce(resetAppliedFilters);
                case StudentListChanged studentListUpdated:
                    return Reduce(studentListUpdated);
                default:
                    return state;
            }
        }

        private static FilterStudentsState Reduce(StudentListChanged studentListUpdated)
        {
            return new FilterStudentsState(StudentFilter.Default);
        }

        private static FilterStudentsState Reduce(ResetAppliedFilters resetAppliedFilters)
        {
            return new FilterStudentsState(StudentFilter.Default);
        }

        private static FilterStudentsState Reduce(FiltersApplied filterApplied)
        {
            return new FilterStudentsState(filterApplied.Filters);
        }
    }
}