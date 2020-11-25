using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiClassroom.Models;
namespace DigiClassroom.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Invite> Invites { get; set; }
        public IEnumerable<ClassroomUser> UserClassrooms { get; set; }
    }
}
