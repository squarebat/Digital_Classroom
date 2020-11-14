using DigiClassroom.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigiClassroom.Models.Repositories
{
    public interface IClassroomUserRepository
    {
        ClassroomUser Add(ClassroomUser classroomUser);
        IEnumerable<ClassroomUser> GetClassroomMentors(int id);
        IEnumerable<ClassroomUser> GetClassroomStudents(int id);

    }
    public class SQLClassroomUserRepository : IClassroomUserRepository
    {
        private readonly ApplicationDbContext context;
        public SQLClassroomUserRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        ClassroomUser IClassroomUserRepository.Add(ClassroomUser classroomUser)
        {
            context.ClassroomUsers.Add(classroomUser);
            context.SaveChanges();
            return classroomUser;
        }
        IEnumerable<ClassroomUser> IClassroomUserRepository.GetClassroomMentors(int id)
        {
            return context.ClassroomUsers.Where(cu => cu.Role == "Mentor" && cu.ClassroomId == id).Include(au => au.AppUser);
        }
        IEnumerable<ClassroomUser> IClassroomUserRepository.GetClassroomStudents(int id)
        {
            return context.ClassroomUsers.Where(cu => cu.Role == "Student" && cu.ClassroomId == id).Include(au => au.AppUser);
        }
    }
}
