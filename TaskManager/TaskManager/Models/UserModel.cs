using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace TaskManager.Models
{
    public class UserModel : IdentityUser<int>
    {
        //public string Name { get; set; }

        public ICollection<TaskModel> Tasks { get; set; }
    }
}
