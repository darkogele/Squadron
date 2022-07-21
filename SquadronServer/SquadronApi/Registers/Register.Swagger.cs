namespace SquadronApi.Registers;

public static partial class Register
{
	public static IServiceCollection RegisterSwagger(this IServiceCollection services)
	{
		services.AddSwaggerGen(options =>
		{
			options.CustomSchemaIds(x => x.FullName);

			options.EnableAnnotations();
		});

		return services;

	}

	public static IApplicationBuilder UseSwaggerDoc(this IApplicationBuilder app)
	{
		app.UseSwagger();

		app.UseSwaggerUI(options => options.DocumentTitle = "File Upload API");

		return app;
	}
}