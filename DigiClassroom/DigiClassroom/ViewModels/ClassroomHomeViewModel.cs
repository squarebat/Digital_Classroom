using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiClassroom.Models;
namespace DigiClassroom.ViewModels
{
    public class ClassroomHomeViewModel
    {
        public Classroom Classroom { get; set; }
        public string ClassroomUserRole { get; set; }
        public IEnumerable<BlackBoard> BlackBoards { get; set; }
        public IEnumerable<ClassroomUser> ClassroomMentors { get; set; }
        public IEnumerable<ClassroomUser> ClassroomStudents { get; set; }
        public IEnumerable<Invite> StudentInvites { get; set; } 
        public IEnumerable<Assignment> Assignments { get; set; }
        public BlackBoardViewModel BlackBoardViewModel { get; set; }
        public AssignmentViewModel AssignmentViewModel { get; set; }
    }
}
