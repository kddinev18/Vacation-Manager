using DataAccessLayer;
using DataAccessLayer.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Logic
{
    // POCO class userd for transferring information about a vacation
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
        // Adds vacation
        public static void AddVacation(int userId, DateTime from, DateTime to, byte[] image, VacationManagerContext dbContext)
        {
            // Add the vacation
            dbContext.Vacations.Add(new Vacation()
            {
                UserId = userId,
                From = from,
                To = to,
                Image = image,
                Approoved = false,
                Published = DateTime.Now
            });
            // Save the changes into the database
            dbContext.SaveChanges();
        }

        // Deletes vacations that are expired
        public static void DeleteOldVacations(VacationManagerContext dbContext)
        {
            foreach (Vacation vacation in dbContext.Vacations)
            {
                // If the time of the stating the vacation is before now delete it
                if(vacation.From < DateTime.Now)
                {
                    // Deletes the vacation
                    dbContext.Vacations.Remove(vacation);
                }
            }
        }

        public static IEnumerable<VacationInformation> GetVacations(int userId, int pagingSize, int skipAmount, VacationManagerContext dbContext)
        {
            VacationManagerContext nesteddbContext = new VacationManagerContext();
            ICollection<VacationInformation> vacationsInformation = new List<VacationInformation>();
            IEnumerable<Vacation> vacations;
            // Checks if the user is admin
            if (UserLogic.CheckAuthorisation(userId, dbContext))
            {
                // If the user is admin
                vacations = dbContext.Vacations.Where(vacation=>vacation.Approoved == false).Skip(skipAmount).Take(pagingSize);
            }
            else
            {
                // If the user is not admin
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

        // Gets the count of the vacation
        public static int GetVacationsCount(int userId, VacationManagerContext dbContext)
        {
            // Checks if the user is admin
            if (UserLogic.CheckAuthorisation(userId, dbContext))
            {
                // If the user is admin
                // Return the count of the vacations that are not approved
                return dbContext.Vacations.Where(vacation => vacation.Approoved == false).Count();
            }
            else
            {
                // If the user is not admin
                // Returs the count of the vacation requested by the users
                return dbContext.Vacations.Where(vacation => vacation.UserId == userId).Count();
            }
        }

        // Aprooves the vacation
        public static void ApprooveVacation(int vacationId, VacationManagerContext dbContext)
        {
            // Assign the vacation aprooval
            dbContext.Vacations
                // Gets the vacations matching the vacation we want to aproove
                .Where(vacation => vacation.VacationId == vacationId)
                // Gets the first element
                .First()
                // Assign true to the aprooved property in vacaiton
                .Approoved = true;
            // Saves the canges to the database
            dbContext.SaveChanges();
        }
    }
}
