﻿using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Context;

public class AppDbContext:DbContext
{

	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{

	}

	public DbSet<Slider> Sliders { get; set; } = null!;
	public DbSet<Service> Services { get; set; } = null!;
}
