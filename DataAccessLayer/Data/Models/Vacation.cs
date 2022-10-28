using System;
using System.Collections.Generic;

#nullable disable

namespace VacationManager
{
    public partial class Vacation
    {
        public int VacationId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DateTime DateCreated { get; set; }
        public bool HalfDayVacation { get; set; }
        public bool Aprooved { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
