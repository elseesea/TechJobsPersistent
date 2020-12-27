using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TechJobsPersistent.Models;

namespace TechJobsPersistent.ViewModels
{
    public class AddJobViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name must be at least 3 characters & at most 100 characters", MinimumLength = 3)]
        public string Name { get; set; }
        public int EmployerId { get; set; }
        public List<SelectListItem> Employers { get; set; } = new List<SelectListItem>();
        public List<Skill> Skills { get; set; } = new List<Skill>();

        public AddJobViewModel(List<Employer> employers, List<Skill> skills)
        {
            //Employers = new List<SelectListItem>();
            foreach (Employer employer in employers)
            {
                Employers.Add(new SelectListItem
                {
                    Value = employer.Id.ToString(),
                    Text = employer.Name
                });
            }

            foreach (Skill skill in skills)
            {
                Skills.Add(new Skill
                {
                    Id = skill.Id,
                    Name = skill.Name,
                    Description = skill.Description
                });
            }
        }

        public AddJobViewModel()
        {

        }

    } // class
} // namespace
