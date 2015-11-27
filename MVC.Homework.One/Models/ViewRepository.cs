using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC.Homework.One.Models
{   
	public  class ViewRepository : EFRepository<View>, IViewRepository
	{

	}

	public  interface IViewRepository : IRepository<View>
	{

	}
}