﻿namespace WebApplication2.Models.Common
{
	public abstract class BaseEntity
	{
		public DateTime CreatedAt { get; set; }
		public DateTime ModifiedAt { get; set; }
	}
}
