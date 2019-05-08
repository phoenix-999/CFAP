using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace CFAPDataModel.Models
{
    [DataContract]
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

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required, MaxLength(70)]
        public string UserName { get; set; }

        [DataMember]
        [Required]
        public string Password { get; set; }

        [DataMember]
        public bool IsAdmin { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [DataMember]
        public virtual ICollection<Project> Projects { get; set; }

        [DataMember]
        public virtual ICollection<BudgetItem> BudgetItems { get; set; }

        [DataMember]
        public virtual ICollection<DescriptionItem> Descriptions { get; set; }

        [DataMember]
        public virtual ICollection<Accountable> Accountables { get; set; }

        [DataMember]
        public virtual ICollection<Summary> Summaries { get; set; }


        public void EncriptPassword()
        {
            HashAlgorithm mhash = new SHA1CryptoServiceProvider();
            byte[] bytValue = Encoding.UTF8.GetBytes(this.Password);
            byte[] bytHash = mhash.ComputeHash(bytValue);
            mhash.Clear();
            this.Password =  Convert.ToBase64String(bytHash);
        }
    }
}
