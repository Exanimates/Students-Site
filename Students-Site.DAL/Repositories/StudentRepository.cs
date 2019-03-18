﻿using System;
using System.Collections.Generic;
using System.Text;
using Students_Site.DAL.EF;
using Students_Site.DAL.Entities;

namespace Students_Site.DAL.Repositories
{
    public class StudentRepository : BaseRepository<Student>
    {
        public StudentRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
