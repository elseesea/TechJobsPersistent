using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechJobsPersistent.Models;
using TechJobsPersistent.ViewModels;
using TechJobsPersistent.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace TechJobsPersistent.Controllers
{
    public class HomeController : Controller
    {
        private JobDbContext context;
        private List<Employer> allEmployers;
        private List<Skill> allSkills;

        public HomeController(JobDbContext dbContext)
        {
            context = dbContext;
            allEmployers = context.Employers.ToList();
            allSkills = context.Skills.ToList();
    }

    public IActionResult Index()
        {
            List<Job> jobs = context.Jobs.Include(j => j.Employer).ToList();

            return View(jobs);
        }

        [HttpGet("/Add")]
        public IActionResult AddJob()
        {
            AddJobViewModel addJobViewModel = new AddJobViewModel(allEmployers, allSkills);
            return View(addJobViewModel);
        }

        [HttpPost]
        public IActionResult ProcessAddJobForm(AddJobViewModel addJobViewModel, string[] selectedSkills)
        {
            if (ModelState.IsValid)
            {
                Employer anER = context.Employers.Single(e => e.Id == addJobViewModel.EmployerId);
                Job newJob = new Job
                {
                    Name = addJobViewModel.Name,
                    EmployerId = addJobViewModel.EmployerId,
                    Employer = anER,
                    JobSkills = new List<JobSkill>()
                };

                List<Skill> possibleSkills = context.Skills.ToList();
                foreach (string selectedSkill in selectedSkills)
                {
                    foreach (Skill skill in possibleSkills)
                    {
                        if (selectedSkill.Equals(skill.Id.ToString()))
                        {
                            JobSkill jobskill = new JobSkill
                            {
                                JobId = anER.Id,
                                SkillId = skill.Id
                            };
                            if (!newJob.JobSkills.Contains(jobskill))
                            {
                                newJob.JobSkills.Add(jobskill);
                            }
                        }
                    }
                }

                context.Jobs.Add(newJob);
                context.SaveChanges();

                return Redirect("/Home");
            }
            else
            {
                foreach (Employer employer in allEmployers)
                {
                    addJobViewModel.Employers.Add(new SelectListItem
                    {
                        Value = employer.Id.ToString(),
                        Text = employer.Name
                    });
                }
                addJobViewModel.Skills = allSkills;
                return View("AddJob", addJobViewModel);
            }
        }

        public IActionResult Detail(int id)
        {
            Job theJob = context.Jobs
                .Include(j => j.Employer)
                .Single(j => j.Id == id);

            List<JobSkill> jobSkills = context.JobSkills
                .Where(js => js.JobId == id)
                .Include(js => js.Skill)
                .ToList();

            JobDetailViewModel viewModel = new JobDetailViewModel(theJob, jobSkills);
            return View(viewModel);
        }
    }
}
