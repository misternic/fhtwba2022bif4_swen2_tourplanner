using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using TourPlanner.Common;
using TourPlanner.Common.DTO;
using TourPlanner.DAL;
using TourPlanner.DAL.Repositories;

namespace TourPlanner.BL
{
    public class TourController
    {
        static readonly BaseRepository<TourDto> tourRepository = new TourRepository(DbContext.GetInstance());

        public static IEnumerable<TourDto> GetItems(string filter)
        {
            if (filter == null || filter == string.Empty)
                return tourRepository.Get();
            else
                return tourRepository.Get().ToList().Where(t => Regex.Match(t.ToJson(), filter, RegexOptions.IgnoreCase).Success);
        }

        public static bool AddItem(TourDto tourDto)
        {
            return tourRepository.Insert(tourDto);
        }

        public static bool UpdateItem(TourDto tourDto)
        {
            return tourRepository.Update(tourDto);
        }

        public static bool RemoveItem(TourDto tourDto)
        {
            return tourRepository.Delete(tourDto.Id);
        }
    }
}
