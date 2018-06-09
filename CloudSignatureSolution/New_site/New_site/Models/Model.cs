using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace New_site.Models
{
    [Table("Models")]
    public class Model
    {
        [Key]
        public int Id { get; set; }
        public string UserID { get; set; }
        public string Tara_forma_scurta { get; set; }
        public string Tara { get; set; }
        public string Localitate { get; set; }
        public string Organizatie { get; set; }
        public string Departament { get; set; }
        public string Domeniu { get; set; }

        [NotMapped]
        public string Email { get; set; }
        public string Private_password { get; set; }
    }
}