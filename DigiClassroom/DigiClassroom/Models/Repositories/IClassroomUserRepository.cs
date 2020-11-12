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
        
    }
}
