using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SmsApi.Models
{
    [Table("sms_messages")]
    public class SmsMessage
    {
        public int Id { get; set; }
        public string? Phone { get; set; }
        public string? Message { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
