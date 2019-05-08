using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CFAPDataModel.Models
{
    public class Accountable
    {

        public Accountable()
        {
            this.Projects = new HashSet<Project>();
            this.BudgetItems = new HashSet<BudgetItem>();
            this.Descriptions = new HashSet<DescriptionItem>();
            this.Users = new HashSet<User>();
            this.Summaries = new HashSet<Summary>();
        }
        public int Id { get; set; }

        [Required]
        public string AccountableName { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<BudgetItem> BudgetItems { get; set; }

        public virtual ICollection<DescriptionItem> Descriptions { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Summary> Summaries { get; set; }

    }
}
