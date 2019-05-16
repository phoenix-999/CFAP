using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations.Schema;

namespace CFAPDataModel.Models
{
    [DataContract]
    public class User
    {
        public User()
        {
            this.UserGroups = new List<UserGroup>();
            
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required, MaxLength(70)]
        [Index(IsUnique = true)]
        public string UserName { get; set; }

        [DataMember]
        [Required]
        public string Password { get; set; }

        [DataMember]
        public bool IsAdmin { get; set; }

        [DataMember]
        public bool CanAddNewUsers { get; set; }

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


        public ICollection<int> GetUserGroupsId()
        {
            var groupsId = (from g in this.UserGroups
                            select g.Id).ToList();

            return groupsId;
        }
        public void LoadUserGroups(CFAPContext ctx)
        {
            //Для корректной загрузки данных нужен экземпляр контекста вызывающей строны
                ctx.Configuration.ProxyCreationEnabled = false;

            //Поучение данных о связанных сущностя необходимо для корректного добавления сущности User
            //В случае обычного ctx.Users.Add(newUser); без выборки связанных существующих данных
            //получим исключение "Не удаеться добавить сущности в коллекцию UserGroup.Users по причине невозможности удаления из Array фиксированной длинны типа"
            //Суть ошибки состоит в том, что при вызове метода ctx.Users.Add(newUser); все поля экземпляра помеаються как Added
            //Возникает конфликт первычных ключей в БД

            var goupsId = this.GetUserGroupsId();

            //LINQ to Entities не умеет вызывать методы
            var groups = (from g in ctx.UserGroups
                          where goupsId.Contains(g.Id)
                          select g).ToList();

            this.UserGroups = groups;
        }
    }
}
