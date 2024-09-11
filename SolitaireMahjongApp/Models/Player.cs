using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireMahjongApp.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public DateTime? RegistrationDate { get; set; }
    }
}
