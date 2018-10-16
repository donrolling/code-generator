using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models.Entities;

namespace Data.Models.ViewModels {
	public class ProjectDetailViewModel {
		public Project Project { get; set; }
		public IEnumerable<Template> Templates { get; set; }
	}
}
