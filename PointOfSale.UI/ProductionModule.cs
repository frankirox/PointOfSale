﻿using System;
using System.Configuration;
using Autofac;
using Autofac.Features.ResolveAnything;
using PointOfSale.Domain;
using PointOfSale.SqlDataAccess;

namespace PointOfSale.ConsoleApp
{
	public class ProductionModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["pointofsale"].ConnectionString;

			builder.RegisterTypes(typeof(MySqlItemRegistry))
				   .Where(t => t.Name.StartsWith("MySql", StringComparison.Ordinal))
				   .WithParameter("connectionString", connectionString)
				   .AsImplementedInterfaces();

			builder.RegisterType<ConsolePosDisplay>().AsImplementedInterfaces();
			builder.RegisterType<ConsoleReceiptFactory>().AsImplementedInterfaces();

			builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
		}
	}
}

