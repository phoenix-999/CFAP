using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CFAPDataModel.Models
{
    public class User
    {
        public User()
        {
            this.Projects = new HashSet<Project>();
            this.Summaries = new HashSet<Summary>();
            this.Accountables = new HashSet<Accountable>();
            this.Descriptions = new HashSet<DescriptionItem>();
            this.BudgetItems = new HashSet<BudgetItem>();
        }
        public int Id { get; set; }

        [Required, MaxLength(70)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<BudgetItem> BudgetItems { get; set; }

        public virtual ICollection<DescriptionItem> Descriptions { get; set; }

        public virtual ICollection<Accountable> Accountables { get; set; }

        public virtual ICollection<Summary> Summaries { get; set; }
    }
}
