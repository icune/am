namespace AdvMusicTest;

static class Server
{
    public static async Task Run(WebApplicationBuilder builder)
    {
        
        
        
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "Adv Music Test Task", Version = "v1" });
            c.EnableAnnotations();
        });

        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MinimalApp v1");
            });
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}