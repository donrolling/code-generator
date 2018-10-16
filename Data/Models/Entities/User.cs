using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models.Entities {
	public class User {
		[Key]
		[Required]
		public long Id { get; set; }
		[Required]
		public string Email { get; set; }
		private bool _isActive = true;
		public bool IsActive {
			get { return _isActive; }
			set { _isActive = value; }
		}
		public long? CreatedById { get; set; }
		public long? UpdatedById { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}