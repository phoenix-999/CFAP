﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CFAPDataModel.Models
{
    public class BudgetItem
    {

        public BudgetItem()
        {
            this.Projects = new HashSet<Project>();
            this.Users = new HashSet<User>();
            this.Accountables = new HashSet<Accountable>();
            this.Descriptions = new HashSet<DescriptionItem>();
            this.Summaries = new HashSet<Summary>();
        }

        public int Id { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Accountable> Accountables { get; set; }

        public virtual ICollection<DescriptionItem> Descriptions { get; set; }

        public virtual ICollection<Summary> Summaries { get; set; }
    }
}
