using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using NLog;
using CFAPDataModel.Models;

namespace CFAPDataModel.Models.Exceptions
{
    [DataContract]
    public class TryChangeReadOnlyFiledException
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

        [DataMember]
        public int FieldId { get; set; }

        [DataMember]
        public string FieldName { get; set; }

        Type EntityType { get; set; }

        public TryChangeReadOnlyFiledException(Type enityType, int fieldId, string fieldname, User user)
        {
            this.EntityType = enityType;
            this.FieldId = fieldId;
            this.FieldName = fieldname != null ? fieldname : "";
            this.errorString = CreateErrorString();
            string logErrorString = string.Format("Entity type = {0}, Message:\n{1}", this.EntityType.FullName, this.errorString);

            Log.Error(logErrorString);
        }

        string errorString;

        string CreateErrorString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("Попытка изменить значение поля только для чтения\n");
            builder.Append(string.Format("Id = {0}, наименование = {1}", this.FieldId, this.FieldName));

            return builder.ToString();
        }

        [DataMember]
        public virtual string Message
        {
            get
            {
                return this.errorString;
            }
            set { }
        }

    }
}
