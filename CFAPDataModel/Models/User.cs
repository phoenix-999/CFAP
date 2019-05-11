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
            this.UserGroups = new HashSet<UserGroup>();
            
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
        [Required]
        public virtual ICollection<UserGroup> UserGroups { get; set; }


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
