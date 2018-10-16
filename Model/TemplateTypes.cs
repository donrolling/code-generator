using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
	/// <summary>
	/// These Types represent template processors that have been written to handle these templates. 
	/// </summary>
	public enum TemplateType {
		Interface,
		Model,
		Procedure,
		Query,
		Repository,
		Service,
		Table,
		TableValuedFunction		
	}
}
