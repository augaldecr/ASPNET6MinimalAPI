var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

var games = new List<Game>
{
    new Game { Id = 1, Name = "Super Mario", Platform="SNES", State= true },
    new Game { Id = 2, Name = "Sonic the Hedgehog", Platform="Sega", State= true },
    new Game { Id = 3, Name = "Zelda: The ocarine of time", Platform="N64", State= true },
    new Game { Id = 4, Name = "Minecraft", Platform="PC", State= true },
    new Game { Id = 5, Name = "PES 2002", Platform="PSX", State= true },
    new Game { Id = 6, Name = "Pong", Platform="Arcade", State= true },
    new Game { Id = 7, Name = "Halo", Platform="XBOX", State= true },
};

//app.MapGet("/", () => "Hola mundo");

app.MapGet("/", () => games);

app.MapGet("/{id:int}", (int id) => games.Find( game => game.Id == id));

app.MapPost("/", (Game game) =>
{
    int id  = games.MaxBy(game => game.Id).Id + 1;
    game = game with { Id = id };
    games.Add(game);
    return game.Id;
});

app.MapPut("/{id:int}", (Game game) =>
{
    games.RemoveAll(game => game.Id == game.Id);
    games.Add(game);
});

app.MapDelete("/{id:int}", (int id) => games.RemoveAll(game => game.Id == id));

app.Run();

record Game
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Platform { get; init; }
    public bool State { get; init; }
}