using DataAccessLayer;
using DataAccessLayer.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Logic
{
    public class VacationInformation
    {
        public int VacationId { get; set; }
        public string UserName { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool Approoved { get; set; }
        public byte[] Image { get; set; }
    }
    public static class VacationLogic
    {
        public static void AddVacation(int userId, DateTime from, DateTime to, byte[] image, VacationManagerContext dbContext)
        {
            dbContext.Vacations.Add(new Vacation()
            {
                UserId = userId,
                From = from,
                To = to,
                Image = image,
                Approoved = false,
                Published = DateTime.Now
            });
            dbContext.SaveChanges();
        }

        public static void DeleteOldVacations(VacationManagerContext dbContext)
        {
            foreach (Vacation vacation in dbContext.Vacations)
            {
                if(vacation.From < DateTime.Now)
                {
                    dbContext.Vacations.Remove(vacation);
                }
            }
        }

        public static IEnumerable<VacationInformation> GetVacations(int userId, int pagingSize, int skipAmount, VacationManagerContext dbContext)
        {
            VacationManagerContext nesteddbContext = new VacationManagerContext();
            ICollection<VacationInformation> vacationsInformation = new List<VacationInformation>();
            IEnumerable<Vacation> vacations;
            if (UserLogic.CheckAuthorisation(userId, dbContext))
            {
                vacations = dbContext.Vacations.Where(vacation=>vacation.Approoved == false).Skip(skipAmount).Take(pagingSize);
            }
            else
            {
                vacations = dbContext.Vacations.Where(vacation => vacation.UserId == userId).Skip(skipAmount).Take(pagingSize); ;
            }

            foreach (Vacation vacation in vacations)
            {
                vacationsInformation.Add(new VacationInformation()
                {
                    Approoved = vacation.Approoved,
                    From = vacation.From,
                    To = vacation.To,
                    Image = vacation.Image,
                    VacationId = vacation.VacationId,
                    UserName = nesteddbContext.Users.Where(user=>user.UserId==vacation.UserId).First().UserName,
                });
            }
            return vacationsInformation;
        }

        public static int GetVacationsCount(int userId, VacationManagerContext dbContext)
        {
            if (UserLogic.CheckAuthorisation(userId, dbContext))
            {
                return dbContext.Vacations.Where(vacation => vacation.Approoved == false).Count();
            }
            else
            {
                return dbContext.Vacations.Where(vacation => vacation.UserId == userId).Count();
            }
        }

        public static void ApprooveVacation(int vacationId, VacationManagerContext dbContext)
        {
            dbContext.Vacations.Where(vacation => vacation.VacationId == vacationId).First().Approoved = true;
            dbContext.SaveChanges();
        }
    }
}
